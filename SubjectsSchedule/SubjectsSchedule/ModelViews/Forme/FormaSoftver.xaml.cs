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
    /// Interaction logic for FormaSoftver.xaml
    /// </summary>
    public partial class FormaSoftver : UserControl, INotifyPropertyChanged
    {

        public Software SelectedSoftware
        {
            get
            {
                return SoftwareHandler.Instance.SelectedSoftware;
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

        public FormaSoftver()
        {
            InitializeComponent();

            DataContext = this;
        }

        private bool Validate()
        {
            Console.WriteLine(Identifikator.Text);
            Console.WriteLine(Naziv.Text);
            Console.WriteLine(Opis.Text);
            Console.WriteLine(Proizvodjac.Text);
            Console.WriteLine(OSComboBox.Text);
            Console.WriteLine(godinaIzdavanjaUpDown.Text);
            Console.WriteLine(cijenaUpDown.Text);
            Console.WriteLine(Sajt.Text);

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
            else if (SelectedSoftware == null || SelectedSoftware.Id != Identifikator.Text)
            {
                if (SoftwareHandler.Instance.Has(Identifikator.Text))
                {
                    ValidationError = "Ucionica sa identicnim identifikatorom vec postoji, promenite identifikator!";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(Naziv.Text))
            {
                ValidationError = "Naziv mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Opis.Text))
            {
                ValidationError = "Opis mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Proizvodjac.Text))
            {
                ValidationError = "Proizvodjac mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(godinaIzdavanjaUpDown.Text))
            {
                ValidationError = "Godina izdavanja mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(cijenaUpDown.Text))
            {
                ValidationError = "Cena mora biti popunjen!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Sajt.Text))
            {
                ValidationError = "Web sajt mora biti popunjen!";
                return false;
            }

            ValidationError = null;
            return true;
        }

        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            if (string.IsNullOrEmpty(SelectedSoftware.Id))
                SoftwareHandler.Instance.Add(Identifikator.Text, Naziv.Text, Helper.GetOSFromString(OSComboBox.Text),
                    Proizvodjac.Text, Sajt.Text, godinaIzdavanjaUpDown.Text, Helper.ParseStringToDouble(cijenaUpDown.Text),
                    Opis.Text, (MainWindow)Window.GetWindow(this));
            else
                SoftwareHandler.Instance.Update(SelectedSoftware.Id, Identifikator.Text, Naziv.Text, Helper.GetOSFromString(OSComboBox.Text),
                    Proizvodjac.Text, Sajt.Text, godinaIzdavanjaUpDown.Text, Helper.ParseStringToDouble(cijenaUpDown.Text),
                    Opis.Text, (MainWindow)Window.GetWindow(this));

            ((MainWindow)Window.GetWindow(this)).Softveri_Show();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).Softveri_Show();
        }
    }
}
