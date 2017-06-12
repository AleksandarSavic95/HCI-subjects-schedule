using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class FieldOfStudyHanlder
    {
        private static FieldOfStudyHanlder instance;

        public static FieldOfStudyHanlder Instance
        {
            get
            {
                if (instance == null)
                    instance = new FieldOfStudyHanlder();
                return instance;
            }
        }

        private Dictionary<string, FieldOfStudy> fieldsOfStudy;
        private FieldOfStudy selectedFieldOfStudy;

        public List<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                return fieldsOfStudy.Values.ToList();
            }
        }

        public FieldOfStudy SelectedFieldOfStudy
        {
            get
            {
                return selectedFieldOfStudy;
            }
            set
            {
                if (value == null)
                {
                    selectedFieldOfStudy = new FieldOfStudy();
                    // set defaults
                    selectedFieldOfStudy.Since = DateTime.Today;
                }
                else
                    selectedFieldOfStudy = fieldsOfStudy[value.Id];
            }
        }

        private FieldOfStudyHanlder()
        {
            fieldsOfStudy = new Dictionary<string, FieldOfStudy>();
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, fieldsOfStudy);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                fieldsOfStudy = ( Dictionary<string, FieldOfStudy> ) formatter.Deserialize(file);
            }
        }

        public void Add(string id, string name, DateTime since, string description)
        {
            fieldsOfStudy.Add(id, new FieldOfStudy(id, name, since, description));
        }

        public void Add(string id, string name, DateTime since, string description, MainWindow context)
        {
            Add(id, name, since, description);
            context.NotifyAll("FieldsOfStudy");
        }
  
        public void SetSelectedFieldOfStudy(FieldOfStudy fieldOfStudy, MainWindow context)
        {
            SelectedFieldOfStudy = fieldOfStudy;
            context.NotifyAll("SelectedFieldOfStudy");
        }

        public void SetSelectedFieldOfStudy(string id, MainWindow context)
        {
            SetSelectedFieldOfStudy(FindById(id), context);
        }

        public void Update(string id, string newId, string name, DateTime since, string description)
        {
            if (id != newId)
            {
                Remove(id);
                Add(newId, name, since, description);
            }
            else
                fieldsOfStudy[id] = new FieldOfStudy(id, name, since, description);
        }

        public void Update(string id, string newId, string name, DateTime since, string description, MainWindow context)
        {
            Update(id, newId, name, since, description);
            SetSelectedFieldOfStudy(newId, context);
            // may exist list change
            context.NotifyAll("FieldsOfStudy");
        }

        public bool Has(string id)
        {
            FieldOfStudy fos = new FieldOfStudy();
            
            return fieldsOfStudy.TryGetValue(id, out fos);
        }

        public FieldOfStudy FindById(string id)
        {
            return fieldsOfStudy[id];
        }

        public void Remove(string id)
        {
            fieldsOfStudy.Remove(id);
        }

        public void Remove(string id, MainWindow context)
        {
            Remove(id);
            context.NotifyAll("FieldsOfStudy");
        }

    }
}
