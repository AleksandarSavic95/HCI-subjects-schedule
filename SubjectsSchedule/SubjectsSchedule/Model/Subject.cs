using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaki predmet je opisan preko:
    /// jedinstvene, LJUDSKI-ČITLJIVE oznake predmeta,
    /// naziva predmeta, smera predmeta, opisa predmeta,
    /// veličine grupe u kojoj se radi predmet,
    /// minimalne dužine termina predmeta(u časovima od po 45 min),
    /// broja termina,
    /// neophodnosti projektora, table i pametne table za nastavu,
    /// neophodnog operativnog sistema {windows, linux, svejedno} i softvera.
    /// </summary>
    [Serializable()]
    public class Subject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region private fields

        private string _id;
        private string _name;
        /// <summary>
        /// ID smjera u mapi smjerova
        /// </summary>
        private string _fieldOfStudy;
        private string _description;
        private int _groupSize;
        /// <summary>
        /// Minimalna dužina termina predmeta (u časovima od po 45 min)
        /// </summary>
        private int _classLength;
        /// <summary>
        /// Broj termina koje predmet zahtijeva.
        /// </summary>
        private int _terminNumber;
        private bool _needsProjector;
        private bool _needsBoard;
        private bool _needsSmartBoard;
        /// <summary>
        /// Neophodni operativni sistem za nastavu {windows, linux, svejedno}
        /// </summary>
        private OS _needsOS;
        /// <summary>
        /// Neophodni softver za nastavu. Lista, jer može biti više od jednog.
        /// </summary>
        private List<string> _needsSoftware;

        /// <summary>
        /// Broj temrina ovog predmeta koji nisu raspoređeni.
        /// </summary>
        private int _unscheduledTermins;

        #endregion

        #region Public props, most of which raise the OnPropertyChanged event
        

        public string Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; }
            set {
                if (value != _name) {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string FieldOfStudy { get { return _fieldOfStudy; }
            set {
                if (value != _fieldOfStudy) {
                    _fieldOfStudy = value;
                    OnPropertyChanged("FieldOfStudy");
                }
            }
        }
        public string Description { get { return _description; }
            set {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public int GroupSize { get { return _groupSize; } set { _groupSize = value; } }
        public int ClassLength { get { return _classLength; } set { _classLength = value; } }
        public int TerminNumber { get { return _terminNumber; }
            set {
                if (value != _terminNumber)
                {
                    _terminNumber = value;
                    OnPropertyChanged("TerminNumber");
                }
            }
        }

        public bool NeedsProjector { get { return _needsProjector; }
            set {
                if (value != _needsProjector)
                {
                    _needsProjector = value;
                    OnPropertyChanged("NeedsProjector");
                }
            }
        }
        public bool NeedsBoard { get { return _needsBoard; } set { _needsBoard = value; } }
        public bool NeedsSmartBoard { get { return _needsSmartBoard; } set { _needsSmartBoard = value; } }
        public OS NeedsOS { get { return _needsOS; } set { _needsOS = value; } }
        public List<string> NeedsSoftware { get { return _needsSoftware; } set { _needsSoftware = value; } }

        public int UnscheduledTermins
        {
            get { return _unscheduledTermins; }
            set
            {
                if (value != _unscheduledTermins)
                {
                    _unscheduledTermins = value;
                    OnPropertyChanged("UnscheduledTermins");
                }
            }
        }

        #endregion

        public Subject()
        {
            _needsSoftware = new List<string>();
        }

        public Subject(string id, string name, string fieldOfStudy, string description, int groupSize, int classLength, int terminNumber,
            bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
            : this()
        {
            this._id = id;
            this._name = name;
            this._fieldOfStudy = fieldOfStudy;
            this._description = description;
            this._groupSize = groupSize;
            this._classLength = classLength;

            this._terminNumber = terminNumber;
            this._unscheduledTermins = terminNumber; // bitno! - broj nerasp. termina

            this._needsProjector = needsProjector;
            this._needsBoard = needsBoard;
            this._needsSmartBoard = needsSmartBoard;
            this._needsOS = needsOS;
        }
    }
}
