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
    /// Interaction logic for PrikazFieldOfStudy.xaml
    /// </summary>
    public partial class PrikazFieldOfStudy : UserControl, INotifyPropertyChanged
    {
        public FieldOfStudy SelectedFieldOfStudy
        {
            get
            {
                return FieldOfStudyHanlder.Instance.SelectedFieldOfStudy;
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
        public PrikazFieldOfStudy()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
