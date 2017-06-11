using MindFusion.Scheduling.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Schedules
{
    class CustomAppointmentPresenter : ItemPresenter
    {
        public CustomAppointmentPresenter(Calendar calendar)
               : base(calendar)
        {
            this.DefaultStyleKey = typeof(CustomAppointmentPresenter);
        }
    }
}
