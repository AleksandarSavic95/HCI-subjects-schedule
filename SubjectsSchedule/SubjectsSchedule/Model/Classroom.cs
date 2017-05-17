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
        private string id { get; set; }
        private string description { get; set; }
        private int seats { get; set; }
        private bool projector { get; set; }
        private bool board { get; set; }
        private bool smartBoard { get; set; }
        private OS operatingSystem { get; set; }
        private List<string> installedSoftware { get; set; }

        public Classroom()
        {
            installedSoftware = new List<string>();
        }

        public Classroom(string id, string description, int seats,
            bool projector, bool board, bool smartBoard, OS operatingSystem)
        {
            this.id = id;
            this.description = description;
            this.seats = seats;
            this.projector = projector;
            this.board = board;
            this.smartBoard = smartBoard;
            this.operatingSystem = operatingSystem;
            installedSoftware = new List<string>();
        }

        public override string ToString()
        {
            return string.Format("\nClassroom <<{0}>>\nseats: {1}, " + 
                "projector: {2}\nboard: {3}, smart board: {4}, OS: {5}",
                id, seats, projector, board, smartBoard, operatingSystem);
        }
    }
}
