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

            /// Popunjavanje kalendara danima u sedmici <see cref="Model.ScheduleDays"/>
            for (int i = 0; i < 6; i++)
            {
                kalendar.TimetableSettings.Dates
                    .Add(Model.ScheduleDays.workDays[i]);
            }

            kalendar.EndInit();
        }
    }
}
