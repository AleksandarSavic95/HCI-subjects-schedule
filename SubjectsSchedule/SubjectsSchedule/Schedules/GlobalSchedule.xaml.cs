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

            // disable idem modification + AllowInPlaceEdit/Create = false
            globalCalendar.ItemStartModifying += (s, e) => e.Confirm = false;
            globalCalendar.ItemDeleting += (s, e) => e.Confirm = false;

            //globalCalendar.ItemSelecting += (s, e) => e.Confirm = false; // nema mnogo svrhe..
            //Schedule.RegisterItemClass(typeof(MyTermin), ",mytermin", 1);//ima vec..
            globalCalendar.InteractiveItemType = typeof(MyTermin);
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

            //DefineLocations();
            /// nema potrebe zbog PopulateResources..
            //DefineResources();

            /// poziva se iz <see cref="MainWindow.DockPanelLoaded"/>
            //PopulateResources();

            /* Appointments creation START */
            //CreateAppointments(); // stavljeno u svrhu testiranja u PopulateResources

            /// poziva se iz <see cref="MainWindow.DockPanelLoaded"/>
            //LoadTerminsToGlobal();

            // Enable grouping by locations or resources
            //globalCalendar.GroupType = GroupType.GroupByLocations;
            globalCalendar.GroupType = GroupType.GroupByResources;

            // Da mora da drži ALT da bi pomjerio u kolonu sa drugim resursom/lokacijom
            //globalCalendar.ItemChangeReferenceKey = ModifierKeys.Alt | ModifierKeys.Control;
        }

        // ucitavanje termina prvi put
        public void LoadTerminsToGlobal()
        {
            Console.WriteLine("Loading termins.... IZ JEDINSTVENOG RJECNIKA TERMINA!");
            globalCalendar.Schedule.Items.Clear();
            foreach (var terminPair in TerminHandler.Instance.TerminsByIds)
            {
                if (!terminPair.Value.Resources.Contains(globalCalendar.Schedule.Resources[terminPair.Value.InClassroom.Id]))
                    terminPair.Value.Resources.Add(globalCalendar.Schedule.Resources[terminPair.Value.InClassroom.Id]);

                foreach (var r in terminPair.Value.Resources)
                    Console.WriteLine("Termin {0} ima resurs {1}", terminPair.Value.HeaderText, r.Id);

                globalCalendar.Schedule.Items.Add(terminPair.Value);
            }
        }

        /// <summary>
        /// Ucitavanje ucionica u resurse globalnog kalendara. Po tim resursima se grupisu elementi.
        /// </summary>
        public void PopulateResources()
        {
            Resource r;
            Console.WriteLine("Populating global schedule's resources with {0} classrooms", ClassroomHandler.Instance.Classrooms.Count);
            foreach (var c in ClassroomHandler.Instance.Classrooms)  // ((MainWindow)Window.GetWindow(this)).ClassroomTabela.Classrooms ??
            {
                r = new Resource();
                r.Name = c.Id;
                r.Tag = c;
                r.Id = c.Id;
                globalCalendar.Schedule.Resources.Add(r);
                globalCalendar.ItemResources.Add(r);
            }
            /// Testiranje
            //CreateAppointments();
        }

        internal void AddResource(Classroom c)
        {
            Resource r = new Resource();
            r.Name = c.Id;
            r.Tag = c;
            r.Id = c.Id;
            globalCalendar.Schedule.Resources.Add(r);
            globalCalendar.ItemResources.Add(r);
        }

        private void CreateAppointments()
        {
            Appointment app1 = new Appointment();
            app1.HeaderText = "APP 1";
            app1.DescriptionText = "app1 desc";
            app1.StartTime = ScheduleDays.workDays[0].AddHours(8); // isto vrijeme
            app1.EndTime = ScheduleDays.workDays[0].AddHours(8).AddMinutes(45);
            //app1.Location = globalCalendar.Schedule.Locations[0]; // PRVA lokacija!
            app1.Resources.Add(globalCalendar.Schedule.Resources[0]);

            Appointment app2 = new Appointment();
            app2.HeaderText = "APP 2";
            app2.DescriptionText = "app2 desc";
            app2.StartTime = ScheduleDays.workDays[0].AddHours(8); // isto vrijeme
            app2.EndTime = ScheduleDays.workDays[0].AddHours(8).AddMinutes(45);
            //app2.Location = globalCalendar.Schedule.Locations[1]; // DRUGA lokacija!
            app2.Resources.Add(globalCalendar.Schedule.Resources[1]);

            Appointment app3 = new Appointment();
            app3.HeaderText = "APP 3";
            app3.DescriptionText = "app3 desc";
            app3.StartTime = ScheduleDays.workDays[3].AddHours(9); // razl. vrijeme
            app3.EndTime = ScheduleDays.workDays[3].AddHours(9).AddMinutes(45);
            //app3.Location = globalCalendar.Schedule.Locations[1]; // DRUGA lokacija!
            app3.Resources.Add(globalCalendar.Schedule.Resources[1]);

            globalCalendar.Schedule.Items.Add(app1);
            globalCalendar.Schedule.Items.Add(app2);
            globalCalendar.Schedule.Items.Add(app3);

            // Appointments creation END */

            DateTime start = ScheduleDays.workDays[0].AddHours(9);
            DateTime end = ScheduleDays.workDays[0].AddHours(9).AddMinutes(20);
            MyTermin termin = new MyTermin("app3", start, end);
            termin.Resources.Add(globalCalendar.Schedule.Resources[1]);
            globalCalendar.Schedule.Items.Add(termin);
        }

        /// <summary>
        /// Ažuriranje termina ili brisanje ako je <code>deleted = true</code>.
        /// </summary>
        /// <param name="myTermin">termin koji ažuriramo</param>
        /// <param name="deleted">da li je obrisan u raporedu učionice</param>
        internal void UpdateTermin(MyTermin myTermin, bool deleted = false)
        {
            Console.WriteLine("Prije azuriranja ima {0} itema.", globalCalendar.Schedule.Items.Count);
            if (!deleted)
            {
                Console.WriteLine("Trazimo termin {0}", myTermin.Id);
                Console.WriteLine("stara lokacija je: {0} - {1}", myTermin.StartTime, myTermin.EndTime);
            }
            else
                globalCalendar.Schedule.Items.Remove(myTermin);

            Console.WriteLine("Posliej ima {0} itema.", globalCalendar.Schedule.Items.Count);
        }

        private void DefineResources()
        {
            Classroom c1 = new Classroom("L-1", "prva", 20, true, true, true, OS.C_BOTH);
            Resource r1 = new Resource();
            r1.Name = c1.Id;
            r1.Tag = c1;
            r1.Id = "r1";
            globalCalendar.Schedule.Resources.Add(r1);
            globalCalendar.ItemResources.Add(r1);

            Classroom c2 = new Classroom("L-2", "druga", 16, false, false, false, OS.LINUX);
            Resource r2 = new Resource();
            r2.Name = c2.Id;
            r2.Tag = c2;
            r2.Id = "r2";
            globalCalendar.Schedule.Resources.Add(r2);
            globalCalendar.ItemResources.Add(r2);

            Classroom c3 = new Classroom("L-3", "treca", 12, false, true, false, OS.WINDOWS);
            Resource r3 = new Resource();
            r3.Name = c3.Id;
            r3.Tag = c3;
            r3.Id = "r3";
            globalCalendar.Schedule.Resources.Add(r3);
            globalCalendar.ItemResources.Add(r3);
        }

        private void DefineLocations()
        {
            // Define locations to group by
            Location loc1 = new Location();
            Location loc2 = new Location();
            Location loc3 = new Location();
            loc1.Name = "L1";
            loc2.Name = "L2";
            loc3.Name = "L3";

            // ~ Add the locations to the schedule
            globalCalendar.Schedule.Locations.Add(loc1);
            globalCalendar.Schedule.Locations.Add(loc2);
            globalCalendar.Schedule.Locations.Add(loc3);

            // ~ Select these locations for grouping by adding them to
            // the appropriate Locations collection
            globalCalendar.ItemLocations.Add(loc1);
            globalCalendar.ItemLocations.Add(loc2);
            globalCalendar.ItemLocations.Add(loc3);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            foreach (var res in grid.Resources)
            {
                Console.WriteLine(res);
                Console.WriteLine(((MindFusion.Scheduling.Resource)res).Tag);
                Console.WriteLine( ((Classroom) (((MindFusion.Scheduling.Resource)res).Tag)).Id);
            }
            var text = grid.Children[0] as TextBlock;

            if ((Window.GetWindow(this) as MainWindow).Obavjestenja[""])
                MessageBox.Show("Prikaz rasporeda za ucionicu: " + text.Text +
                    "\nOvo obavještenje možete sakriti u meniju \"Obavještenja\".");

            MainWindow parent = (MainWindow)Window.GetWindow(this);
            Classroom c = ClassroomHandler.Instance.FindById(text.Text);

            parent.PrikazRasporedaUcionice(c);
        }

        private void GlobalCalendar_LayoutUpdated(object sender, EventArgs e)
        {
            //Console.WriteLine("GlobalCalendar_LayoutUpdated!");
            //Console.WriteLine(globalCalendar.Schedule.Items.Count);
        }

        /// <summary>
        /// Kopira termin sa rasporeda učionice na globalni raspored, uz dodavanje resusra.
        /// </summary>
        /// <param name="termin">termin koji se kopira</param>
        internal void CopyItem(MyTermin termin)
        {
            Console.WriteLine("Copy MyTermin sa id: " + termin.Id + " uz dodavanje resura "+ 
                globalCalendar.Schedule.Resources[termin.InClassroom.Id].Name);

            termin.Resources.Add(GetResourceForClassroom(termin.InClassroom.Id));

            globalCalendar.Schedule.Items.Add(termin);
        }

        internal Resource GetResourceForClassroom(string selectedClassroomId)
        {
            //
            Resource retVal1 = globalCalendar.ItemResources[selectedClassroomId];
            Resource retVal2 = globalCalendar.Schedule.Resources[selectedClassroomId]; // ???

            Console.WriteLine("resurs za {0}: {1}~itemresoruces | {2}~sched.resources",
                selectedClassroomId, retVal1.Id, retVal2.Id);

            if (retVal1 == null)
            {
                Console.WriteLine("nadjeni Resource - null ~ nije nadjen?");
                return new Resource();
            }
            return retVal1;
        }
    }
}
