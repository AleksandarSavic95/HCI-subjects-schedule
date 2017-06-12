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
    /// Interaction logic for PrikazClassroom.xaml
    /// </summary>
    public partial class PrikazClassroom : UserControl, INotifyPropertyChanged
    {

        public Classroom SelectedClassroom
        {
            get
            {
                return ClassroomHandler.Instance.SelectedClassroom;
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

        public PrikazClassroom()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
