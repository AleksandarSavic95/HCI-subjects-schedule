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
    /// Interaction logic for FormaSubject.xaml
    /// </summary>
    public partial class FormaSubject : UserControl, INotifyPropertyChanged
    {

        public Subject SelectedSubject
        {
            get
            {
                return SubjectHandler.Instance.SelectedSubject;
            }
        }

        public List<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                return FieldOfStudyHanlder.Instance.FieldsOfStudy;
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

        public FormaSubject()
        {
            InitializeComponent();

            DataContext = this;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Identifikator.Text))
            {
                ValidationError = "Identifikator mora biti popunjen!";
                return false;
            }
            else if (Identifikator.Text.Length > 4)
            {
                ValidationError = "Identifikator mora biti do 4 karaktera!";
                return false;
            }
            else if (SelectedSubject == null || SelectedSubject.Id != Identifikator.Text)
            {
                if (SubjectHandler.Instance.Has(Identifikator.Text))
                {
                    ValidationError = "Ucionica sa identicnim identifikatorom vec postoji, promenite identifikator!";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(Opis.Text))
            {
                ValidationError = "Opis mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Naziv.Text))
            {
                ValidationError = "Naziv mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(velicinaGrupeUpDown.Text)) {
                ValidationError = "Velicina grupe mora biti popunjena!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(duzinaTerminaUpDown.Text))
            {
                ValidationError = "Duzina termina mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(brojTerminaUpDown.Text))
            {
                ValidationError = "Broj termina mora biti popunjen!";
                return false;
            }

            if (SmjerComboBox.SelectedValue == null || string.IsNullOrWhiteSpace(SmjerComboBox.SelectedValue.ToString()))
            {
                ValidationError = "Smer mora biti odabran!";
                return false;
            }
            else if (!FieldOfStudyHanlder.Instance.Has(SmjerComboBox.SelectedValue.ToString()))
            {
                ValidationError = "Nevalidan smer odabran!";
                return false;
            }

            // TODO add validation: ukoliko postoji softver odabran, proveriti da li se operativni sistemi softera poklapaju
            // sa operativnim sistemom koji zahteva predmet

            ValidationError = null;
            return true;
        }

        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            // Dodavanje novog softvera - novi softver
            if (string.IsNullOrEmpty(SelectedSubject.Id))
            {
                Subject created = SubjectHandler.Instance.Add(Identifikator.Text, Naziv.Text, FieldOfStudyHanlder.Instance.FindById(SmjerComboBox.SelectedValue.ToString()), Opis.Text, Helper.ParseStringToInt(velicinaGrupeUpDown.Text),
                    Helper.ParseStringToInt(duzinaTerminaUpDown.Text), Helper.ParseStringToInt(brojTerminaUpDown.Text),
                    Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded), Helper.GetOSFromString(OSComboBox.Text), (MainWindow)Window.GetWindow(this));

                TerminHandler.Instance.AddSubject(created.Id);
            }
            else
                SubjectHandler.Instance.Update(SelectedSubject.Id, Identifikator.Text, Naziv.Text, null, Opis.Text, Helper.ParseStringToInt(velicinaGrupeUpDown.Text),
                    Helper.ParseStringToInt(duzinaTerminaUpDown.Text), Helper.ParseStringToInt(brojTerminaUpDown.Text),
                    Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded), Helper.GetOSFromString(OSComboBox.Text), (MainWindow)Window.GetWindow(this));

            if (SoftwaresList.SelectedItems != null && SoftwaresList.SelectedItems.Count > 0)
            {
                List<string> softwares = new List<string>();

                foreach (Software s in SoftwaresList.SelectedItems)
                    softwares.Add(s.Id);

                SubjectHandler.Instance.FindById(Identifikator.Text).NeedsSoftware = softwares;
            }

            ((MainWindow)Window.GetWindow(this)).Predmeti_Show();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).Softveri_Show();
        }
    }
}
