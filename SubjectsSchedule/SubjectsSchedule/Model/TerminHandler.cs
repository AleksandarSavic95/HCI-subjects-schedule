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

        private int _int_id;

        public Dictionary<string, List<string>> TerminsByClassrooms { get; set; }

        public Dictionary<string, List<string>> TerminsBySubjects { get; set; }

        public Dictionary<string, MyTermin> TerminsByIds { get; set; }

        public List<MyTermin> GetTerminsInClassroom(string classroomId)
        {
            List<MyTermin> retList = new List<MyTermin>(TerminsByClassrooms[classroomId].Count);
            foreach (var id in TerminsByClassrooms[classroomId])
            {
                retList.Add(TerminsByIds[id]);
            }
            return retList;
        }

        public List<MyTermin> GetTerminsOfSubject(string subjectId)
        {
            List<MyTermin> retList = new List<MyTermin>(TerminsBySubjects[subjectId].Count);
            foreach (var id in TerminsBySubjects[subjectId])
            {
                retList.Add(TerminsByIds[id]);
            }
            return retList;
        }

        private TerminHandler()
        {
            _int_id = 1;

            TerminsByClassrooms = new Dictionary<string, List<string>>();
            TerminsBySubjects = new Dictionary<string, List<string>>();
            TerminsByIds = new Dictionary<string, MyTermin>();
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName + "_ByC", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsByClassrooms);
            }
            using (Stream file = File.Open(fileName + "_ByS", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsBySubjects);
            }
            using (Stream file = File.Open(fileName + "_ByID", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsByIds);
            }
            using (Stream file = File.Open(fileName + "_int_ID", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, _int_id);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName + "_ByC", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsByClassrooms = (Dictionary<string, List<string>>)formatter.Deserialize(file);
            }
            using (Stream file = File.Open(fileName + "_ByS", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsBySubjects = (Dictionary<string, List<string>>)formatter.Deserialize(file);
            }
            using (Stream file = File.Open(fileName + "_ByID", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsByIds = (Dictionary<string, MyTermin>)formatter.Deserialize(file);
            }
            using (Stream file = File.Open(fileName + "_int_ID", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _int_id = (int)formatter.Deserialize(file);
            }
        }


        private string NextId()
        {
            _int_id++;
            return _int_id.ToString();
        }

        #region Dodavanja
        public void AddTermin(MyTermin termin)
        {
            try
            {
                termin.Id = NextId();
                TerminsByClassrooms[termin.InClassroom.Id].Add(termin.Id);
                TerminsBySubjects[termin.ForSubject.Id].Add(termin.Id);
                TerminsByIds.Add(termin.Id, termin);
            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspjeli pokusaj dodavanja termina {0}<{1}> u ucionicu {2}",
                    termin.HeaderText, termin.Id, termin.InClassroom.Id);
                Console.WriteLine("Razlog: " + e.Message + "\n");
                //throw;
            }
        }

        public void AddClassroom(string classroomId)
        {
            TerminsByClassrooms.Add(classroomId, new List<string>());
        }
        public void AddSubject(string subjectId)
        {
            TerminsBySubjects.Add(subjectId, new List<string>());
        }

        /// <summary>
        /// Ne poziva se nikad?
        /// </summary>
        /// <param name="id"></param>
        /// <param name="header"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="classroom"></param>
        /// <param name="subject"></param>
        public void Add(string id, string header, DateTime start, DateTime end, Classroom classroom, Subject subject)
        {
            AddTermin(new MyTermin(NextId(), header, start, end, classroom, subject));
        }
        #endregion

        #region Brisanja
        /// <summary>
        /// Brisanje učionice iz "baze".
        /// </summary>
        /// <param name="id">id učionice koja se briše</param>
        public void RemoveClassroom(string id)
        {
            TerminsByClassrooms.Remove(id);
        }

        public void RemoveSubject(string id)
        {
            TerminsBySubjects.Remove(id);
        }

        public void RemoveTermin(MyTermin termin)
        {
            try
            {
                TerminsByClassrooms[termin.InClassroom.Id].Remove(termin.Id);
                TerminsBySubjects[termin.ForSubject.Id].Remove(termin.Id);
                TerminsByIds.Remove(termin.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Neuspjeli pokusaj brisanja termina {0} u ucionici {1}", termin.HeaderText, termin.InClassroom.Id);
                Console.WriteLine("Razlog: " + e.Message + "\n");
                //throw;
            }
        }
        #endregion
    }
}
