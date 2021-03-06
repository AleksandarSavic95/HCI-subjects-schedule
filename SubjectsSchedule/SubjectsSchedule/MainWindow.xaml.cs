using MindFusion.Scheduling;
using SubjectsSchedule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SubjectsSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Enable_komande

        private bool _enable;

        public static Thread demoModeThread = null;
        public DemoMode.DemoModeWindow demoModeWindow;

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

            DataLoading = true;

            try
            {
                Deserialize();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load data...");
            }

            this.DataContext = this;

            Obavjestenja = new Dictionary<string, bool>()
            {
                { "Prekoračenje radnog vremena", true},
                { "Najava prikaza rasporeda učionice", true }
            };

            // Omogućuje postavku naziva za dugmad u messageBox-ovima
            // credits: https://www.codeproject.com/Articles/18399/Localizing-System-MessageBox
            System.Windows.Forms.MessageBoxManager.Register();

            // prevent the window to cover the TaskBar: https://stackoverflow.com/a/35010001/2101117
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        /// <summary>
        /// Mapa u kojoj su kljucevi opisi obavjestenja,
        /// a vrijednosti indikatori da li su ukljucena
        /// (<code>true</code>) ili iskljucena (<code>false</code>).
        /// Nece biti serijalizovana, jer je bolje da sva obavjestenja na pocetku
        /// svakog pokretanja aplikacije budu ukljucena, pa se lako mogu iskljuciti.
        /// </summary>
        public Dictionary<string, bool> Obavjestenja { get; set; }

        private void Serialize()
        {
            Console.WriteLine("Serializing data...");

            // Testing
            // FieldOfStudyHanlder.Instance.Add("f1", "name1", new DateTime(), "This is dummy description 1");
            // ClassroomHandler.Instance.Add("c1", "This is dummy description 2", 20, true, true, true, OS.CROSS_PLATFORM);
            // SoftwareHandler.Instance.Add("s1", "name2", OS.LINUX, "producer", "http://www.org.com", "2012", 300.00, "This is dummy description 3");
            // SubjectHandler.Instance.Add("s2", "name3", "f1", "This is dummy description 4", 21, 60, 3, true, false, false, OS.LINUX);
            
            // TODO: maybe pull file paths to some global config file? - kasnije ;)
            FieldOfStudyHanlder.Instance.Serialize("study-fields.bin");
            ClassroomHandler.Instance.Serialize("classrooms.bin");
            SoftwareHandler.Instance.Serialize("softwares.bin");
            SubjectHandler.Instance.Serialize("subjects.bin");

            // Pogledati dokumentaciju ove metode
            TerminHandler.Instance.Serialize("termins");

            Console.WriteLine("Serialization finished");
        }

        private void Deserialize()
        {
            Console.WriteLine("Deserializing data...");

            FieldOfStudyHanlder.Instance.Deserialize("study-fields.bin");
            ClassroomHandler.Instance.Deserialize("classrooms.bin");
            SoftwareHandler.Instance.Deserialize("softwares.bin");
            SubjectHandler.Instance.Deserialize("subjects.bin");

            TerminHandler.Instance.Deserialize("termins");

            // Testing
            // Console.WriteLine(FieldOfStudyHanlder.Instance.FieldsOfStudy[0]);
            // Console.WriteLine(ClassroomHandler.Instance.Classrooms[0]);
            // Console.WriteLine(SoftwareHandler.Instance.Softwares[0]);
            // Console.WriteLine(SubjectHandler.Instance.Subjects[0]);

            Console.WriteLine("Deserialization finished");
        }

        #region komande menija
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

            FieldOfStudy fos1 = new FieldOfStudy("fos1", "SIIT", DateTime.Parse("25/05/2014"), "opisFOS1");
            /** inicijalizacija predmeta */
            Subject subj1 = new Subject("1", "prd1", fos1, "subjO1", 20, 1, 2, false, false, false, OS.SUBJ_WHATEVER);
            subj1.NeedsSoftware = new List<string>() { "1", "2" };
            Subject subj2 = new Subject("2", "prd2", fos1, "subjO2", 16, 1, 3, false, false, false, OS.SUBJ_WHATEVER);
            subj2.NeedsSoftware = new List<string>() { "2", "4" };
            Subject subj3 = new Subject("3", "prd3", fos1, "subjO3", 22, 2, 2, false, false, false, OS.SUBJ_WHATEVER);
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

        private void AbortDemo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (demoModeThread != null);
        }

        private void AbortDemo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            demoModeWindow.AbortDemo();
        }

        #endregion

        #region PropertyChanged handling
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region Menu item click actions

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var w = new Grafika.CustomRender();
            //w.ShowDialog();
        }

        private void demoMode() // DemoMode.DemoModeWindow demoModeWindow
        {

            #region unos učionice

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = "\nI - Dodavanje učionice\n"
                  + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber +
                ". Kliknemo na dugme Dodaj učionicu u gornjem desnom uglu ekrana\n"
                  + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() => {
                ButtonAutomationPeer peer =
                new ButtonAutomationPeer(dodajUcionicuDugme);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

                peer.SetFocus();
                invokeProv.Invoke();
            });
            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Kliknemo na polje identifikatora učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() => {
                TextBoxAutomationPeer peer = new TextBoxAutomationPeer(ClassroomForma.Identificator);
                peer.SetFocus();
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Unesemo identifikator nove učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() => { ClassroomForma.Identificator.Text = "L-52"; });
            Thread.Sleep(2000);

            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Kliknemo na polje opisa učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() => {
                TextBoxAutomationPeer peer = new TextBoxAutomationPeer(ClassroomForma.Description);
                peer.SetFocus();
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Unesemo opis učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() => { ClassroomForma.Description.Text = "Opis učionice"; });
            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Unesemo broj radnika\n\n" + demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(2500);

            Dispatcher.Invoke(() => { ClassroomForma.brojMijestaUpDown.Value = 3; });
            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Odaberemo operativni sistem\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() => {
                ComboBoxAutomationPeer peer = new ComboBoxAutomationPeer(ClassroomForma.OperatingSystem);
                peer.SetFocus();
                IExpandCollapseProvider provider = (IExpandCollapseProvider)
                    peer.GetPattern(PatternInterface.ExpandCollapse);
                provider.Expand();
            });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() => { ClassroomForma.OperatingSystem.SelectedIndex = 2; });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() => {
                ComboBoxAutomationPeer peer = new ComboBoxAutomationPeer(ClassroomForma.OperatingSystem);
                peer.SetFocus();
                IExpandCollapseProvider provider = (IExpandCollapseProvider)
                    peer.GetPattern(PatternInterface.ExpandCollapse);
                provider.Collapse();
            });

            Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Odaberemo instalirani softver iz liste\n\n" + demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(5500);

            Dispatcher.Invoke(() => {
                ListBoxAutomationPeer peer = new ListBoxAutomationPeer(ClassroomForma.SoftwaresList);
                peer.SetFocus();
                ClassroomForma.SoftwaresList.SelectedIndex = 2;
            });
            Thread.Sleep(2000);
            Dispatcher.Invoke(() => { ClassroomForma.SoftwaresList.SelectedIndex = 4; });

            Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Označimo opremu koja postoji. Tablu nemamo, ali imamo projektor\n\n" + demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(5500);

            Dispatcher.Invoke(() => {
                ClassroomForma.TableNeeded.IsChecked = false;
                ClassroomForma.ProjectorNeeded.IsChecked = true;
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Potvrdimo unos\n\n" + demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer =
                new ButtonAutomationPeer(ClassroomForma.Potvrda);
                peer.SetFocus();
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(ClassroomForma.Potvrda);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();

                demoModeWindow.demoNumber = 0;
            });

            Thread.Sleep(1000);
            Console.WriteLine("444444444444");

            #endregion unos učionice

            #region pregled učionica
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = "\nII Pregled učionica\n\n" +
                    demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber +
                ". Odaberemo meni Tabele\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);


            Dispatcher.Invoke(() =>
            {
                MenuItemAutomationPeer peer =
                new MenuItemAutomationPeer(this.MeniTabele);

                IExpandCollapseProvider provider = (IExpandCollapseProvider)
                    peer.GetPattern(PatternInterface.ExpandCollapse);
                provider.Expand();
            });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber + ". Odaberemo stavku Učionice\n\n"
                  + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                MenuItemAutomationPeer peer = new MenuItemAutomationPeer(this.MeniItemUcionice);
                peer.SetFocus();
            });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                MenuItemAutomationPeer peer = new MenuItemAutomationPeer(this.MeniItemUcionice);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

                peer.SetFocus();
                invokeProv.Invoke();
            });

            Thread.Sleep(2000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber + ". Sada vidimo tabelu sa učionicama" +
                   " gdje možemo svaku od njih obrisati ili izmijeniti\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            #endregion

            #region izmjena učionice
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = "\nIII Izmjena učionice\n\n" +
                    demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(4000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber +
                ". Izmijenićemo jednu učionicu, klikom na dugme Izmijeni u prvom redu\n\n" +
                demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(4000);
            Dispatcher.Invoke(() =>
            {
                this.ClassroomTabela.ClassroomsList.SelectedItem = 1;
                ClassroomHandler.Instance.SetSelectedClassroom(ClassroomTabela.Classrooms[0], (MainWindow)Window.GetWindow(this));
                ((MainWindow)Window.GetWindow(this)).NovaUcionica_Show();
            });
            Thread.Sleep(4000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Kliknemo na polje opisa učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() => {
                TextBoxAutomationPeer peer = new TextBoxAutomationPeer(ClassroomForma.Description);
                peer.SetFocus();
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Unesemo novi opis učionice\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() => { ClassroomForma.Description.Text = "Novi opis za L-42"; });

            Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Izmijenimo postojeću opremu. Kupili smo pametnu tablu.\n\n" + demoModeWindow.currentDemoDescription.Text;
            });
            Thread.Sleep(5000);

            Dispatcher.Invoke(() => {
                ClassroomForma.SmartTableNeeded.IsChecked = true;
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber
                  + ". Potvrdimo unos.\n\n" + demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer =
                new ButtonAutomationPeer(ClassroomForma.Potvrda);
                peer.SetFocus();
            });

            Thread.Sleep(2000);
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(ClassroomForma.Potvrda);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber +
                  ". Nakon unosa ponovo vidimo prikaz svih učionica, te možemo vidjeti " +
                  "da je njen identifikator izmijenjen.\n\n" + demoModeWindow.currentDemoDescription.Text;

                demoModeWindow.demoNumber = 0;
            });

            Thread.Sleep(5000);
            #endregion

            #region brisanje učionice
            Dispatcher.Invoke(() =>
            {
                demoModeWindow.demoNumber = 0;
                demoModeWindow.currentDemoDescription.Text = "\nIV Brisanje učionice\n\n" +
                    demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(3000);

            Dispatcher.Invoke(() =>
            {
                demoModeWindow.currentDemoDescription.Text = demoModeWindow.demoNumber +
                ". Obrisaćemo jednu učionicu, recimo prvu, klikom na dugme Obriši u prvom redu\n\n" +
                demoModeWindow.currentDemoDescription.Text;
            });

            Thread.Sleep(4000);
            Dispatcher.Invoke(() =>
            {
                this.ClassroomTabela.ClassroomsList.SelectedItem = 1;
                ClassroomHandler.Instance.SetSelectedClassroom(ClassroomTabela.Classrooms[0], (MainWindow)Window.GetWindow(this));
                ClassroomHandler.Instance.Remove(ClassroomHandler.Instance.SelectedClassroom.Id, (MainWindow)Window.GetWindow(this));
            });
            Thread.Sleep(4000);
            #endregion

        }

        private void Demonstracioni_mod_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Sada će aplikacija početi da Vam prikazuje funkcionalnosti.\n" +
            //    "\n\r U donjem desnom uglu Vašeg ekrana će biti prikazan prozor na kome \n" +
            //    "će Vam se, po redu izvršavanja, prikazivati akcije koje budu izvršavane. \n\n" +
            //    "Prekid prikaza funkcionalnosti možete izvršiti na dva načina: \n\n" + 
            //    "  1. Pomoću tastature\n" +
            //    "    Kombinacijom dugmadi CTRL i slova D\n" +
            //    "  \n  2. Pomoću miša\n" +
            //    "    Klikom na dugme < Prekid demonstracije > na dnu novootvorenog \n" +
            //    "    prozora u desnom uglu ekrana.",
            //    "Početak demonstracije");

            demoModeWindow = new DemoMode.DemoModeWindow();
            demoModeWindow.Show();

            //demoModeThread = new Thread( () => demoMode(demoModeWindow) );
            demoModeThread = new Thread(new ThreadStart(demoMode));
            demoModeThread.SetApartmentState(ApartmentState.STA);
            demoModeThread.Start();
        }

        private void O_Aplikaciiji_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacija je pravljena za predmetni projekat" +
                " iz predmeta HCI (interakcija čovjek-računar).\nRezultat je"+
                " rada Tima 13, čiji su članovi:\n"+
                "\tAleksandar Savić - SW51/2014,\n" +
                "\tFilip Savić - SW30/2014,\n" +
                "\tAcko Spasić - SW24/2014.",
                "O aplikaciji", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        #endregion

        #region help support
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
        #endregion

        #region komande i dugmad za forme
        private void New_Classroom_Click(object sender, RoutedEventArgs e) { }

        private void New_Subject_Click(object sender, RoutedEventArgs e) { }

        private void New_FoS_Click(object sender, RoutedEventArgs e) { }

        private void New_Software_Click(object sender, RoutedEventArgs e) { }

        private void NovaUcionica_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Sakriva globalnu i pojedinacnu shemu rasporeda,
        /// kao i forme za dodavanje entiteta (predmet, softver...).
        /// </summary>
        private void HideAllForms()
        {
            ClassroomForma.Visibility = Visibility.Collapsed;
            SubjectForma.Visibility = Visibility.Collapsed;
            SoftverForma.Visibility = Visibility.Collapsed;
            FieldOfStudyForma.Visibility = Visibility.Collapsed;

            ClassroomTabela.Visibility = Visibility.Collapsed;
            SubjectTabela.Visibility = Visibility.Collapsed;
            SoftverTabela.Visibility = Visibility.Collapsed;
            FieldOfStudyTabela.Visibility = Visibility.Collapsed;

            ClassroomPrikaz.Visibility = Visibility.Collapsed;
            SubjectPrikaz.Visibility = Visibility.Collapsed;
            SoftverPrikaz.Visibility = Visibility.Collapsed;
            FieldOfStudyPrikaz.Visibility = Visibility.Collapsed;

            RasporedUcionice.Visibility = Visibility.Collapsed;
            GlobalnaShema.Visibility = Visibility.Collapsed;
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
            Console.WriteLine("Ne treba ovo ili treba mijenjati!");
            GlobalnaShema.Visibility = Visibility.Visible;
        }
        #endregion

        /// <summary>
        /// Ucitavanje DockPanel-a kao glavnog kontejnera je znak da su sve
        /// unutrasnje komponente spremne da prime podatke.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanelLoaded(object sender, RoutedEventArgs e)
        {
            DataLoading = true;
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                InitializeClassroomsList()
            ));
            
            GlobalnaShema.PopulateResources();

            RasporedUcionice.MainWindowParent = this;

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                GlobalnaShema.LoadTerminsToGlobal()
            ));

            /// <see cref="SubjectHandler.Instance.ResetAllUncheduledTermins"/>.
            if (TerminHandler.Instance.TerminsByIds.Count == 0)
                SubjectHandler.Instance.ResetAllUncheduledTermins();
        }

        private void InitializeClassroomsList()
        {
            foreach (var classroom in ClassroomHandler.Instance.Classrooms)
                AddClassroomButton(classroom);
            DataLoading = false;
        }

        /// <summary>
        /// Dodavanje dugmeta ciji naziv odgovara nazivu ucionice, a Tag objekat samoj instanci.
        /// </summary>
        /// <param name="classroom"></param>
        public void AddClassroomButton(Classroom classroom)
        {
            Button classroomButton = new Button();
            classroomButton.ToolTip = classroom.Description;
            classroomButton.Content = classroom.Id;
            classroomButton.Click += ClassroomButton_Click;
            classroomButton.Margin = new Thickness(0, 1, 0, 1);

            // Kačimo objekat za dugme, da ga ne tražimo poslije u bazi
            classroomButton.Tag = classroom;

            ClassroomButtonList.Children.Add(classroomButton);
        }

        public void RemoveClassroomButton(Classroom classroom)
        {
            Button cButton;
            for (int i = 0; i < ClassroomButtonList.Children.Count; i++)
            {
                cButton = (Button)ClassroomButtonList.Children[i];
                if (((Classroom)cButton.Tag).Id == classroom.Id)
                {
                    ClassroomButtonList.Children.RemoveAt(i);
                    break;
                }
            }
        }

        private void ClassroomButton_Click(object sender, RoutedEventArgs e)
        {
            Classroom c = (Classroom)((Button)e.Source).Tag;
            PrikazRasporedaUcionice(c);
        }

        public void PrikazRasporedaUcionice(Classroom c)
        {
            if (RasporedUcionice.SelectedScheduleClassroom == c)
                return;

            Console.WriteLine("DATAloading = TRUE - PrikazRasporedaUcionice");
            DataLoading = true; // false-ovaće ga RasporedUcionice... ljepota jedna xD

            this.HideAllForms();
            RasporedUcionice.SelectedScheduleClassroom = c;
            GlobalnaShema.Visibility = Visibility.Collapsed;
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                RasporedUcionice.InitializeSubjectList(c)));

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                RasporedUcionice.LoadTermins()));
        }

        #region Prikaz poruke o učitavanju podataka
        private bool _isLoading;
        public bool DataLoading
        {
            get
            { return _isLoading; }

            set
            {
                _isLoading = value;
                Console.WriteLine("DataLoading se promijenilo!");
                OnPropertyChanged("DataLoading");
            }
        }

        private double popupLeft;
        public double PopupLeft
        {
            get
            {
                //popupLeft = (this.ActualWidth / 2 - 50);
                popupLeft = 100;
                return popupLeft;
            }

            set
            {
                popupLeft = value;
                OnPropertyChanged("PopupLeft");
            }
        }
        #endregion

        #region Show-ovi za forme i prikaze
        private void Predmeti_Click(object sender, RoutedEventArgs e)
        {
            Predmeti_Show();
        }

        private void Smerovi_Click(object sender, RoutedEventArgs e)
        {
            Smerovi_Show();
        }

        private void Ucionice_Click(object sender, RoutedEventArgs e)
        {
            Ucionice_Show();
        }

        private void Softveri_Click(object sender, RoutedEventArgs e)
        {
            Softveri_Show();
        }

        public void NotifyAll(string name)
        {
            Console.WriteLine("Notifying all with {0}", name);
            ClassroomTabela.OnPropertyChanged(name);
            FieldOfStudyTabela.OnPropertyChanged(name);
            SubjectTabela.OnPropertyChanged(name);
            SoftverTabela.OnPropertyChanged(name);

            ClassroomPrikaz.OnPropertyChanged(name);
            FieldOfStudyPrikaz.OnPropertyChanged(name);
            SubjectPrikaz.OnPropertyChanged(name);
            SoftverPrikaz.OnPropertyChanged(name);

            ClassroomForma.OnPropertyChanged(name);
            FieldOfStudyForma.OnPropertyChanged(name);
            SubjectForma.OnPropertyChanged(name);
            SoftverForma.OnPropertyChanged(name);
        }

        public void Predmeti_Show()
        {
            HideAllForms();
            SubjectTabela.Visibility = Visibility.Visible;
        }

        public void Smerovi_Show()
        {
            HideAllForms();
            FieldOfStudyTabela.Visibility = Visibility.Visible;
        }

        public void Ucionice_Show()
        {
            HideAllForms();
            ClassroomTabela.Visibility = Visibility.Visible;
        }

        public void Softveri_Show()
        {
            HideAllForms();
            SoftverTabela.Visibility = Visibility.Visible;
        }

        public void NovaUcionica_Show()
        {
            HideAllForms();
            ClassroomForma.Visibility = Visibility.Visible;
        }

        public void Ucionica_Show()
        {
            HideAllForms();
            ClassroomPrikaz.Visibility = Visibility.Visible;
        }

        public void NoviSmer_Show()
        {
            HideAllForms();
            FieldOfStudyForma.Visibility = Visibility.Visible;
        }

        public void Smer_Show()
        {
            HideAllForms();
            FieldOfStudyPrikaz.Visibility = Visibility.Visible;
        }

        public void NoviSoftver_Show()
        {
            HideAllForms();
            SoftverForma.Visibility = Visibility.Visible;
        }

        public void Softver_Show()
        {
            HideAllForms();
            SoftverPrikaz.Visibility = Visibility.Visible;
        }

        public void NoviPredmet_Show()
        {
            HideAllForms();
            SubjectForma.Visibility = Visibility.Visible;
        }

        public void Predmet_Show()
        {
            HideAllForms();
            SubjectPrikaz.Visibility = Visibility.Visible;
        }

        #endregion


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // TODO maybe move this to separate thread?
            Serialize();
        }

        private void Podesavanja_ItemCheck(object sender, RoutedEventArgs e)
        {
            MenuItem source = (sender as MenuItem);
            Console.WriteLine("promjena za {0}. Sad je {1}", source.Header.ToString(), Obavjestenja[source.Header as string]);
            Obavjestenja[source.Header as string] = source.IsChecked;
            Console.WriteLine("promjena za {0}. Sad je {1}", source.Header.ToString(), Obavjestenja[source.Header as string]);
        }
    }
}
