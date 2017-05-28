using System;
using System.Collections.Generic;
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
    class Classroom
    {
        #region private fields

        private string id;
        private string description;
        private int seats;
        private bool projector;
        private bool board;
        private bool smartBoard;
        private OS operatingSystem;
        private List<string> installedSoftware;

        #endregion

        #region public properties

        public string Id { get { return id; } set { id = value; } }
        public string Description { get { return description; } set { description = value; } }
        public int Seats { get { return seats; } set { seats = value; } }
        public bool Projector { get { return projector; } set { projector = value; } }
        public bool Board { get { return board; } set { board = value; } }
        public bool SmartBoard { get { return smartBoard; } set { smartBoard = value; } }
        public OS OperatingSystem { get { return operatingSystem; } set { operatingSystem = value; } }
        public List<string> InstalledSoftware { get { return installedSoftware; } set { installedSoftware = value; } }

        #endregion

        public Classroom()
        {
            installedSoftware = new List<string>();
        }

        public Classroom(string id, string description, int seats,
            bool projector, bool board, bool smartBoard, OS operatingSystem)
            : this()
        {
            this.id = id;
            this.description = description;
            this.seats = seats;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.operatingSystem = operatingSystem;
        }

        public override string ToString()
        {
            return string.Format("\nClassroom <<{0}>>\nseats: {1}, " + 
                "projector: {2}\nboard: {3}, smart board: {4}, OS: {5}",
                id, seats, projector, board, smartBoard, operatingSystem);
        }
    }
}
