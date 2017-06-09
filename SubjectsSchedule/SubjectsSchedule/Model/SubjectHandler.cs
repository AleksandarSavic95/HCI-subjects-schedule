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

        public void TryAdd(string id, string name, string fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
        {
            try
            {
                subjects.Add(id, new Subject(id, name, fieldOfStudy,
                    description, groupSize, classLength, terminNumber,
                    needsProjector, needsBoard, needsSmartBoard, needsOS));
            }
            catch (Exception e)
            {
                Console.WriteLine("Nije dodat predmet sa ID = " + id + ". Razlog:\n" + e.Message);
            }
        }

        public Subject FindById(string id)
        {
            return subjects[id];
        }

        public List<Subject> FindByClassroom(Classroom classroom)
        {
            List<Subject> resultList = new List<Subject>();

            List<string> toFind = classroom.InstalledSoftware;

            HashSet<string> hashSet = new HashSet<string>(toFind);
            //bool contained = this.Subjects[0].NeedsSoftware.All(i => hashSet.Contains(i));

            foreach (Subject s in this.Subjects)
            {
                //if (s.NeedsSoftware.Except(toFind).Any())
                if (s.NeedsSoftware.All(i => hashSet.Contains(i)))
                {
                    Console.WriteLine("sadrzi predmet: " + s.Name);
                    resultList.Add(s);
                }
            }
            return resultList;
        }

        public void Remove(string id)
        {
            subjects.Remove(id);
        }

        /// <summary>
        /// Ažurira broj neraspoređenih termina za predmet sa Id = id.
        /// </summary>
        /// <param name="id">id predmeta koji mijenjamo</param>
        /// <param name="dropped">true ako je broj NEraspoređenih temina opao.</param>
        public void ChangeUnscheduledTermins(string id, bool dropped = true)
        {
            try
            {
                FindById(id); // već baca izuzetak jer koristimo [] operator
            }
            catch (Exception e)
            {
                Console.WriteLine("Nema id-ja << " + id + " >> u bazi! Poruka:");
                Console.WriteLine(e.Message);
            }
            //    throw new Exception("Promjena UnschTermina za nepostojeci ID!");

            if (dropped)
                FindById(id).UnscheduledTermins -= 1;
            else
                FindById(id).UnscheduledTermins += 1;
        }
    }
}
