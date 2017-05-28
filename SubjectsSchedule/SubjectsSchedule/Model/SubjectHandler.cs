using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class SubjectHandler
    {
        private static SubjectHandler instance = null;

        public static SubjectHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new SubjectHandler();
                return instance;
            }
        }

        private Dictionary<string, Subject> subjects;

        public List<Subject> Subjects
        {
            get
            {
                return subjects.Values.ToList();
            }
        }

        private SubjectHandler()
        {
            subjects = new Dictionary<string, Subject>();
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, subjects);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                subjects = ( Dictionary<string, Subject> ) formatter.Deserialize(file);
            }
        }

        public void Add(string id, string name, string fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
        {
            subjects.Add(id, new Subject(id, name, fieldOfStudy, description, groupSize, classLength, terminNumber, needsProjector, needsBoard, needsSmartBoard, needsOS));
        }
        
        public bool Has(string id)
        {
            return true ? subjects[id] != null : false;
        }

        public Subject FindById(string id)
        {
            return subjects[id];
        }

        public void Remove(string id)
        {
            subjects.Remove(id);
        }
    }
}
