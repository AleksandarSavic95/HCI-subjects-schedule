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

        private void Calendar_ItemClick(object sender, MindFusion.Scheduling.Wpf.ItemMouseEventArgs e)
        {
            if (e.Item is MyTermin)
            {
                kalendar.ResetDrag();
                MessageBox.Show("This is our item.");
            }
        }
    }
}
