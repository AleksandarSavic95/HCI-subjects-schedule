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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("aco");
            Console.WriteLine(Naziv_Input.Text);
            Console.WriteLine(Opis_Input.Text);
            Console.WriteLine(Datum_Input.Text);
            Console.WriteLine(Dugme_Button.ClickMode);

            if (string.IsNullOrEmpty(SelectedFieldOfStudy.Id))
                FieldOfStudyHanlder.Instance.Add(Id_Input.Text, Naziv_Input.Text, DateTime.ParseExact(Datum_Input.Text, "d.M.yyyy.", CultureInfo.InvariantCulture), Opis_Input.Text, (MainWindow)Window.GetWindow(this));
            else
                FieldOfStudyHanlder.Instance.Update(SelectedFieldOfStudy.Id, Id_Input.Text, Naziv_Input.Text, DateTime.ParseExact(Datum_Input.Text, "d.M.yyyy.", CultureInfo.InvariantCulture), Opis_Input.Text, (MainWindow)Window.GetWindow(this));

            Console.WriteLine("{0}", FieldOfStudyHanlder.Instance.FindById(Id_Input.Text).Description);
            MessageBox.Show("Smer uspesno dodat!");
            Console.WriteLine("aco");
        }
    }
}
