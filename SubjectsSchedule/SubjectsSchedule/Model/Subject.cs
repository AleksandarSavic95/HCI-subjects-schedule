using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaki predmet je opisan preko:
    /// jedinstvene, ljudski-čitljive oznake predmeta,
    /// naziva predmeta,
    /// smera predmeta,
    /// opisa predmeta,
    /// veličine grupe u kojoj se radi predmet,
    /// minimalne dužine termina predmeta(u časovima od po 45 min),
    /// broja termina koji predmet zahteva,
    /// neophodnosti projektora za nastavu,
    /// neophodnosti table za nastavu,
    /// neophodnosti pametne table za nastavu,
    /// neophodnog operativnog sistema za nastavu {windows, linux, svejedno},
    /// neophodnog softvera za nastavu.
    /// </summary>
    class Subject
    {

        #region private fields

        private string id;
        private string name;
        /// <summary>
        /// ID smjera u mapi smjerova
        /// </summary>
        private string fieldOfStudy;
        private string description;
        private int groupSize;
        /// <summary>
        /// Minimalna dužina termina predmeta (u časovima od po 45 min)
        /// </summary>
        private int classLength;
        private int terminNumber;
        private bool needsProjector;
        private bool needsBoard;
        private bool needsSmartBoard;
        /// <summary>
        /// Neophodni operativni sistem za nastavu {windows, linux, svejedno}
        /// </summary>
        private OS needsOS;
        /// <summary>
        /// Neophodni softver za nastavu. Lista, jer može biti više od jednog.
        /// </summary>
        private List<string> needsSoftware { get; set; }

        #endregion

        #region public properties

        public string Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string FieldOfStudy { get { return fieldOfStudy; } set { fieldOfStudy = value; } }
        public string Description { get { return description; } set { description = value; } }
        public int GroupSize { get { return groupSize; } set { groupSize = value; } }
        public int ClassLength { get { return classLength; } set { classLength = value; } }
        public int TerminNumber { get { return terminNumber; } set { terminNumber = value; } }
        public bool NeedsProjector { get { return needsProjector; } set { needsProjector = value; } }
        public bool NeedsBoard { get { return needsBoard; } set { needsBoard = value; } }
        public bool NeedsSmartBoard { get { return needsSmartBoard; } set { needsSmartBoard = value; } }
        public OS NeedsOS { get { return needsOS; } set { needsOS = value; } }
        public List<string> NeedsSoftware { get { return needsSoftware; } set { needsSoftware = value; } }

        #endregion

        public Subject()
        {
            needsSoftware = new List<string>();
        }

        public Subject(string id, string name, string fieldOfStudy, string description, int groupSize, int classLength, int terminNumber,
            bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
            : this()
        {
            this.id = id;
            this.name = name;
            this.fieldOfStudy = fieldOfStudy;
            this.description = description;
            this.groupSize = groupSize;
            this.classLength = classLength;
            this.terminNumber = terminNumber;
            this.needsProjector = needsProjector;
            this.needsBoard = needsBoard;
            this.needsSmartBoard = needsSmartBoard;
            this.needsOS = needsOS;
        }
    }
}
