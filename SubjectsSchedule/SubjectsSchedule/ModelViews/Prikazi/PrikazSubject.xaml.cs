using SubjectsSchedule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SubjectsSchedule.ModelViews.Prikazi
{
    /// <summary>
    /// Interaction logic for PrikazSubject.xaml
    /// </summary>
    public partial class PrikazSubject : UserControl, INotifyPropertyChanged
    {
        public Subject SelectedSubject
        {
            get
            {
                return SubjectHandler.Instance.SelectedSubject;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public PrikazSubject()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).Predmeti_Show();
        }
    }
}
