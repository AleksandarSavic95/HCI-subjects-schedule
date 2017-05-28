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
    [Serializable()]
    class Software
    {

        #region private fields

        private string id;
        private string name;
        private OS operatingSystem;
        private string producer;
        /// <summary>
        /// Sajt softvera. Da li je bolji tip "Uri"?
        /// </summary>
        private string webSite;
        /// <summary>
        /// Godina izdavanja.
        /// </summary>
        private string year;
        private double price;
        private string description;

        #endregion

        #region public properties

        public string Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public OS OperatingSystem { get { return operatingSystem; } set { operatingSystem = value; } }
        public string Producer { get { return producer; } set { producer = value; } }
        public string WebSite { get { return webSite; } set { webSite = value; } }
        public string Year { get { return year; } set { year = value; } }
        public double Price { get { return price; } set { price = value; } }
        public string Description { get { return description; } set { description = value; } }

        #endregion

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
