using MindFusion.Scheduling;
using MindFusion.Scheduling.Wpf;
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
    /// Interaction logic for GlobalSchedule.xaml
    /// </summary>
    public partial class GlobalSchedule : UserControl
    {
        public GlobalSchedule()
        {
            InitializeComponent();
        }

        private void GlobalSchedule_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("GlobalSchedule_IsVisibleChanged!");
        }

        private void globalCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            globalCalendar.BeginInit();
            globalCalendar.TimetableSettings.Dates.Clear();

            /// Popunjavanje kalendara danima u sedmici <see cref="ScheduleDays"/>
            for (int i = 0; i < 6; i++)
                globalCalendar.TimetableSettings.Dates.Add(ScheduleDays.workDays[i]);

            globalCalendar.EndInit();

            // Define two locations to group by
            Location l1 = new Location();
            l1.Name = "L1";

            Location l2 = new Location();
            l2.Name = "L2";

            // Add the locations to the schedule
            globalCalendar.Schedule.Locations.Add(l1);
            globalCalendar.Schedule.Locations.Add(l2);

            // Select these contacts for grouping by adding them to
            // the appropriate Calendar collection
            globalCalendar.ItemLocations.Add(l1);
            globalCalendar.ItemLocations.Add(l2);
            Classroom c1 = new Classroom("L-1", "prva", 20, true, true, true, OS.C_BOTH);
            Classroom c2 = new Classroom("L-2", "druga", 16, false, false, false, OS.LINUX);
            Classroom c3 = new Classroom("L-3", "treca", 12, false, true, false, OS.WINDOWS);

            Resource r1 = new Resource();
            r1.Name = c1.Id;
            r1.Tag = c1;
            r1.Id = "r1";
            globalCalendar.ItemResources.Add(r1);

            Resource r2 = new Resource();
            r2.Name = c2.Id;
            r2.Tag = c2;
            r2.Id = "r2";
            globalCalendar.ItemResources.Add(r2);

            Resource r3 = new Resource();
            r3.Name = c3.Id;
            r3.Tag = c3;
            r3.Id = "r3";
            globalCalendar.ItemResources.Add(r3);

            Appointment app1 = new Appointment();
            app1.HeaderText = "APP 1";
            app1.DescriptionText = "app1 desc";
            app1.StartTime = ScheduleDays.workDays[0].AddHours(8); // isto vrijeme
            app1.EndTime = ScheduleDays.workDays[0].AddHours(8).AddMinutes(45);
            app1.Location = l1; // PRVA lokacija!
            app1.Resources.Add(r1);

            Appointment app2 = new Appointment();
            app2.HeaderText = "APP 2";
            app2.DescriptionText = "app2 desc";
            app2.StartTime = ScheduleDays.workDays[0].AddHours(8); // isto vrijeme
            app2.EndTime = ScheduleDays.workDays[0].AddHours(8).AddMinutes(45);
            app2.Location = l2; // DRUGA lokacija!
            app2.Resources.Add(r2);

            Appointment app3 = new Appointment();
            app3.HeaderText = "APP 3";
            app3.DescriptionText = "app3 desc";
            app3.StartTime = ScheduleDays.workDays[0].AddHours(10); // razl. vrijeme
            app3.EndTime = ScheduleDays.workDays[0].AddHours(10).AddMinutes(45);
            app3.Location = l2; // DRUGA lokacija!
            app3.Resources.Add(r3);

            globalCalendar.Schedule.Items.Add(app1);
            globalCalendar.Schedule.Items.Add(app2);
            globalCalendar.Schedule.Items.Add(app3);

            //DateTime start = ScheduleDays.workDays[0].AddHours(8);
            //DateTime end = ScheduleDays.workDays[0].AddHours(8).AddMinutes(45);
            //MyTermin termin = new MyTermin("app3", start, end);
            //globalCalendar.Schedule.Items.Add(termin);

            // Enable grouping by locations
            //globalCalendar.GroupType = GroupType.GroupByLocations;
            globalCalendar.GroupType = GroupType.GroupByResources;
            
        }
    }
}
