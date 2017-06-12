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
    /// Interaction logic for TabelaFieldOfStudy.xaml
    /// </summary>
    public partial class TabelaFieldOfStudy : UserControl, INotifyPropertyChanged
    {
        public List<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                return FieldOfStudyHanlder.Instance.FieldsOfStudy;
            }
        }

        public TabelaFieldOfStudy()
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
            OnPropertyChanged("FieldsOfStudy");
        }

        private void NoviSmer_Btn_Click(object sender, RoutedEventArgs e)
        {
            FieldOfStudyHanlder.Instance.SetSelectedFieldOfStudy((FieldOfStudy)null, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).NoviSmer_Show();
        }

        private void DataGrid_DblClick(object sender, RoutedEventArgs e)
        {
            if (FieldsOfStudyList.SelectedItem == null) return;

            FieldOfStudyHanlder.Instance.SetSelectedFieldOfStudy(FieldsOfStudyList.SelectedItem as FieldOfStudy, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).Smer_Show();
        }

        private void EditFieldOfStudy(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    FieldOfStudyHanlder.Instance.SetSelectedFieldOfStudy(row.DataContext as FieldOfStudy, (MainWindow)Window.GetWindow(this));
                    break;
                }
            ((MainWindow)Window.GetWindow(this)).NoviSmer_Show();
        }

        private void DeleteFieldOfStudy(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    FieldOfStudyHanlder.Instance.Remove((row.DataContext as FieldOfStudy).Id, (MainWindow)Window.GetWindow(this));
                    break;
                }
        }
    }
}
