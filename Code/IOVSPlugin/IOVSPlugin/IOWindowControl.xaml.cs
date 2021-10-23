﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="IOWindowControl"/> class.
        /// </summary>
        public IOWindowControl()
        {
            DataContext = this;
            this.InitializeComponent();

            Posts = new List<SOPost>();

            SOPost sample = new SOPost();
            sample.Score = 14;
            sample.Title = "This is a sample stack overflow post question!!";
            sample.IsAnswered = true;
            sample.AnswerCount = 3;

            Posts.Add(sample);
            Posts.Add(sample);
            Posts.Add(sample);
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

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(queryTextBox.Text))
            {
                queryTextBox.Text = "Enter the search query...";
                queryTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "IOWindow");
        }
    }
}