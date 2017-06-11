using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaka učionica računarskog centra se opisuje preko:
    /// * jedinstvene, ljudski-čitljive oznake učionice,
    /// * opisa učionice,
    /// * broja radnih mesta,
    /// * prisustva projektora,
    /// * prisustva table,
    /// * prisustva pametne table,
    /// * operativnog sistema u učionici { windows, linux, oboje }
    /// * instaliranog softvera u učionici.
    /// 
    /// Smatra se da sve učionice rade šest dana u nedelji (bez nedelje)
    /// od 07:00 do 22:00, a termin nastave može biti zakazan
    /// u bilo kom trenutku u tom periodu.
    /// </summary>
    [Serializable()]
    public class Classroom : INotifyPropertyChanged
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
        private string _description;
        private int _seats;
        private bool _projector;
        private bool _board;
        private bool _smartBoard;
        private OS _operatingSystem;
        private List<string> _installedSoftware;

        #endregion

        #region public properties
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public int Seats
        {
            get { return _seats; }
            set
            {
                if (_seats != value)
                {
                    _seats = value;
                    OnPropertyChanged("Seats");
                }
            }
        }
        public bool Projector
        {
            get { return _projector; }
            set
            {
                if (_projector  != value)
                {
                    _projector = value;
                    OnPropertyChanged("Projector");
                }
            }
        }
        public bool Board
        {
            get { return _board; }
            set
            {
                if (_board != value)
                {
                    _board = value;
                    OnPropertyChanged("Board");
                }
            }
        }
        public bool SmartBoard
        {
            get { return _smartBoard; }
            set
            {
                if (_smartBoard != value)
                {
                    _smartBoard = value;
                    OnPropertyChanged("SmartBoard");
                }
            }
        }
        public OS OperatingSystem
        {
            get { return _operatingSystem; }
            set
            {
                if (_operatingSystem != value)
                {
                    _operatingSystem = value;
                    OnPropertyChanged("OperatingSystem");
                }
            }
        }
        public List<string> InstalledSoftware
        {
            get { return _installedSoftware; }
            set
            {
                if (_installedSoftware != value)
                {
                    _installedSoftware = value;
                    OnPropertyChanged("InstalledSoftware");
                }
            }
        }

        #endregion

        public Classroom()
        {
            _installedSoftware = new List<string>();
        }

        public Classroom(string id, string description, int seats,
            bool projector, bool board, bool smartBoard, OS operatingSystem)
            : this()
        {
            _id = id;
            _description = description;
            _seats = seats;
            _projector = projector;
            _board = board;
            _smartBoard = smartBoard;
            _operatingSystem = operatingSystem;
        }

        public override string ToString()
        {
            return string.Format("\nClassroom <<{0}>>\nseats: {1}, " + 
                "projector: {2}\nboard: {3}, smart board: {4}, OS: {5}",
                _id, _seats, _projector, _board, _smartBoard, _operatingSystem);
        }

        public override bool Equals(object obj)
        {
            return this.Id.Equals(((Classroom)obj).Id);
        }
    }
}
