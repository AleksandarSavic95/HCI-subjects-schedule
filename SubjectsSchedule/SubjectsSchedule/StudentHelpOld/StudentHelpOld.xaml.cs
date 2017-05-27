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

namespace SubjectsSchedule.StudentHelpOld
{
    /// <summary>
    /// Interaction logic for StudentHelpOld.xaml
    /// </summary>
    public partial class StudentHelpOld : InterfaceWindow
    {
        public StudentHelpOld()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            // da radi za sve kontrole
            // DependencyObject focusedControl = FocusManager.GetFocusScope(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                Console.WriteLine("focusedControl je DependencyObject");
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void ShowDialog(string[] fields)
        {
            throw new NotImplementedException();
        }

        public void doThings(string param)
        {
            throw new NotImplementedException();
        }
    }
}
