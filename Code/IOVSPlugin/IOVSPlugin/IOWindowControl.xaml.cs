using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using IntelliOverflowAPI;

namespace IOVSPlugin
{
    /// <summary>
    /// Interaction logic for IOWindowControl.
    /// </summary>
    public partial class IOWindowControl : UserControl, INotifyPropertyChanged
    {
        private List<SOPost> _posts;
        public List<SOPost> Posts
        {
            get { return _posts; }
            set
            {
                if(_posts != value)
                {
                    _posts = value;
                    OnPropertyChanged();
                }
            }
        }

        private string recentSearch = "";
        private API.SortType sortType = API.SortType.RANKED;
        private Button[] sortButtons;
        private List<StackExchangeRequest> currentRequests;

        /// <summary>
        /// Initializes a new instance of the <see cref="IOWindowControl"/> class.
        /// </summary>
        public IOWindowControl()
        {
            DataContext = this;
            this.InitializeComponent();

            Posts = new List<SOPost>();
            currentRequests = new List<StackExchangeRequest>();

            sortType = API.SortType.RANKED;

            sortButtons = new Button[] { sortRankedBtn, sortScoreBtn, sortAnswersBtn, sortTagsBtn, sortDateBtn };

            SortButtonVisibility();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            if (queryTextBox.Text == "Enter the search query...")
            {
                queryTextBox.Text = "";
                queryTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        public async void DoSearch(string query, bool updateSearchBox=true)
        {
            if (query == recentSearch)
            {
                return;
            }

            postBoxSpinner.Visibility = Visibility.Visible;

            Task<StackExchangeRequest> task = API.DoSearchAsync(query);
            StackExchangeRequest requestOutput = await task;
            List<Post> posts = API.SortResults(requestOutput);
            recentSearch = query;
            queryTextBox.Foreground = System.Windows.Media.Brushes.Black;

            List<SOPost> newPosts = new List<SOPost>();

            foreach (Post p in posts)
            {
                SOPost nextPost = new SOPost();
                nextPost.Tags = p.tags;
                nextPost.IsAnswered = p.is_answered;
                nextPost.ViewCount = p.view_count;
                nextPost.AnswerCount = p.answer_count;
                nextPost.Score = p.score;
                nextPost.CreationDate = p.creation_date;
                nextPost.QuestionID = p.question_id;
                nextPost.Title = p.title;
                nextPost.Link = p.link;

                newPosts.Add(nextPost);
            }

            Posts = newPosts;

            postBoxSpinner.Visibility = Visibility.Hidden;

            if (updateSearchBox)
            {
                queryTextBox.Text = query;
            }
        }

        public async void DoSearch(List<string> queries, bool updateSearchBox=true)
        {
            postBoxSpinner.Visibility = Visibility.Visible;


            currentRequests.Clear();
            foreach (string query in queries)
            {
                Task<StackExchangeRequest> task = API.DoSearchAsync(query);
                StackExchangeRequest requestOutput = await task;
                currentRequests.Add(requestOutput);
            }

            SortPostResults();

            queryTextBox.Foreground = System.Windows.Media.Brushes.Black;

            postBoxSpinner.Visibility = Visibility.Hidden;

            if (updateSearchBox)
            {
                queryTextBox.Text = queries[0];
            }
        }

        private void SortPostResults()
        {
            List<SOPost> newPosts = new List<SOPost>();
            List<Post> posts = API.SortResults(currentRequests, sortType);

            foreach (Post p in posts)
            {
                // Filter out duplicates
                bool cont = false;
                foreach (SOPost sop in newPosts)
                {
                    if (sop.QuestionID == p.question_id)
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont)
                {
                    continue;
                }

                SOPost nextPost = new SOPost();
                nextPost.Tags = p.tags;
                nextPost.IsAnswered = p.is_answered;
                nextPost.ViewCount = p.view_count;
                nextPost.AnswerCount = p.answer_count;
                nextPost.Score = p.score;
                nextPost.CreationDate = p.creation_date;
                nextPost.QuestionID = p.question_id;
                nextPost.Title = p.title;
                nextPost.Link = p.link;

                newPosts.Add(nextPost);
            }

            Posts = newPosts;
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(queryTextBox.Text))
            {
                queryTextBox.Text = "Enter the search query...";
                queryTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(queryTextBox.Text) || queryTextBox.Text == "Enter the search query..."))
            {
                DoSearch(queryTextBox.Text, false);
            }
            else
            {
                MessageBox.Show("Search query box is empty, please enter a search term", "Error: Empty Search", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DoubleClickPost(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SOPost selected = postList.SelectedItem as SOPost;

            if (selected != null && !string.IsNullOrWhiteSpace(selected.Link))
            {
                IOWebBrowserCommand.Instance.RouteToLink(selected.Link);
            }
        }

        private void externalBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.DataContext is SOPost)
            {
                SOPost selected = (SOPost)btn.DataContext;

                if (selected != null && !string.IsNullOrWhiteSpace(selected.Link))
                {
                    System.Diagnostics.Process.Start(selected.Link);
                }
            }
        }

        private void sortButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            sortType = (API.SortType)Array.IndexOf(sortButtons, btn);

            SortPostResults();

            SortButtonVisibility();
        }

        private void SortButtonVisibility()
        {
            foreach (Button b in sortButtons)
            {
                b.IsEnabled = true;
            }

            sortButtons[(int)sortType].IsEnabled = false;
        }
    }
}