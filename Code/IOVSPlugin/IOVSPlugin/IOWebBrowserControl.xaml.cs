using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace IOVSPlugin
{
    /// <summary>
    /// Interaction logic for IOWebBrowserControl.
    /// </summary>
    public partial class IOWebBrowserControl : UserControl
    {
        Stack previousPages;
        Stack forwardPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="IOWebBrowserControl"/> class.
        /// </summary>
        public IOWebBrowserControl()
        {
            this.InitializeComponent();
            previousPages = new Stack();
            forwardPages = new Stack();
        }

        public void GoToUri(string uri)
        {
            this.urlTextBox.Text = uri;

            previousPages.Push(this.webBrowser.Source.ToString());
            forwardPages.Clear();

            this.webBrowser.Navigate(uri.ToString());
        }

        private void urlTextBox_Enter(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // This is a KeyUp event
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                previousPages.Push(this.webBrowser.Source.ToString());
                forwardPages.Clear();
                Uri uri = new Uri(this.urlTextBox.Text, UriKind.RelativeOrAbsolute);

                this.webBrowser.Navigate(uri.ToString());
            }
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            previousPages.Push(this.webBrowser.Source.ToString());
            forwardPages.Clear();
            this.urlTextBox.Text = "https://www.stackoverflow.com/";
            this.webBrowser.Navigate("https://www.stackoverflow.com/");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (previousPages.Count > 0)
            {
                forwardPages.Push(this.webBrowser.Source.ToString());
                string uri = (string) previousPages.Pop();
                this.urlTextBox.Text = uri;
                this.webBrowser.Navigate(uri);
            }
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (forwardPages.Count > 0)
            {
                previousPages.Push(this.webBrowser.Source.ToString());
                string uri = (string)forwardPages.Pop();
                this.urlTextBox.Text = uri;
                this.webBrowser.Navigate(uri);
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser.Refresh();
        }
    }
}