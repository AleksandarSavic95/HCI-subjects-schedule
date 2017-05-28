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
    [Serializable()]
    class FieldOfStudy
    {
        #region private fields

        private string id;
        private string name;
        /// <summary>
        /// Datum uvođenja smjera
        /// </summary>
        private DateTime since;
        private string description;
        
        #endregion

        #region public properties

        public string Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public DateTime Since { get { return since; } set { since = value; } }
        public string Description { get { return description; } set { description = value; } }
        
        #endregion

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
