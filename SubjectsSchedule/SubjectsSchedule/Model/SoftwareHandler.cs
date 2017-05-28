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

        public List<Software> Softwares
        {
            get
            {
                return softwares.Values.ToList();
            }
        }

        private SoftwareHandler()
        {
            softwares = new Dictionary<string, Software>();
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

        public Software FindById(string id)
        {
            return softwares[id];
        }

        public void Remove(string id)
        {
            softwares.Remove(id);
        }
    }
}
