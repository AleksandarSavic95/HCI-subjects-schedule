using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaki komad softvera je opisan preko:
	/// jedinstvene, ljudski-čitljive oznake softvera,
	/// naziva softvera,
	/// operativnog sistema softvera(windows, linux, ili cross-platform),
	/// proizvođača softvera,
	/// sajta softvera,
	/// godine izdavanja softvera,
	/// cene softvera, i
	/// opisa softvera.
    /// </summary>
    class Software
    {
        string id;
        string name;
        OS operatingSystem;
        string producer;
        /// <summary>
        /// Sajt softvera. Da li je bolji tip "Uri"?
        /// </summary>
        string webSite;
        /// <summary>
        /// Godina izdavanja.
        /// </summary>
        string year;
        double price;
        string description;

        public Software() { }

        public Software(string id, string name, OS operatingSystem, string producer,
            string webSite, string year, double price, string description)
        {
            this.id = id;
            this.name = name;
            this.operatingSystem = operatingSystem;
            this.producer = producer;
            this.webSite = webSite;
            this.year = year;
            this.price = price;
            this.description = description;
        }

        public override string ToString()
        {
            return String.Format("\nSoftware <<{0}>> {1}\n", id, name);
        }
    }
}
