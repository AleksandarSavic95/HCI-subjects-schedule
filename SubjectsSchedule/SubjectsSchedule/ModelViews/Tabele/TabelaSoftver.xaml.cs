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

namespace SubjectsSchedule.ModelViews.Tabele
{
    /// <summary>
    /// Interaction logic for TabelaSoftver.xaml
    /// </summary>
    public partial class TabelaSoftver : UserControl, INotifyPropertyChanged
    {
        public List<Software> Softwares
        {
            get
            {
                return SoftwareHandler.Instance.Softwares;
            }
        }

        public TabelaSoftver()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Softwares");
        }

        private void NoviSoftver_Btn_Click(object sender, RoutedEventArgs e)
        {
            SoftwareHandler.Instance.SetSelectedSoftware((Software)null, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).NoviSoftver_Show();
        }

        private void DataGrid_DblClick(object sender, RoutedEventArgs e)
        {
            if (SoftwaresList.SelectedItem == null) return;

            SoftwareHandler.Instance.SetSelectedSoftware(SoftwaresList.SelectedItem as Software, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).Softver_Show();
        }

        private void EditSoftware(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    SoftwareHandler.Instance.SetSelectedSoftware(row.DataContext as Software, (MainWindow)Window.GetWindow(this));
                    break;
                }
            ((MainWindow)Window.GetWindow(this)).NoviSoftver_Show();
        }

        private void DeleteSoftware(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    SoftwareHandler.Instance.Remove((row.DataContext as Software).Id, (MainWindow)Window.GetWindow(this));
                    break;
                }
        }
    }
}
