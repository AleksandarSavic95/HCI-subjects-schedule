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
using SubjectsSchedule.Model;

namespace SubjectsSchedule.ModelViews.Tabele
{
    /// <summary>
    /// Interaction logic for TabelaClassroom.xaml
    /// </summary>
    public partial class TabelaClassroom : UserControl, INotifyPropertyChanged
    {

        public List<Classroom> Classrooms
        {
            get
            {
                return ClassroomHandler.Instance.Classrooms;
            }
        }

        public TabelaClassroom()
        {
            InitializeComponent();

            DataContext = this;

            // neće raditi ovdje, jer nisu sve komponente učitane
            //OnPropertyChanged("Classrooms");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void NovaUcionica_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClassroomHandler.Instance.SetSelectedClassroom((Classroom)null, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).NovaUcionica_Show();
        }

        private void DataGrid_DblClick(object sender, RoutedEventArgs e)
        {
            if (ClassroomsList.SelectedItem == null) return;

            ClassroomHandler.Instance.SetSelectedClassroom(ClassroomsList.SelectedItem as Classroom, (MainWindow)Window.GetWindow(this));
            ((MainWindow)Window.GetWindow(this)).Ucionica_Show();
        }

        private void EditClassroomDetails(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.GetType());
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    ClassroomHandler.Instance.SetSelectedClassroom(row.DataContext as Classroom, (MainWindow)Window.GetWindow(this));
                    break;
                }
            ((MainWindow)Window.GetWindow(this)).NovaUcionica_Show();
        }

        private void DeleteClassroom(object sender, RoutedEventArgs e)
        {
            Classroom c;
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    c = row.DataContext as Classroom;

                    (Window.GetWindow(this) as MainWindow).RemoveClassroomButton(c);
                    ClassroomHandler.Instance.Remove(c.Id, (MainWindow)Window.GetWindow(this));
                    TerminHandler.Instance.RemoveClassroom(c.Id);

                    break;
                }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("Classrooms");
        }
    }
}
