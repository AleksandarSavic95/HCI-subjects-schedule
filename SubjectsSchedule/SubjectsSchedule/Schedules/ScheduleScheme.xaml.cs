using MindFusion.Scheduling;
using SubjectsSchedule.Model;
using SubjectsSchedule.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace SubjectsSchedule.Schedules
{
    /// <summary>
    /// Interaction logic for ScheduleScheme.xaml
    /// </summary>
    public partial class ScheduleScheme : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Subject> PredmetiZaUcionicu
        {
            get; set;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private Classroom _selectedClassroom;

        public Classroom SelectedClassroom
        {
            get { return _selectedClassroom; }
            set
            {
                if (value != _selectedClassroom)
                {
                    _selectedClassroom = value;
                    OnPropertyChanged("SelectedClassroom");
                }
            }
        }

        private bool mouseDown;

        public ScheduleScheme()
        {
            PredmetiZaUcionicu = new ObservableCollection<Subject>();
            InitializeComponent(); // Bitan redoslijed prve dvije linije?

            DataContext = this; // da bi binding radio// Može i u XML-u

            // Da na kalendaru pravimo termine tipa MyTermin, a ne Appointment
            kalendar.InteractiveItemType = typeof(MyTermin);
            
            kalendar.ItemCreated += (s, e) =>
            {
                if (e.Item is MyTermin) // ovo prije pisalo: kalendar.ResetDrag();
                    Console.WriteLine("ItemCreated Event: Moj termin!");
                else Console.WriteLine("ItemCreated Event nesto =/= MyTermin");
            };

            #region Pomijeranje postejećeg termina - na druge termine i van vremena [7-22h]
            kalendar.ItemModified += (s, e) =>
            {
                if (e.Item is MyTermin)
                {
                    Console.WriteLine("modifikovan termin za " + ((MyTermin)e.Item).ForSubject.Name);
                    // TODO: dodati promjenu stringa! e.Item.DescriptionText = ... ima gotovo ...
                    DateTime start = e.Item.StartTime;
                    DateTime end = e.Item.EndTime;
                    string TooEarlyOrTooLate = "Termin je previše ";

                    if (start.TimeOfDay < TimeSpan.FromHours(7))
                    {
                        // kraj = 7:00 + razlika kraja i početka ~~~ očuvanje trajanja termina
                        end = start.Date.AddHours(7).Add(end.Subtract(start));
                        start = start.Date.AddHours(7); // počinje u sedam
                        TooEarlyOrTooLate += "rano. Biće pomjeren tako da počinje u 07:00.\n";
                    }
                    else if (end.TimeOfDay > TimeSpan.FromHours(22))
                    {
                        // početak = 22:00 - razlika kraja i početka ~~~ očuvanje trajanja termina
                        start = start.Date.AddHours(22).Subtract(end.Subtract(start));
                        end = start.Date.AddHours(22);
                        TooEarlyOrTooLate += "kasno. Biće pomjeren tako da završava u 22:00.\n";
                    }

                    // Uzmi sve Item-e osim tek spuštenog
                    var items = kalendar.Schedule.GetAllItems(start, end.AddSeconds(-1)).Except(new List<Item>() { e.Item });
                    // izbacivanje https://stackoverflow.com/a/407741/2101117 <- prvi komentar: Any efikasnije od Count
                    bool zauzeto = items.Any();

                    if (zauzeto)
                    {
                        // Da li želite da obrišete postojeće termine? Argument je tekst "termin je prerano/prekasno".
                        if (Questions.BrisanjeTermina(TooEarlyOrTooLate.Length > 22 ? TooEarlyOrTooLate : "") == MessageBoxResult.Yes)
                        {
                            items.ToList().ForEach(x => RemoveFromCalendar(x));
                            zauzeto = false;
                        }
                    }
                    // ako je slobodno, samo poruka o izlasku iz opsega [7-22]
                    else
                        if (TooEarlyOrTooLate.Length > 22) // ima prekoračenja
                            MessageBox.Show(TooEarlyOrTooLate);

                    if (!zauzeto)
                    {
                        e.Item.StartTime = start; //
                        e.Item.EndTime = end;     //
                    }
                    else
                    {
                        e.Item.StartTime = e.OldStartTime;
                        e.Item.EndTime = e.OldEndTime;
                    }
                }

            };
            #endregion

            // tokom izmjene - mogućnost prekida događaja
            kalendar.ItemModifying += (s, e) =>
            {
                // e.Confirm = false;
            };

            // Serialization support
            Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);
        }

        private void DataGrid_DblClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("\nDvoklik na element u DataGrid-u!");
        }

        private void kalendar_ItemClick(object sender, MindFusion.Scheduling.Wpf.ItemMouseEventArgs e)
        {
            if (e.Item is MyTermin)
            {
                Console.WriteLine("Moj termin: " +
                    (e.Item as MyTermin).StartTime.ToShortTimeString() +
                    " - " + (e.Item as MyTermin).EndTime.ToShortTimeString());
            }
        }

        private void ScheduleSchemeLoaded(object sender, RoutedEventArgs e)
        {
            kalendar.BeginInit();
            kalendar.TimetableSettings.Dates.Clear();

            /// Popunjavanje kalendara danima u sedmici <see cref="ScheduleDays"/>
            for (int i = 0; i < 6; i++)
                kalendar.TimetableSettings.Dates.Add(ScheduleDays.workDays[i]);

            kalendar.EndInit();
        }

        /// <summary>
        /// Prikaz rasporeda za datu učionicu.
        /// </summary>
        /// <param name="classroom">učionica čiji se raspored prikazuje</param>
        public void InitializeSubjectList(Classroom classroom = null)
        {
            Console.WriteLine("InitializeSubjectsList for: " + classroom.Id);

            // TODO: popunjavanje liste predmeta za ucionicu [iz "baze"]
            Console.WriteLine("Ucitavanje podataka za odabranu ucionicu..." +
                "samo ako je odabrana i prozor prikazan! Prebaciti negdje drugo");

            /** Prikaz stranice za raspored učionice */
            PredmetiZaUcionicu.Clear();
            this.Visibility = Visibility.Visible;


            if (classroom == null)
            {
                Console.WriteLine("Nema proslijedjene ucionice!");
                Console.WriteLine("Nema proslijedjene ucionice!");
                classroom = new Classroom("1", "", 30, true, true, true, OS.C_BOTH);
                classroom.InstalledSoftware.AddRange(new List<string>() { "1", "2", "3", "4" });
            }

            SelectedClassroom = classroom;

            /* ako NE postoje u Handler-u, zakomentarisati ovo *
            PredmetiZaUcionicu.Add(new Subject("1", "subj1", "1", "oSubj1", 20, 1, 2, false, true, true, OS.WINDOWS));
            PredmetiZaUcionicu.Add(new Subject("2", "subj2", "2", "oSubj2", 22, 2, 1, false, true, true, OS.SUBJ_WHATEVER));
            PredmetiZaUcionicu.Add(new Subject("3", "bazePod", "Siit3", "oSubj3", 18, 3, 1, true, true, false, OS.WINDOWS));
            PredmetiZaUcionicu.Add(new Subject("4", "HCI", "siit", "Interakcija covjek-racunar", 16, 2, 2, false, false, false, OS.SUBJ_WHATEVER));
            // */

            /* ako već postoje u Handler-u, zakomentarisati ovo */
            FieldOfStudy fos1 = new FieldOfStudy("fos1", "SIIT", DateTime.Parse("25/05/2014"),"opisFOS1");
            FieldOfStudy fos2 = new FieldOfStudy("fos2", "E3", DateTime.Parse("24/04/2015"), "opisFOS2");

            SubjectHandler.Instance.TryAdd("1", "subj1", fos1, "opis sub1", 20, 1, 2, false, true, true, OS.WINDOWS);
            SubjectHandler.Instance.TryAdd("2", "ISA", fos2, "oSubj2", 22, 2, 1, false, true, true, OS.SUBJ_WHATEVER);
            SubjectHandler.Instance.TryAdd("3", "bazePod", fos1, "oSubj3", 18, 3, 1, true, true, false, OS.WINDOWS);
            SubjectHandler.Instance.TryAdd("4", "HCI", fos2, "Interakcija covjek-racunar", 16, 2, 2, false, false, false, OS.SUBJ_WHATEVER);
            // */

            List<Subject> tmp = SubjectHandler.Instance.FindByClassroom(classroom); // RADI!
            if (tmp.Count > 0) tmp.ForEach(subj => AddToSubjectListAndUpdateRow(subj));
        }

        private void AddToSubjectListAndUpdateRow(Subject subj)
        {
            PredmetiZaUcionicu.Add(subj);
            // dodatni kod za ažuriranje reda
        }

        private void kalendar_ItemDeleted(object sender, ItemEventArgs e)
        {
            MyTermin deletedTermin = (MyTermin)e.Item;
            Subject deletedTerminSubject = deletedTermin.ForSubject;

            SubjectHandler.Instance.ChangeUnscheduledTermins(deletedTerminSubject.Id, false);

            UpdateSubjectRow(deletedTerminSubject);

            SubjectsList.UnselectAll();
        }

        #region Drag & drop ListaPredmeta --> kalendar

        private void subjList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = e.LeftButton == MouseButtonState.Pressed;
        }

        private void subjList_MouseMove(object sender, MouseEventArgs e)
        {
            if (SubjectsList.SelectedItem != null && mouseDown)
            {
                mouseDown = false;
                // Za d&d bitan je tip DATA parametra
                Subject data = (Subject)SubjectsList.SelectedItem;

                // TODO: bolje hendlanje zabrane slanja
                if (data.UnscheduledTermins < 1)
                    return;

                // Obtaining DoDragDrop
                // https://stackoverflow.com/a/1720709/2101117
                //var dataObj = new DataObject(data);  // Ako koristiš ovo
                //dataObj.SetData("DragSource", this); // PROMIJENI IME PARAMETRA "data" u "dataObj"

                //string data = ((ListBoxItem)SubjectsList.SelectedItem).Content.ToString();

                Console.WriteLine("mouseMOve: " + data);
                DragDrop.DoDragDrop(SubjectsList, data, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Gives visual feedback to the user when the mouse moves
        /// over the kalendar control during drag & drop operation.
        /// Checks if the dragged data matches the expected type and
        /// if the location under the mouse cursor represents a valid date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kalendar_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            
            if (e.Data.GetDataPresent(typeof(Subject))) {
                Console.WriteLine("Subject! DragOver");
                Subject subject = (Subject)e.Data.GetData(typeof(Subject));

                DateTime? date = kalendar.GetDateAt(e.GetPosition(kalendar));
                Console.WriteLine("\nsupsteno SUBJECT na datum: " + date);

                if (date != null)
                    e.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Reakcija na puštanje predmeta na kalendar - novi termin!
        /// </summary>
        private void kalendar_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Subject)))
                AddTerminFromDragAndDrop(e);
        }

        /// <summary>
        /// Pravljenje termina na Drag and DROP.
        /// </summary>
        /// <param name="e"></param>
        private void AddTerminFromDragAndDrop(DragEventArgs e)
        {
            Point point = e.GetPosition(kalendar);
            DateTime? date = kalendar.GetDateAt(point);

            Subject subject = (Subject) e.Data.GetData(typeof(Subject));

            // TODO: Srediti da se može dodati termin "tik uz drugi" - pogledaj kod ispod ove linije
            // trenutno preko štapa i kanapa [ ili nije? :) ]
            DateTime start = date.Value;
            DateTime end = start.AddMinutes(subject.ClassLength * 45);

            var allItems = kalendar.Schedule.GetAllItems(start, end.AddSeconds(-1));
            bool zauzeto = allItems.Any();

            if (date != null)
            {
                if (zauzeto) // postoji 1 ili više termina
                {
                    if (Questions.BrisanjeTermina() == MessageBoxResult.Yes)
                    {
                        // čišćenje polja sa taskovima
                        allItems.ToList().ForEach(x => RemoveFromCalendar(x));
                        zauzeto = false; // fleg za dodavanje novog
                    }
                }
                if (!zauzeto) // ne može else zbog flag-a iznad
                {
                    Console.WriteLine(start.TimeOfDay.ToString());
                    Console.WriteLine(start.ToString("HH:mm") + " - nezavršeno" + end.ToString("HH:mm"));
                    MyTermin termin = new MyTermin(subject.Name, start, end);
                    termin.ForSubject = subject;

                    // broj nerapoređenih termina se smanjuje
                    SubjectHandler.Instance.ChangeUnscheduledTermins(subject.Id);

                    // TODO: poboljšati (?) označavanje potpuno raspoređenog predmeta
                    UpdateSubjectRow(subject);

                    kalendar.Schedule.Items.Add(termin);
                }
            }
        }

        /// <summary>
        /// Uklanjanje termina iz ovog rasporeda
        /// </summary>
        /// <param name="item"></param>
        private void RemoveFromCalendar(Item item)
        {
            Subject itemsSubject = ((MyTermin)item).ForSubject;
            // broj nerapoređenih termina se povećava
            SubjectHandler.Instance.ChangeUnscheduledTermins(itemsSubject.Id, false);

            // TODO: poboljšati (?) označavanje NEraspoređenog predmeta ** kada postane NErasp. drugačije od već NErasp.?? **
            UpdateSubjectRow(itemsSubject);

            kalendar.Schedule.Items.Remove(item);
        }

        #endregion

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("ScheduleScheme IsVisibleChanged + " + ((bool)e.NewValue));
        }

        /// <summary>
        /// Asinhroni(?) poziv za bojenje svih redova u listi predmeta.
        /// <see cref="ColorMyRows"/>
        /// ASYNC EVENT: https://stackoverflow.com/questions/24087058/do-something-after-datagrid-finished-loading-with-async-itemssource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubjectsList_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => ColorMyRows()));
        }

        private void SubjectsList_LayoutUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => ColorMyRows()));
        }

        /// <summary>
        /// Boji redove u listi predmeta u zavisnosti
        /// od broja NEraspoređenih termina.
        /// </summary>
        private void ColorMyRows()
        {
            DataGridRow row = null;
            Subject currentSubject;
            for (int i = 0; i < SubjectsList.Items.Count; i++)
            {
                row = SubjectsList.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                currentSubject = (Subject)row.Item;
                if (currentSubject.UnscheduledTermins > 0)
                {
                    row.Background = Brushes.PaleGoldenrod; // black'n gold baby :P
                    row.ToolTip = "Predmet " + currentSubject.Name + " ima neraspoređene termine.";
                }
                else
                {
                    row.Background = Brushes.PaleGreen;
                    row.ToolTip = "Predmet " + currentSubject.Name + " nema neraspoređenih termina.";
                }
            }
        }

        private void UpdateSubjectRow(Subject subject)
        {
            if (subject.UnscheduledTermins < 1)
            {
                DataGridRow row = SubjectsList.ItemContainerGenerator.ContainerFromItem(subject) as DataGridRow;
                row.Background = Brushes.PaleGreen;
                row.ToolTip = "Predmet " + subject.Name + " je raspoređen.";
                //row.IsEnabled = false;
                SubjectsList.UnselectAll();
            }
            else
            {
                DataGridRow row = SubjectsList.ItemContainerGenerator.ContainerFromItem(subject) as DataGridRow;
                row.Background = Brushes.PaleGoldenrod; // black'n gold baby :P
                row.ToolTip = "Predmet " + subject.Name + " ima neraspoređene termine.";
                //row.IsEnabled = true;
                SubjectsList.UnselectAll();
            }
        }
    }
}
