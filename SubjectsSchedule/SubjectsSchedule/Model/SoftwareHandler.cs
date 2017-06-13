using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class SoftwareHandler
    {
        private static SoftwareHandler instance = null;

        public static SoftwareHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoftwareHandler();
                return instance;
            }
        }

        private Dictionary<string, Software> softwares;
        private Software selectedSoftware;

        public List<Software> Softwares
        {
            get
            {
                return softwares.Values.ToList();
            }
        }

        public Software SelectedSoftware
        {
            get
            {
                return selectedSoftware;
            }
            set
            {
                if (value == null)
                {
                    selectedSoftware = new Software();

                    // set defaults
                }
                else
                    selectedSoftware = softwares[value.Id];
            }
        }

        private SoftwareHandler()
        {
            softwares = new Dictionary<string, Software>();
            SelectedSoftware = null;
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, softwares);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                softwares = ( Dictionary<string, Software> ) formatter.Deserialize(file);
            }
        }

        public void Add(string id, string name, OS operatingSystem, string producer, string webSite, string year, double price, string description)
        {
            softwares.Add(id, new Software(id, name, operatingSystem, producer, webSite, year, price, description));
        }

        public void Add(string id, string name, OS operatingSystem, string producer, string webSite, string year, double price, string description, MainWindow context)
        {
            Add(id, name, operatingSystem, producer, webSite, year, price, description);
            context.NotifyAll("Softwares");
        }

        public void SetSelectedSoftware(Software software, MainWindow context)
        {
            SelectedSoftware = software;
            context.NotifyAll("SelectedSoftware");
        }

        public void SetSelectedSoftware(string id, MainWindow context)
        {
            SetSelectedSoftware(FindById(id), context);
        }

        public void Update(string id, string newId, string name, OS operatingSystem, string producer, string webSite, string year, double price, string description)
        {
            if (id != newId)
            {
                Remove(id);
                Add(newId, name, operatingSystem, producer, webSite, year, price, description);
            }
            else
                softwares[id] = new Software(id, name, operatingSystem, producer, webSite, year, price, description);
        }

        public void Update(string id, string newId, string name, OS operatingSystem, string producer, string webSite, string year, double price, string description, MainWindow context)
        {
            Update(id, newId, name, operatingSystem, producer, webSite, year, price, description);
            SetSelectedSoftware(newId, context);
            // may exists list change
            context.NotifyAll("Softwares");
        }

        public bool Has(string id)
        {
            return softwares.ContainsKey(id);
        }

        public Software FindById(string id)
        {
            return softwares[id];
        }

        public void Remove(string id)
        {
            softwares.Remove(id);
        }

        public void Remove(string id, MainWindow context)
        {
            Remove(id);
            context.NotifyAll("Softwares");
        }
    }
}
