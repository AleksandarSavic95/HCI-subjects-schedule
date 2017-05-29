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

namespace SubjectsSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Enable_komande

        private bool _enable;

        public bool MenuEnabled
        {
            get
            {
                return _enable;
            }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    OnPropertyChanged("MenuEnabled");
                }
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            //newClassroom = new FormaClassroom.FormaClassroom();

            // Serialize();
            // Deserialize();

            this.DataContext = this;
        }

        private void Serialize()
        {
            Console.WriteLine("Serializing data...");

            // Testing
            // FieldOfStudyHanlder.Instance.Add("f1", "name1", new DateTime(), "This is dummy description 1");
            // ClassroomHandler.Instance.Add("c1", "This is dummy description 2", 20, true, true, true, OS.CROSS_PLATFORM);
            // SoftwareHandler.Instance.Add("s1", "name2", OS.LINUX, "producer", "http://www.org.com", "2012", 300.00, "This is dummy description 3");
            // SubjectHandler.Instance.Add("s2", "name3", "f1", "This is dummy description 4", 21, 60, 3, true, false, false, OS.LINUX);
            
            // TODO: maybe pull file paths to some global config file?
            FieldOfStudyHanlder.Instance.Serialize("study-fields.bin");
            ClassroomHandler.Instance.Serialize("classrooms.bin");
            SoftwareHandler.Instance.Serialize("softwares.bin");
            SubjectHandler.Instance.Serialize("subjects.bin");

            Console.WriteLine("Serialization finished");
        }

        private void Deserialize()
        {
            Console.WriteLine("Deserializing data...");

            FieldOfStudyHanlder.Instance.Deserialize("study-fields.bin");
            ClassroomHandler.Instance.Deserialize("classrooms.bin");
            SoftwareHandler.Instance.Deserialize("softwares.bin");
            SubjectHandler.Instance.Deserialize("subjects.bin");

            // Testing
            // Console.WriteLine(FieldOfStudyHanlder.Instance.FieldsOfStudy[0]);
            // Console.WriteLine(ClassroomHandler.Instance.Classrooms[0]);
            // Console.WriteLine(SoftwareHandler.Instance.Softwares[0]);
            // Console.WriteLine(SubjectHandler.Instance.Subjects[0]);

            Console.WriteLine("Deserialization finished");
        }

        private void HelloWorld_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HelloWorld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
            /** inicijalizacija softvera */
            Software soft1 = new Software("1", "s1", OS.WINDOWS, "pro1", "a.com", "1997", 51, "o1");
            Software soft2 = new Software("2", "s2", OS.WINDOWS, "pro2", "b.com", "1998", 52, "o2");
            Software soft3 = new Software("3", "s3", OS.WINDOWS, "pro3", "c.com", "1999", 53, "o3");

            /** inicijalizacija ucionice id|opis|mijesta|proj|tabla|pametna| OS */
            Classroom c1 = new Classroom("1", "o1", 20, false, true, false, OS.C_BOTH);
            c1.InstalledSoftware = new List<string>() { "1", "2", "3" };

            /** inicijalizacija predmeta */
            Subject subj1 = new Subject("1", "prd1", "1", "subjO1", 20, 1, 2, false, false, false, OS.SUBJ_WHATEVER);
            subj1.NeedsSoftware = new List<string>() { "1", "2" };
            Subject subj2 = new Subject("2", "prd2", "2", "subjO2", 16, 1, 3, false, false, false, OS.SUBJ_WHATEVER);
            subj2.NeedsSoftware = new List<string>() { "2", "4" };
            Subject subj3 = new Subject("3", "prd3", "3", "subjO3", 22, 2, 2, false, false, false, OS.SUBJ_WHATEVER);
            subj3.NeedsSoftware = new List<string>() { "1", "2" };

            List<Subject> subjects = new List<Subject>() { subj1, subj2, subj3 };

            List<Subject> retVal = new List<Subject>();
            List<string> tuFajnd = c1.InstalledSoftware;
            HashSet<string> hesSet = new HashSet<string>(tuFajnd);
            bool sadrzi = subj1.NeedsSoftware.All(i => hesSet.Contains(i));

            foreach (Subject s in subjects)
            {
                //if (s.NeedsSoftware.Except(toFind).Any())
                if (s.NeedsSoftware.All(i => hesSet.Contains(i)))
                {
                    Console.WriteLine("sadrzi predmet: " + s.Name);
                    retVal.Add(s);
                }
            }

        }

        private void Enable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Enable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // pogledati MenuItem za komandu "RoutedCommands.Enable"
        }

        private void Komanda_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Komanda!");
        }

        private void Komanda_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MenuEnabled;
        }

        private void Ugradjene_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var w = new MyCommands.UgradjeneKomande();
            w.ShowDialog();
        }

        private void Ugradjene_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Menu item click actions

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var w = new Grafika.CustomRender();
            //w.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //var w = new Table.TableExample();
            //w.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //var w = new Grafika3D.Kocka();
            //w.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            // prije bilo reakcija za Stilovi -> Stilovi i triggeri
            //var w = new Stil.StilPrimer();
            //w.ShowDialog();

            // stara forma za unos studenta - radi Help
            var w = new StudentHelpOld.StudentHelpOld();
            w.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //var w = new DDrop.DragDropWindow();
            //w.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //var w = new Kontrole.ToolbarTreeContext();
            //w.ShowDialog();
        }
        #endregion

        /// <summary>
        /// Help command event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            // da radi za sve kontrole
            // DependencyObject focusedControl = FocusManager.GetFocusScope(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                Console.WriteLine("focusedControl je DependencyObject");
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        /// <summary>
        /// Dodatak za help - sprega između C# i JavaScripta (?)
        /// </summary>
        /// <param name="param"></param>
        public void doThings(string param)
        {
            //btnOK.Background = new SolidColorBrush(Color.FromRgb(32, 64, 128));
            Title = param;
        }

        private void New_Classroom_Click(object sender, RoutedEventArgs e) { }

        private void New_Subject_Click(object sender, RoutedEventArgs e) { }

        private void New_FoS_Click(object sender, RoutedEventArgs e) { }

        private void New_Software_Click(object sender, RoutedEventArgs e) { }

        private void NovaUcionica_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Sakriva sve (četiri) forme za dodavanje entiteta.
        /// </summary>
        private void HideAllForms()
        {
            ClassroomForma.Visibility = Visibility.Collapsed;
            SubjectForma.Visibility = Visibility.Collapsed;
            SoftverForma.Visibility = Visibility.Collapsed;
            FieldOfStudyForma.Visibility = Visibility.Collapsed;
        }

        private void NovaUcionica_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            ClassroomForma.Visibility = Visibility.Visible;
        }

        private void NoviPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            SubjectForma.Visibility = Visibility.Visible;
        }

        private void NoviSmjer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviSmjer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            FieldOfStudyForma.Visibility = Visibility.Visible;
        }

        private void NoviSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            SoftverForma.Visibility = Visibility.Visible;
        }

        private void PregledSheme_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PregledSheme_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            ShemaRasporeda.Visibility = Visibility.Visible;
        }
    }
}
