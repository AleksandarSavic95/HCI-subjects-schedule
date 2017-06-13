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
using System.Globalization;
using System.ComponentModel;

namespace SubjectsSchedule.ModelViews.Forme
{
    /// <summary>
    /// Interaction logic for FormaFieldOfStudy.xaml
    /// </summary>
    public partial class FormaFieldOfStudy : UserControl, INotifyPropertyChanged
    {

        public FieldOfStudy SelectedFieldOfStudy
        {
            get
            {
                return FieldOfStudyHanlder.Instance.SelectedFieldOfStudy;
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

        public FormaFieldOfStudy()
        {
            InitializeComponent();

            DataContext = this;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Id_Input.Text))
            {
                ValidationError = "Identifikator mora biti popunjen!";
                return false;
            }
            else if (Id_Input.Text.Length > 4)
            {
                ValidationError = "Identifikator mora biti do 4 karaktera!";
                return false;
            }
            else if (SelectedFieldOfStudy == null || SelectedFieldOfStudy.Id != Id_Input.Text)
            {
                if (FieldOfStudyHanlder.Instance.Has(Id_Input.Text))
                {
                    ValidationError = "Smer sa identicnim identifikatorom vec postoji!";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(Naziv_Input.Text))
            {
                ValidationError = "Naziv mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Opis_Input.Text))
            {
                ValidationError = "Opis mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Datum_Input.Text))
            {
                ValidationError = "Datum mora biti popunjen!";
                return false;
            }
            else
            {
                DateTime mock = new DateTime();
                if (!DateTime.TryParseExact(Datum_Input.Text, "%d.%M.%yyyy.", CultureInfo.InvariantCulture, DateTimeStyles.None, out mock))
                {
                    ValidationError = "Format datuma mora biti u formatu \"1.2.2012\"!";
                    return false;
                }
                else if (mock.Year < 1950)
                {
                    ValidationError = "Datum ne moze biti pre 1950-te godine!";
                    return false;
                }
            }

            ValidationError = null;
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            if (string.IsNullOrEmpty(SelectedFieldOfStudy.Id))
                FieldOfStudyHanlder.Instance.Add(Id_Input.Text, Naziv_Input.Text, DateTime.ParseExact(Datum_Input.Text, "d.M.yyyy.", CultureInfo.InvariantCulture), Opis_Input.Text, (MainWindow)Window.GetWindow(this));
            else
                FieldOfStudyHanlder.Instance.Update(SelectedFieldOfStudy.Id, Id_Input.Text, Naziv_Input.Text, DateTime.ParseExact(Datum_Input.Text, "d.M.yyyy.", CultureInfo.InvariantCulture), Opis_Input.Text, (MainWindow)Window.GetWindow(this));

            ((MainWindow)Window.GetWindow(this)).Smerovi_Show();
        }
    }
}
