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

        public List<Software> Softwares
        {
            get
            {
                return SoftwareHandler.Instance.Softwares;
            }
        }

        private string _validationError;

        public string ValidationError
        {
            get
            {
                return _validationError;
            }
            set
            {
                _validationError = value;
                OnPropertyChanged("ValidationError");
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

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Identificator.Text))
            {
                ValidationError = "Identifikator mora biti popunjen!";
                return false;
            }
            else if (Identificator.Text.Length > 4)
            {
                ValidationError = "Identifikator mora biti do 4 karaktera!";
                return false;
            }
            else if (SelectedClassroom == null || SelectedClassroom.Id != Identificator.Text)
            {
                if (ClassroomHandler.Instance.Has(Identificator.Text))
                {
                    ValidationError = "Ucionica sa identicnim identifikatorom vec postoji, promenite identifikator!";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(Description.Text))
            {
                ValidationError = "Opis mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(brojMijestaUpDown.Text))
            {
                ValidationError = "Broj mesta mora biti popunjen!";
                return false;
            }

            // TODO add validation: ukoliko postoji softver odabran, proveriti da li se operativni sistemi softera poklapaju
            // sa operativnim sistemom koji zahteva ucionica

            ValidationError = null;
            return true;
        }

        /// <summary>Adding classroom</summary>
        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Adding Classroom with data: {0}, {1}, {2}, {3}, {4}, {5}, {6}", Identificator.Text, Description.Text, Helper.ParseStringToInt(brojMijestaUpDown.Text),
                Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded), Helper.GetOSFromString(OperatingSystem.Text));

            if (!Validate())
                return;

            if (string.IsNullOrEmpty(SelectedClassroom.Id))
            {
                Classroom created = 
                    ClassroomHandler.Instance.Add(Identificator.Text, Description.Text, Helper.ParseStringToInt(brojMijestaUpDown.Text),
                    Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded),
                    Helper.GetOSFromString(OperatingSystem.Text), (MainWindow)Window.GetWindow(this));

                TerminHandler.Instance.AddClassroom(created.Id);

                ((MainWindow)Window.GetWindow(this)).AddClassroomButton(created);
            }
            else
                ClassroomHandler.Instance.Update(SelectedClassroom.Id, Identificator.Text, Description.Text, Helper.ParseStringToInt(brojMijestaUpDown.Text),
                    Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded),
                    Helper.GetOSFromString(OperatingSystem.Text), (MainWindow)Window.GetWindow(this));

            if (SoftwaresList.SelectedItems != null && SoftwaresList.SelectedItems.Count > 0)
            {
                List<string> softwares = new List<string>();

                foreach (Software s in SoftwaresList.SelectedItems)
                    softwares.Add(s.Id);

                ClassroomHandler.Instance.FindById(Identificator.Text).InstalledSoftware = softwares;
            }

            ((MainWindow)Window.GetWindow(this)).Ucionice_Show();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).Ucionice_Show();
        }
    }
}
