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

        public Dictionary<string, List<string>> TerminsByClassrooms { get; set; }

        public Dictionary<string, List<string>> TerminsBySubjects { get; set; }


        public List<string> GetTerminsInClassroom(string classroomId)
        {
            return TerminsByClassrooms[classroomId];
        }

        private TerminHandler()
        {
            TerminsByClassrooms = new Dictionary<string, List<string>>();
            TerminsBySubjects = new Dictionary<string, List<string>>();

        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsByClassrooms);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsByClassrooms = (Dictionary<string, List<string>>)formatter.Deserialize(file);
            }
        }

        public void AddTermin(string classroomId, MyTermin termin)
        {
            try
            {
                TerminsByClassrooms[classroomId].Add(termin.Id);
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
            TerminsByClassrooms.Add(classroomId, new List<string>());
        }

        public void Add(string id, string header, DateTime start, DateTime end, Classroom classroom, Subject subject)
        {
            Console.WriteLine("TerminHandler add sa konstruktorom - fali new MyTermin(....)!");
            TerminsByClassrooms[id].Add(id);
        }

        /// <summary>
        /// Brisanje učionice iz "baze".
        /// </summary>
        /// <param name="id">id učionice koja se briše</param>
        public void Remove(string id)
        {
            TerminsByClassrooms.Remove(id);
        }

        public void RemoveTermin(string classroomId, MyTermin termin)
        {
            try
            {
                TerminsByClassrooms[classroomId].Remove(termin.Id);
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
