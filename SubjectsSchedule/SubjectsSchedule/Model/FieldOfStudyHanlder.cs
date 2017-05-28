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

        public List<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                return fieldsOfStudy.Values.ToList();
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

        public FieldOfStudy FindById(string id)
        {
            return fieldsOfStudy[id];
        }

        public void Remove(string id)
        {
            fieldsOfStudy.Remove(id);
        }
        
    }
}
