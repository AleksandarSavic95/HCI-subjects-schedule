using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SubjectsSchedule.DemoMode
{
    /// <summary>
    /// Interaction logic for DemoModeWindow.xaml
    /// </summary>
    public partial class DemoModeWindow : Window
    {
        public int demoNumber;
        public DemoModeWindow()
        {
            InitializeComponent();
            demoNumber = 0;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AbortDemo();
        }

        public void AbortDemo()
        {
            MainWindow.demoModeThread.Abort();
            Console.WriteLine("abortirao demoModeThread");
            currentDemoDescription.Text = "\n Prekid demonstracije \n"
                + currentDemoDescription.Text + "\n";

            MainWindow.demoModeThread = null;
            this.Close();
        }

        private void currentDemoDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("textChanged u Demo modu, demoNumber ++");
            demoNumber++;
        }

        private void AbortDemo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AbortDemo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AbortDemo();
        }
    }
}
