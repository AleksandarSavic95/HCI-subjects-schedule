using MindFusion.Scheduling;
using SubjectsSchedule.Model;
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

namespace SubjectsSchedule.Schedules
{
    /// <summary>
    /// Interaction logic for ScheduleScheme.xaml
    /// </summary>
    public partial class ScheduleScheme : UserControl
    {
        private bool mouseDown;

        public ScheduleScheme()
        {
            InitializeComponent();

            // Da na kalendaru pravimo termine tipa MyTermin, a ne Appointment
            kalendar.InteractiveItemType = typeof(MyTermin);

            Console.WriteLine("start time: " + kalendar.TimetableSettings.StartTime);
            Console.WriteLine("end time: " + kalendar.TimetableSettings.EndTime);

            kalendar.ItemModifying += (s, e) =>
            {
                DateTime start = e.Item.StartTime;
                DateTime end = e.Item.EndTime;
                // ILI: " ... < kalendar.TimetableSettings.StartTime"
                if (start.TimeOfDay < TimeSpan.FromHours(7) ||
                    end.TimeOfDay > TimeSpan.FromHours(22))
                    e.Confirm = false;

                var items = kalendar.Schedule.GetAllItems(start, end);
                if (items.Except(new List<Item>() { e.Item }).Count() > 0)
                {
                    Console.WriteLine("Ima itema! Ne moze!");
                    e.Confirm = false;
                }
                else
                {
                    Console.WriteLine("NEEEMA itema! MOOOZE!");
                }

            };

            // 
            kalendar.ItemCreated += (s, e) =>
            {
                if (e.Item is MyTermin)
                {
                    //kalendar.ResetDrag();
                    Console.WriteLine("ItemCreated Event: moj termin!");
                }
            };

            // Serialization support
            Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);
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

            // ne radi "Lt-sr-SP", pa mora hrvatski ;(
            //kalendar.Culture = new System.Globalization.CultureInfo("hr-HR");

            /// Popunjavanje kalendara danima u sedmici <see cref="ScheduleDays"/>
            for (int i = 0; i < 6; i++)
                kalendar.TimetableSettings.Dates.Add(ScheduleDays.workDays[i]);

            kalendar.EndInit();

            /** Create an appointment *
            Appointment app = new Appointment();
            app.HeaderText = "Meet George";
            app.DescriptionText = "This is a sample appointment";

            / * *
            app.StartTime = ScheduleDays.workDays[0].Add(new TimeSpan(9, 15, 0));
            app.EndTime = app.StartTime.Add(new TimeSpan(0, 65, 0)); // * /

            / * *
            app.StartTime = new DateTime(2017, 5, 25, 14, 0, 0);
            app.EndTime = new DateTime(2017, 5, 25, 16, 30, 0); // * /

            kalendar.Schedule.Items.Add(app); // */
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
            if (e.Data.GetDataPresent(typeof(string)))
            {
                Point point = e.GetPosition(kalendar);
                DateTime? date = kalendar.GetDateAt(point);
                MyTermin existing = (MyTermin)kalendar.GetItemAt(point);
                if (date != null)
                {
                    MessageBoxResult messageBoxResult = MessageBoxResult.Yes;
                    if (existing != null) // postoji termin
                    {
                        messageBoxResult = MessageBox
                            .Show("Da li želite da obrišete postojeći termin?",
                            "Zauzet termin", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            // čišćenje polja sa taskovima
                            kalendar.GetItemAt(point).Task = null;

                            // nije dovoljno..
                            //existing = (Appointment)kalendar.GetItemAt(point);

                            existing = null;
                        }
                    }
                    if (existing == null)
                    {
                        string task = (string)e.Data.GetData(typeof(string));
                        MyTermin termin = new MyTermin();
                        termin.HeaderText = task;
                        termin.StartTime = date.Value;
                        termin.EndTime = termin.StartTime.AddMinutes(30);
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
