using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using SubjectsSchedule;

namespace SubjectsSchedule
{
    /// <summary>
    /// Interaction logic for HelpViewer.xaml
    /// </summary>
    public partial class HelpViewer : Window
    {
        private JavaScriptControlHelper ch;
        public HelpViewer(string key, MainWindow originator)
        {
            InitializeComponent();

            DataContext = this;

            string curDir = Directory.GetCurrentDirectory();
            Console.WriteLine(curDir);
            // da bi ova putanja radila, za svaki help fajl moramo
            // postaviti property "Copy to output directory" na "Copy always"!
            string path = String.Format("{0}/HelpSystem/HelpContent/{1}.htm", curDir, key);
            Console.WriteLine("path: " + path);
            if (!File.Exists(path))
            {
                key = "error";
            }
            Uri u = new Uri(String.Format("file:///{0}/HelpSystem/HelpContent/{1}.htm", curDir, key));
            ch = new JavaScriptControlHelper(originator);
            wbHelp.ObjectForScripting = ch;
            wbHelp.Navigate(u);
        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoBack));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoForward));
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoForward();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // ovdje neki destroy, nesto??
        }

        private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Console.WriteLine("wb navigating...");
            StatusBlock.Text = "Navigating...";
        }

        private void wbHelp_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Console.WriteLine("wb NAVIGATED!");
            StatusBlock.Text = "Loading...";
        }

        private void wbHelp_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Console.WriteLine("wb LoadCompleted!");
            StatusBlock.Text = "Loaded!";
        }
    }
}
