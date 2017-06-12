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
    public partial class FormaSubject : UserControl
    {

        public FormaSubject()
        {
            InitializeComponent();

            IsVisibleChanged += SetComboBoxItems;
        }

        private void SetComboBoxItems(object sender, DependencyPropertyChangedEventArgs args)
        {
            Console.WriteLine("Here!, {0}", FieldOfStudyHanlder.Instance.FieldsOfStudy.Count);
            SmjerComboBox.ItemsSource = FieldOfStudyHanlder.Instance.FieldsOfStudy;
            SmjerComboBox.DisplayMemberPath = "Name";
            SmjerComboBox.SelectedValuePath = "Id";
        }

        private void Potvrda_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Identifikator.Text);
            Console.WriteLine(Naziv.Text);
            Console.WriteLine(Opis.Text);
            Console.WriteLine(SmjerComboBox.SelectedValue);
            Console.WriteLine(OSComboBox.Text);
            Console.WriteLine(duzinaTerminaUpDown.Text);
            Console.WriteLine(brojTerminaUpDown.Text);

            SubjectHandler.Instance.TryAdd(Identifikator.Text, Naziv.Text, null, Opis.Text, Helper.ParseStringToInt(velicinaGrupeUpDown.Text),
                Helper.ParseStringToInt(duzinaTerminaUpDown.Text), Helper.ParseStringToInt(brojTerminaUpDown.Text),
                Helper.CheckBoxToBool(ProjectorNeeded), Helper.CheckBoxToBool(TableNeeded), Helper.CheckBoxToBool(SmartTableNeeded), Helper.GetOSFromString(OSComboBox.Text));

            MessageBox.Show("Uspesno dodat predmet!");
        }
    }
}
