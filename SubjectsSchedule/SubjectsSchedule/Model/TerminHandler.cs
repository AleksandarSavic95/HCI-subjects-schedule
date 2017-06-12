using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class TerminHandler
    {
        private static TerminHandler instance = null;

        public static TerminHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new TerminHandler();
                return instance;
            }
        }

        private Dictionary<string, List<MyTermin>> terminsByClassrooms;

        public List<MyTermin> GetTerminsInClassroom(string classroomId)
        {
            return terminsByClassrooms[classroomId];
        }

        private TerminHandler()
        {
            terminsByClassrooms = new Dictionary<string, List<MyTermin>>();
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, terminsByClassrooms);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                terminsByClassrooms = (Dictionary<string, List<MyTermin>>)formatter.Deserialize(file);
            }
        }

        public void Add(string id, MyTermin termin)
        {
            terminsByClassrooms[id].Add(termin);
        }

        public void Add(string id, string header, DateTime start, DateTime end, Classroom classroom, Subject subject)
        {
            terminsByClassrooms[id].Add(new MyTermin(header, start, end, classroom, subject));
        }

        /// <summary>
        /// Brisanje učionice iz "baze".
        /// </summary>
        /// <param name="id">id učionice koja se briše</param>
        public void Remove(string id)
        {
            terminsByClassrooms.Remove(id);
        }
    }
}
