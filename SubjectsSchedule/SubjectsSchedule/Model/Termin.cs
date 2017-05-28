using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Termin nastave.
    /// </summary>
    class Termin
    {
        private string id; // vjerovatno ne treba
        private Classroom inClassroom;
        private Subject forSubject;
        private DateTime start;

        // da li ce ovo biti od koristi?
        //private TimeSpan timeSpan;
    }
}
