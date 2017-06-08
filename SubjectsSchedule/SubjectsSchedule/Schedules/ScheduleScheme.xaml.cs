using MindFusion.Scheduling;
using SubjectsSchedule.Model;
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


        private bool mouseDown;

        public ScheduleScheme()
        {
            PredmetiZaUcionicu = new ObservableCollection<Subject>();
            InitializeComponent(); // Bitan redoslijed prve dvije linije?

            DataContext = this; // da bi binding radio// Može i u XML-u

            // Da na kalendaru pravimo termine tipa MyTermin, a ne Appointment
            kalendar.InteractiveItemType = typeof(MyTermin);

            kalendar.ItemModifying += (s, e) =>
            {
                DateTime start = e.Item.StartTime;
                DateTime end = e.Item.EndTime;

                if (start.TimeOfDay < TimeSpan.FromHours(7))
                {
                    e.Confirm = false;

                    // kraj = 7:00 + razlika kraja i početka ~~~ očuvanje trajanja termina
                    end = start.Date.AddHours(7).Add(end.Subtract(start));
                    start = start.Date.AddHours(7); // počinje u sedam
                }
                else if (end.TimeOfDay > TimeSpan.FromHours(22))
                {
                    e.Confirm = false;

                    // početak = 22:00 - razlika kraja i početka ~~~ očuvanje trajanja termina
                    start = start.Date.AddHours(22).Subtract(end.Subtract(start));
                    end = start.Date.AddHours(22);
                }

                var items = kalendar.Schedule.GetAllItems(start, end);
                // https://stackoverflow.com/a/407741/2101117 <- prvi komentar: Any efikasnije od Count
                if (items.Except(new List<Item>() { e.Item }).Any())
                {
                    // TODO: pitanje o brisanju postojecih termina
                    Console.WriteLine("TODO: pitanje o brisanju postojecih termina");
                    e.Confirm = false;
                }
            };

            // 
            kalendar.ItemCreated += (s, e) =>
            {
                if (e.Item is MyTermin) // ovo prije pisalo: kalendar.ResetDrag();
                    Console.WriteLine("ItemCreated Event: Moj termin!");
                else Console.WriteLine("ItemCreated Event nesto =/= MyTermin");
            };

            kalendar.ItemModified += (s, e) =>
            {
                if (e.Item is MyTermin)
                {
                    Console.WriteLine("modifikovan item (slijedi opis) " + e.Item.DescriptionText);
                    // TODO: dodati promjenu stringa! e.Item.DescriptionText = ... ima gotovo ...
                }

            };

            // Serialization support
            Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);

            InitializeSubjectList(); // TODO: popunjavanje liste predmeta za ucionicu [iz "baze"]
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

            Console.WriteLine("Ucitavanje podataka za ovu... samo ako je odabrana i prozor prikazan!");
        }

        private void InitializeSubjectList()
        {
            Classroom svemoguca = new Classroom("1", "", 30, true, true, true, OS.C_BOTH);
            svemoguca.InstalledSoftware.AddRange(new List<string>() { "1", "2", "3", "4" });

            PredmetiZaUcionicu.Add(new Subject("1", "subj1", "1", "oSubj1", 20, 1, 2, false, true, true, OS.WINDOWS));
            PredmetiZaUcionicu.Add(new Subject("2", "subj2", "2", "oSubj2", 22, 2, 1, false, true, true, OS.SUBJ_WHATEVER));
            PredmetiZaUcionicu.Add(new Subject("3", "bazePod", "Siit3", "oSubj3", 18, 3, 1, true, true, false, OS.WINDOWS));

            /* ako već postoje u Handler-u, zakomentarisati ovo */
            // SubjectHandler.Instance.Add("3", "bazePod", "Siit3", "oSubj3", 18, 3, 1, true, true, false, OS.WINDOWS); // */

            //List <Subject> tmp = SubjectHandler.Instance.FindByClassroom(svemoguca);
            
            //if (tmp.Count > 0) tmp.ForEach(subj => PredmetiZaUcionicu.Add(subj));

            PredmetiZaUcionicu.Add(new Subject("4", "HCI", "siit", "Interakcija covjek-racunar", 16, 45, 2, false, false, false, OS.SUBJ_WHATEVER));

            //SubjectsList.ItemsSource = PredmetiZaUcionicu; // ako nije podešen DataContext mora sa ovim
        }

        #region Drag & drop  TaskList --> kalendar
        private void taskList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = e.LeftButton == MouseButtonState.Pressed;
        }

        private void taskList_MouseMove(object sender, MouseEventArgs e)
        {
            if (taskList.SelectedItem != null && mouseDown)
            {
                mouseDown = false;
                // Za d&d bitan je tip DATA parametra
                string data = ((ListBoxItem)taskList.SelectedItem).Content.ToString();
                Console.WriteLine("mouseMOve: " + data);
                DragDrop.DoDragDrop(taskList, data, DragDropEffects.Copy);
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
            string gotData = (string)e.Data.GetData(DataFormats.StringFormat);
            if (e.Data.GetDataPresent(typeof(string)))
            {
                Console.WriteLine("\nStigao string(data): " + gotData);

                DateTime? date = kalendar.GetDateAt(e.GetPosition(kalendar));

                Console.WriteLine("\nsupsteno na datum: " + date);

                if (date != null)
                    e.Effects = DragDropEffects.Copy;
            }
            else //ne treba mi ovo prije...
            if (e.Data.GetDataPresent(typeof(Subject))) {
                Console.WriteLine("Subject! DragOver");
                Subject subject = (Subject)e.Data.GetData(typeof(Subject));

                DateTime? date = kalendar.GetDateAt(e.GetPosition(kalendar));
                Console.WriteLine("\nsupsteno SUBJECT na datum: " + date);

                if (date != null)
                    e.Effects = DragDropEffects.Copy;
            }
        }

        private void kalendar_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string))) // TODO: promijeniti tip
            {
                AddTaskFromDragAndDrop(e);
            }
            if (e.Data.GetDataPresent(typeof(Subject)))
            {
                AddTerminFromDragAndDrop(e);
            }
        }

        private void AddTerminFromDragAndDrop(DragEventArgs e)
        {
            Point point = e.GetPosition(kalendar);
            DateTime? date = kalendar.GetDateAt(point);

            Subject subject = (Subject) e.Data.GetData(typeof(Subject));

            // TODO: Srediti da se može dodati termin "tik uz drugi" - pogledaj kod ispod ove linije
            // trenutno preko štapa i kanapa [ ili nije? :) ]
            var allItems = kalendar.Schedule.GetAllItems(date.Value, date.Value.AddMinutes(subject.ClassLength * 45).AddSeconds(-1));
            bool zauzeto = allItems.Any();

            if (date != null)
            {
                MessageBoxResult messageBoxResult = MessageBoxResult.Yes;

                if (zauzeto) // postoji 1 ili više termina
                {
                    /** Prevod teksta na dugmadima <see cref="MainWindow()"/>*/
                    System.Windows.Forms.MessageBoxManager.Yes = "Da";
                    System.Windows.Forms.MessageBoxManager.No = "Ne";

                    messageBoxResult = MessageBox
                        .Show("Da li želite da obrišete termine koji se preklapaju?",
                        "Zauzet termin", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        // čišćenje polja sa taskovima
                        allItems.ToList().ForEach(x => kalendar.Schedule.Items.Remove(x));

                        zauzeto = false; // fleg za dodavanje novog
                    }
                }
                if (!zauzeto)
                {
                    Console.WriteLine(date.Value.TimeOfDay.ToString());
                    Console.WriteLine(date.Value.Hour + ":" + date.Value.Minute + " - nezavršeno" );
                    MyTermin termin = new MyTermin(subject.Name, "", date.Value, date.Value.AddMinutes(subject.ClassLength * 45));
                    kalendar.Schedule.Items.Add(termin);
                }
            }
        }

        private void AddTaskFromDragAndDrop(DragEventArgs e)
        {
            Point point = e.GetPosition(kalendar);
            DateTime? date = kalendar.GetDateAt(point);

            // TODO: Srediti da se može dodati termin "tik uz drugi"
            // trenutno preko štapa i kanapa [ ili nije? :) ]
            var allItems = kalendar.Schedule.GetAllItems(date.Value, date.Value.AddMinutes(30).AddSeconds(-1));
            bool zauzeto = allItems.Any();

            if (date != null)
            {
                MessageBoxResult messageBoxResult = MessageBoxResult.Yes;

                if (zauzeto) // postoji 1 ili više termina
                {
                    /** Prevod teksta na dugmadima <see cref="MainWindow()"/>*/
                    System.Windows.Forms.MessageBoxManager.Yes = "Da";
                    System.Windows.Forms.MessageBoxManager.No = "Ne";

                    messageBoxResult = MessageBox
                        .Show("Da li želite da obrišete termine koji se preklapaju?",
                        "Zauzet termin", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        // čišćenje polja sa taskovima
                        allItems.ToList().ForEach(x => kalendar.Schedule.Items.Remove(x));

                        zauzeto = false; // fleg za dodavanje novog
                    }
                }
                if (!zauzeto)
                {
                    string task = (string)e.Data.GetData(typeof(string));
                    MyTermin termin = new MyTermin(task, "", date.Value, date.Value.AddMinutes(30));
                    kalendar.Schedule.Items.Add(termin);
                }
            }
        }

        #endregion

        /** TODO: [low priority] razmotriti korištenje ovoga, ne znam ni šta je.. */
        private void ItemSettings_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine("\n sta ce sad biti?");
            if (e.Data.GetDataPresent(typeof(MyTermin)))
                Console.WriteLine("Termin drop! PUF!");
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
                //string data = ((ListBoxItem)SubjectsList.SelectedItem).Content.ToString();

                Console.WriteLine("mouseMOve: " + data);
                DragDrop.DoDragDrop(SubjectsList, data, DragDropEffects.Copy);
            }
        }

        #endregion
    }
}
