using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Model radnih dana koji se koriste u svim prikazima rasporeda.
    /// </summary>
    public static class ScheduleDays
    {
        public static List<DateTime> workDays = new List<DateTime>()
        {
            new DateTime(2017, 5, 22), // ponedeljak
            new DateTime(2017, 5, 23),
            new DateTime(2017, 5, 24),
            new DateTime(2017, 5, 25),
            new DateTime(2017, 5, 26),
            new DateTime(2017, 5, 27)
        };
    }
}
