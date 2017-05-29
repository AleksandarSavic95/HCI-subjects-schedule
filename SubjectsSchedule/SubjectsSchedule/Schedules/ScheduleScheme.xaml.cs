using MindFusion.Scheduling;
using SubjectsSchedule.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ScheduleScheme : UserControl
    {
        private bool mouseDown;
        private ObservableCollection<Subject> PredmetiZaUcionicu;

        public ScheduleScheme()
        {
            InitializeComponent();

            // Da na kalendaru pravimo termine tipa MyTermin, a ne Appointment
            kalendar.InteractiveItemType = typeof(MyTermin);

            kalendar.ItemModifying += (s, e) =>
            {
                DateTime start = e.Item.StartTime;
                DateTime end = e.Item.EndTime;

                if (start.TimeOfDay < TimeSpan.FromHours(7) ||
                    end.TimeOfDay > TimeSpan.FromHours(22))
                    e.Confirm = false;

                var items = kalendar.Schedule.GetAllItems(start, end);
                // https://stackoverflow.com/a/407741/2101117 <- prvi komentar: Any efikasnije od Count
                if (items.Except(new List<Item>() { e.Item }).Any())
                    e.Confirm = false;
            };

            // 
            kalendar.ItemCreated += (s, e) =>
            {
                if (e.Item is MyTermin)
                //kalendar.ResetDrag();
                    Console.WriteLine("ItemCreated Event: moj termin!");
            };

            // Serialization support
            Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);

            Classroom svemoguca = new Classroom("1", "", 30, true, true, true, OS.C_BOTH);
            svemoguca.InstalledSoftware.AddRange(new List<string>() { "1", "2", "3", "4" });

            SubjectHandler.Instance.Add("1", "subj1", "1", "oSubj1", 20, 1, 2, false, true, true, OS.WINDOWS);
            SubjectHandler.Instance.Add("2", "subj2", "2", "oSubj2", 22, 2, 1, false, true, true, OS.SUBJ_WHATEVER);
            SubjectHandler.Instance.Add("3", "bazePod", "Siit3", "oSubj3", 18, 3, 1, true, true, false, OS.WINDOWS);

            PredmetiZaUcionicu = new ObservableCollection<Subject>(SubjectHandler.Instance.FindByClassroom(svemoguca));
            Console.WriteLine(PredmetiZaUcionicu.Count);
        }

        private void kalendar_ItemClick(object sender, MindFusion.Scheduling.Wpf.ItemMouseEventArgs e)
        {
            if (e.Item is MyTermin)
            {
                //kalendar.ResetDrag();
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
        }

        private void kalendar_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string))) // TODO: promijeniti tip!
            {
                Point point = e.GetPosition(kalendar);
                DateTime? date = kalendar.GetDateAt(point);

                var allItems = kalendar.Schedule.GetAllItems(date.Value, date.Value.AddMinutes(30));
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
        }

        private void ItemSettings_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine("\n sta ce sad biti?");
            if (e.Data.GetDataPresent(typeof(MyTermin)))
                Console.WriteLine("Termin drop! PUF!");
        }
        
        /** Drag and drop podrška za listView kontrolu. */
        private void listaPredmeta_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void listaPredmeta_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
