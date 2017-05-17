using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaki smer je opisan preko:
    /// jedinstvene, ljudski-čitljive oznake smera,
    /// naziva smera,
    /// datuma uvođenja smera, i
    /// opisa smera
    /// </summary>
    class FieldOfStudy
    {
        string id;
        string name;
        /// <summary>
        /// Datum uvođenja smjera
        /// </summary>
        DateTime since;
        string description;

        public FieldOfStudy() { }

        public FieldOfStudy(string id, string name,
            DateTime since, string description)
        {
            this.id = id;
            this.name = name;
            this.since = since;
            this.description = description;
        }

        public override string ToString()
        {
            return String.Format("Field of study <<{0}>> {1}", id, name);
        }
    }
}
