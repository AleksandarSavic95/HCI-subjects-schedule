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
    /// Interaction logic for TabelaSubject.xaml
    /// </summary>
    public partial class TabelaSubject : UserControl, INotifyPropertyChanged
    {
        public List<Subject> Subjects
        {
            get
            {
                return SubjectHandler.Instance.Subjects;
            }
        }

        public TabelaSubject()
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
            OnPropertyChanged("Subjects");
        }

        private void NoviPredmet_Btn_Click(object sender, RoutedEventArgs e)
        {
            SubjectHandler.Instance.SetSelectedSubject((Subject)null, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).NoviPredmet_Show();
        }

        private void DataGrid_DblClick(object sender, RoutedEventArgs e)
        {
            if (SubjectsList.SelectedItem == null) return;

            SubjectHandler.Instance.SetSelectedSubject(SubjectsList.SelectedItem as Subject, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).Predmet_Show();
        }

        private void EditSubject(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    SubjectHandler.Instance.SetSelectedSubject(row.DataContext as Subject, (MainWindow)Window.GetWindow(this));
                    break;
                }
            ((MainWindow)Window.GetWindow(this)).NoviPredmet_Show();
        }

        private void DeleteSubject(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    SubjectHandler.Instance.Remove((row.DataContext as Subject).Id, (MainWindow)Window.GetWindow(this));
                    break;
                }
        }
    }
}
