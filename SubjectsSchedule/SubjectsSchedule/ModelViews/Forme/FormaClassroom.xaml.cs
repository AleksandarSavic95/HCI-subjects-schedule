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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SubjectsSchedule.Model;
using SubjectsSchedule.Utilities;
using System.ComponentModel;

namespace SubjectsSchedule.ModelViews.Forme
{
    /// <summary>
    /// Interaction logic for FormaClassroom.xaml
    /// </summary>
    public partial class FormaClassroom : UserControl, INotifyPropertyChanged
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

        public FormaClassroom()
        {
            InitializeComponent();

            DataContext = this;
        }

        // Adding classroom
        // TODO: Add validation for fields (requied fields and such validations)
        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Adding Classroom with data: {0}, {1}, {2}, {3}, {4}, {5}, {6}", Identificator.Text, Description.Text, Helper.ParseStringToInt(brojMijestaUpDown.Text),
                Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded), Helper.GetOSFromString(OperatingSystem.Text));

            ClassroomHandler.Instance.Add(Identificator.Text, Description.Text, Helper.ParseStringToInt(brojMijestaUpDown.Text),
                Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded),
                Helper.GetOSFromString(OperatingSystem.Text), (MainWindow)Window.GetWindow(this));

            MessageBox.Show("Ucionica uspesno dodata!");
        }
    }
}
