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

namespace SubjectsSchedule.Forme
{
    /// <summary>
    /// Interaction logic for FormaSoftver.xaml
    /// </summary>
    public partial class FormaSoftver : UserControl
    {
        public FormaSoftver()
        {
            InitializeComponent();
        }

        // TODO: cena should be double
        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Identifikator.Text);
            Console.WriteLine(Naziv.Text);
            Console.WriteLine(Opis.Text);
            Console.WriteLine(Proizvodjac.Text);
            Console.WriteLine(OSComboBox.Text);
            Console.WriteLine(godinaIzdavanjaUpDown.Text);
            Console.WriteLine(cijenaUpDown.Text);
            Console.WriteLine(Sajt.Text);

            SoftwareHandler.Instance.Add(Identifikator.Text, Naziv.Text, Helper.GetOSFromString(OSComboBox.Text),
                Proizvodjac.Text, Sajt.Text, godinaIzdavanjaUpDown.Text, Helper.ParseStringToDouble(cijenaUpDown.Text),
                Opis.Text, (MainWindow) Window.GetWindow(this));

            MessageBox.Show("Uspesno dodat softver!");
        }
    }
}
