using MindFusion.Scheduling;
using MindFusion.Scheduling.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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

        /// <summary>
        /// Brojač svih ikada napravljenih Termina.
        /// "Dobar" način za pravljenje jedinstvenog ID-a.
        /// Pogledati: <see cref="NextId"/>.
        /// </summary>
        private int _int_id;

        // Mapiranje ID-a učionice na listu ID-jeva termina koji se održavaju u njoj.
        public Dictionary<string, List<string>> TerminsByClassrooms { get; set; }

        // Mapiranje ID-a predmeta na listu ID-jeva termina koji postoje za njega.
        public Dictionary<string, List<string>> TerminsBySubjects { get; set; }

        // Mapiranje ID-a termina na konkretnu instancu. Predstavlja osnovu Handler-a.
        public Dictionary<string, MyTermin> TerminsByIds { get; set; }

        /// <summary>
        /// Pomoćna instanca klase <see cref="Calendar"/> u kojoj ćemo čuvati sve termine.
        /// </summary>
        private Calendar calendarInstance;

        /// <summary>
        /// Prikuplja sve instance termina koji se odžavaju u ovoj učionici.
        /// </summary>
        /// <param name="classroomId">id učionice za koju se prikupljaju termini</param>
        /// <returns>Lista termina koji se odžavaju u ovoj učionici</returns>
        public List<MyTermin> GetTerminsInClassroom(string classroomId)
        {
            List<MyTermin> retList = new List<MyTermin>();
            if (!TerminsByClassrooms.ContainsKey(classroomId))
            {
                AddClassroom(classroomId);
                return retList; // nema rezultata - prazna lista
            }
            List<string> invalidIds = new List<string>();
            foreach (var id in TerminsByClassrooms[classroomId])
                // bagovito je.. ovo okalšava život..
                if (TerminsByIds.ContainsKey(id))
                    retList.Add(TerminsByIds[id]);
                else
                    invalidIds.Add(id);

            foreach (var invalidId in invalidIds)
                TerminsByClassrooms[classroomId].Remove(invalidId);

            return retList;
        }

        /// <summary>
        /// Prikuplja sve instance termina koji postoje za ovaj predmet.
        /// </summary>
        /// <param name="classroomId">id predmeta za koji se prikupljaju termini</param>
        /// <returns>Lista termina koji postoje za ovaj predmet</returns>
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
            _int_id = 0;

            TerminsByClassrooms = new Dictionary<string, List<string>>();
            TerminsBySubjects = new Dictionary<string, List<string>>();
            TerminsByIds = new Dictionary<string, MyTermin>();

            calendarInstance = new Calendar();
        }

        /// <summary>
        /// Snima u 4 odvojene datoteke (redom):
        /// mapu terminId, MyTermin,
        /// mapu: classroomId, ListaTerminId,
        /// mapu: subjectId, ListaTerminId,
        /// brojac id-jeva: <see cref="_int_id"/>
        /// </summary>
        /// <param name="fileName">početak naziva ciljnih datoteka</param>
        public void Serialize(string fileName)
        {
            XMLSerializeTermins(fileName);

            using (Stream file = File.Open(fileName + "_ByC.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsByClassrooms);
            }
            using (Stream file = File.Open(fileName + "_ByS.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, TerminsBySubjects);
            }

            // brojac ID-jeva uvijek nastavlja gdje je stao, da ne bi bilo preklapanja tokom brisanja i slicno..
            using (Stream file = File.Open(fileName + "_int_ID.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, _int_id);
            }

        }

        private void XMLSerializeTermins(string fileName)
        {
            using (Stream file = File.Open(fileName + "_XML_ByID.xml", FileMode.Create))
            {
                var doc = new XmlDocument();
                //var context = new XmlSerializationContext(calendarInstance.Schedule, doc);
                var context = new XmlSerializationContext(new Schedule(), doc);

                /// Već registrovana u konsturktoru <see cref="Schedules.ScheduleScheme.ScheduleScheme"/>.
                //Schedule.RegisterItemClass(typeof(MyTermin), "mytermin", 1);

                var rootElement = doc.CreateElement("root");
                doc.AppendChild(rootElement);
                
                foreach (var item in TerminsByIds.Values)
                {
                    var itemElement = doc.CreateElement("item");
                    rootElement.AppendChild(itemElement);
                    item.SaveTo(itemElement, context);
                }

                doc.Save(file);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName + "_ByC.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsByClassrooms = (Dictionary<string, List<string>>)formatter.Deserialize(file);
            }
            using (Stream file = File.Open(fileName + "_ByS.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TerminsBySubjects = (Dictionary<string, List<string>>)formatter.Deserialize(file);
            }
            using (Stream file = File.Open(fileName + "_int_ID.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _int_id = (int)formatter.Deserialize(file);
            }

            XMLDeSerializeTermins(fileName);
        }

        private void XMLDeSerializeTermins(string fileName)
        {
            using (Stream file = File.Open(fileName + "_XML_ByID.xml", FileMode.Open))
            {
                var doc = new XmlDocument();
                doc.Load(file);

                var context = new XmlSerializationContext(new Schedule(), doc);

                SubjectHandler.Instance.ResetAllUncheduledTermins();

                foreach (var itemElement in doc.SelectNodes("root/item"))
                {
                    MyTermin item = new MyTermin();
                    item.LoadFrom((XmlElement)itemElement, context);
                    TerminsByIds.Add(item.Id, item);
                    // smanjimo broj NEraspoređenih termina za odgovarajući predmet.
                    SubjectHandler.Instance.ChangeUnscheduledTermins(item.ForSubject.Id);
                }
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
                TerminsByIds.Add(termin.Id, termin);
                if (!TerminsByClassrooms.ContainsKey(termin.InClassroom.Id))
                    TerminsByClassrooms.Add(termin.InClassroom.Id, new List<string>() { termin.Id });
                else
                    TerminsByClassrooms[termin.InClassroom.Id].Add(termin.Id);

                if (!TerminsBySubjects.ContainsKey(termin.ForSubject.Id))
                    TerminsBySubjects.Add(termin.ForSubject.Id, new List<string>() { termin.Id });
                else
                    TerminsBySubjects[termin.ForSubject.Id].Add(termin.Id);
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
        /// Brisanje učionice i svih termina iz "baze".
        /// </summary>
        /// <param name="id">id učionice koja se briše</param>
        public void RemoveClassroom(string id)
        {
            foreach (var terminId in TerminsByClassrooms[id])
                TerminsByIds.Remove(terminId);

            TerminsByClassrooms.Remove(id);
        }

        public void RemoveSubject(string id)
        {
            foreach (var terminId in TerminsBySubjects[id])
                TerminsByIds.Remove(terminId);

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
