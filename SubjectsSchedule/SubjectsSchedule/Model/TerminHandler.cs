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

        private Dictionary<string, List<MyTermin>> _terminsByClassrooms;

        public Dictionary<string, List<MyTermin>> TerminsByClassrooms { get; set; }

        public List<MyTermin> GetTerminsInClassroom(string classroomId)
        {
            return _terminsByClassrooms[classroomId];
        }

        private TerminHandler()
        {
            _terminsByClassrooms = new Dictionary<string, List<MyTermin>>();
            TerminsByClassrooms = _terminsByClassrooms;

        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, _terminsByClassrooms);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _terminsByClassrooms = (Dictionary<string, List<MyTermin>>)formatter.Deserialize(file);
            }
        }

        public void AddTermin(string classroomId, MyTermin termin)
        {
            try
            {
                _terminsByClassrooms[classroomId].Add(termin);
            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspjeli pokusaj dodavanja termina {0} u ucionici {1}", termin.HeaderText, classroomId);
                Console.WriteLine("Razlog: " + e.Message + "\n");
                //throw;
            }
        }

        public void AddClassroom(string classroomId)
        {
            _terminsByClassrooms.Add(classroomId, new List<MyTermin>());
        }

        public void Add(string id, string header, DateTime start, DateTime end, Classroom classroom, Subject subject)
        {
            _terminsByClassrooms[id].Add(new MyTermin(header, start, end, classroom, subject));
        }

        /// <summary>
        /// Brisanje učionice iz "baze".
        /// </summary>
        /// <param name="id">id učionice koja se briše</param>
        public void Remove(string id)
        {
            _terminsByClassrooms.Remove(id);
        }

        public void RemoveTermin(string classroomId, MyTermin termin)
        {
            try
            {
                _terminsByClassrooms[classroomId].Remove(termin);
            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspjeli pokusaj brisanja termina {0} u ucionici {1}", termin.HeaderText, classroomId);
                Console.WriteLine("Razlog: " + e.Message + "\n");
                //throw;
            }
        }
    }
}
