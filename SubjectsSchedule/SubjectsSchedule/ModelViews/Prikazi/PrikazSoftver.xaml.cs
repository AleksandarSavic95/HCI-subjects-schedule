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
    /// Interaction logic for PrikazSoftver.xaml
    /// </summary>
    public partial class PrikazSoftver : UserControl, INotifyPropertyChanged
    {
        public Software SelectedSoftware
        {
            get
            {
                return SoftwareHandler.Instance.SelectedSoftware;
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

        public PrikazSoftver()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).Softveri_Show();
        }
    }
}
