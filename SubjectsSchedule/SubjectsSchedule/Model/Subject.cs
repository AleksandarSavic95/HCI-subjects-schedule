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
        private string id;
        string name;
        /// <summary>
        /// ID smjera u mapi smjerova
        /// </summary>
        string fieldOfStudy; 
        string description;
        int groupSize;
        /// <summary>
        /// Minimalna dužina termina predmeta (u časovima od po 45 min)
        /// </summary>
        int classLength;
        int terminNumber;
        bool needsProjector;
        bool needsBoard;
        bool needsSmartBoard;
        /// <summary>
        /// Neophodni operativni sistem za nastavu {windows, linux, svejedno}
        /// </summary>
        OS needsOS;
        /// <summary>
        /// Neophodni softver za nastavu. Lista, jer može biti više od jednog.
        /// </summary>
        List<string> needsSoftware { get; set; }
    }
}
