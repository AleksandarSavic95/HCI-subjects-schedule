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

namespace SubjectsSchedule.FormaClassroom
{
    /// <summary>
    /// Interaction logic for FormaClassroom.xaml
    /// </summary>
    public partial class FormaClassroom : Window
    {
        public FormaClassroom()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // isti kood u mainWindow.cs
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            // da radi za sve kontrole
            // DependencyObject focusedControl = FocusManager.GetFocusScope(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                Console.WriteLine("focusedControl je DependencyObject");
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                // HelpProvider.ShowHelp(str, this);
                // TODO: Riješiti nasljeđivanje MainWindow-a, preko interfejsa sa doThings()
                // ili preko neke klase koja naslj. Window, a koja ima samo doThings()
                
            }
        }
    }
}
