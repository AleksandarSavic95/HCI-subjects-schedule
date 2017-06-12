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
            demoNumber = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.demoModeThread.Abort();
            Console.WriteLine("abortirao demoModeThread");
            currentDemoDescription.Text = demoNumber + ". abortirao demoModeThread \n"
                + currentDemoDescription.Text + "\n";
        }

        private void currentDemoDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("textChanged u Demo modu, demoNumber ++");
            demoNumber++;
        }
    }
}
