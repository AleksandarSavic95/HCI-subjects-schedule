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
        private Subject selectedSubject;

        public List<Subject> Subjects
        {
            get
            {
                return subjects.Values.ToList();
            }
        }

        public Subject SelectedSubject
        {
            get
            {
                return selectedSubject;
            }
            set
            {
                if (value == null)
                {
                    selectedSubject = new Subject();

                    // set defaults
                }
                else
                    selectedSubject = subjects[value.Id];
            }
        }

        private SubjectHandler()
        {
            subjects = new Dictionary<string, Subject>();
            SelectedSubject = null;
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

            foreach (var subjId in subjects.Keys)
                TerminHandler.Instance.AddSubject(subjId);
        }

        public Subject Add(string id, string name, FieldOfStudy fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
        {
            Subject toAdd = new Subject(id, name, fieldOfStudy,
                description, groupSize, classLength, terminNumber,
                needsProjector, needsBoard, needsSmartBoard, needsOS);
            subjects.Add(id, toAdd);
            return toAdd;
        }

        public Subject Add(string id, string name, FieldOfStudy fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS, MainWindow context)
        {
            Subject added = Add(id, name, fieldOfStudy, description, groupSize, classLength, terminNumber, needsProjector, needsBoard, needsSmartBoard, needsOS);
            context.NotifyAll("Subjects");
            TerminHandler.Instance.AddSubject(id);
            return added;
        }

        public void SetSelectedSubject(Subject subject, MainWindow context)
        {
            SelectedSubject = subject;
            context.NotifyAll("SelectedSubject");
        }

        public void SetSelectedSubject(string id, MainWindow context)
        {
            SetSelectedSubject(FindById(id), context);
        }

        public void Update(string id, string newId, string name, FieldOfStudy fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS)
        {
            if (id != newId)
            {
                Remove(id);
                Add(id, name, fieldOfStudy, description, groupSize, classLength, terminNumber, needsProjector, needsBoard, needsSmartBoard, needsOS);
            }
            else
                subjects[id] = new Subject(id, name, fieldOfStudy, description, groupSize, classLength, terminNumber, needsProjector, needsBoard, needsSmartBoard, needsOS);
        }

        public void Update(string id, string newId, string name, FieldOfStudy fieldOfStudy, string description, int groupSize, int classLength,
            int terminNumber, bool needsProjector, bool needsBoard, bool needsSmartBoard, OS needsOS, MainWindow context)
        {
            Update(id, newId, name, fieldOfStudy, description, groupSize, classLength, terminNumber, needsProjector, needsBoard, needsSmartBoard, needsOS);
            SetSelectedSubject(newId, context);
            // may exists list change
            context.NotifyAll("Subjects");
        }

        public bool Has(string id)
        {
            return subjects.ContainsKey(id);
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

        public void Remove(string id, MainWindow context)
        {
            Remove(id);
            context.NotifyAll("Subjects");
        }

        /// <summary>
        /// Ažurira broj neraspoređenih termina za predmet sa Id = id.
        /// </summary>
        /// <param name="id">id predmeta koji mijenjamo</param>
        /// <param name="dropped">true ako je broj NEraspoređenih temina opao.</param>
        public void ChangeUnscheduledTermins(string id, bool dropped = true)
        {
            Subject toUpdate = null;
            try
            {
                toUpdate = FindById(id); // već baca izuzetak jer koristimo [] operator
            }
            catch (Exception e)
            {
                Console.WriteLine("Nema id-ja << " + id + " >> u bazi! Poruka:");
                Console.WriteLine(e.Message);
                // throw new Exception("Promjena UnschTermina za nepostojeci ID!");
            }
            if (dropped)
                if (toUpdate.UnscheduledTermins > 0)
                    toUpdate.UnscheduledTermins -= 1;
                else
                    Console.WriteLine("UNDERflow broja nerasp. za predmet " + toUpdate.Id);
            else
                if (toUpdate.UnscheduledTermins < toUpdate.TerminNumber-1)
                    toUpdate.UnscheduledTermins += 1;
                else
                    Console.WriteLine("OVERflow broja nerasp. za predmet " + toUpdate.Id);
        }

        /// <summary>
        /// Brisanje svih RASporedjenih termina ako nema učitanih termina (zbog greške i slično).
        /// Radi se u Hanlderu <see cref="MainWindow.DockPanelLoaded"/>.
        /// </summary>
        public void ResetAllUncheduledTermins()
        {
            foreach (var s in subjects.Values)
            {
                s.UnscheduledTermins = s.TerminNumber;
            }
            Console.WriteLine("ResetAllUncheduledTermins\n\t~ Resetovano {0} predmeta.", subjects.Count);
        }
    }
}
