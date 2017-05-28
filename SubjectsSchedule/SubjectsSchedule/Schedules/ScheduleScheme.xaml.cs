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

            // Da na kalendaru pravimo termine tipa MyTermin, a ne Appointment
            kalendar.InteractiveItemType = typeof(MyTermin);

            kalendar.EndInit();

            // Serialization support
            //Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);

            // Create an appointment
            Appointment app = new Appointment();
            app.HeaderText = "Meet George";
            app.DescriptionText = "This is a sample appointment";

            /* *
            app.StartTime = ScheduleDays.workDays[0].Add(new TimeSpan(9, 15, 0));
            app.EndTime = app.StartTime.Add(new TimeSpan(0, 65, 0)); // */

            /* */
            app.StartTime = new DateTime(2017, 5, 25, 14, 0, 0);
            app.EndTime = new DateTime(2017, 5, 25, 16, 30, 0); // */

            kalendar.Schedule.Items.Add(app);
        }

        private void kalendar_ItemClick(object sender, MindFusion.Scheduling.Wpf.ItemMouseEventArgs e)
        {
            if (e.Item is MyTermin)
            {
                kalendar.ResetDrag();
                MessageBox.Show("This is our item.");
            }
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
                Appointment existing = (Appointment)kalendar.GetItemAt(point);
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
                        Appointment appointment = new Appointment();
                        appointment.HeaderText = task;
                        appointment.StartTime = date.Value;
                        appointment.EndTime = appointment.StartTime.AddMinutes(30);
                        kalendar.Schedule.Items.Add(appointment);
                    }
                }
            }
        }
    }
}
