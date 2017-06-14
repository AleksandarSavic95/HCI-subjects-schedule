using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class ClassroomHandler
    {
        private static ClassroomHandler instance = null;

        public static ClassroomHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClassroomHandler();
                return instance;
            }
        }

        private Dictionary<string, Classroom> classrooms;
        private Classroom selectedClassroom;

        public List<Classroom> Classrooms
        {
            get
            {
                return classrooms.Values.ToList();
            }
        }

        public Classroom SelectedClassroom
        {
            get
            {
                return selectedClassroom;
            }
            set
            {
                if (value == null)
                {
                    selectedClassroom = new Classroom();
                    // set defaults
                    selectedClassroom.Seats = 12;
                    selectedClassroom.Board = true; 
                }
                else
                    selectedClassroom = classrooms[value.Id];
            }
        }

        private ClassroomHandler()
        {
            classrooms = new Dictionary<string, Classroom>();
            SelectedClassroom = null; // set default value
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, classrooms);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                classrooms = ( Dictionary<string, Classroom> ) formatter.Deserialize(file);
            }

            foreach (var croomId in classrooms.Keys)
                TerminHandler.Instance.AddClassroom(croomId);
        }

        public Classroom Add(string id, string description, int seats, bool projector, bool board, bool smartBoard, OS operatingSystem)
        {
            Classroom toAdd = new Classroom(id, description, seats, projector, board, smartBoard, operatingSystem);
            classrooms.Add(id, toAdd);
            return toAdd;
        }

        public Classroom Add(string id, string description, int seats, bool projector, bool board, bool smartBoard, OS operatingSystem, MainWindow context)
        {
            Classroom added = Add(id, description, seats, projector, board, smartBoard, operatingSystem);
            context.NotifyAll("Classrooms");
            return added;
        }

        public void SetSelectedClassroom(Classroom classRoom, MainWindow context)
        {
            SelectedClassroom = classRoom;
            context.NotifyAll("SelectedClassroom");
        }

        public void SetSelectedClassroom(string id, MainWindow context)
        {
            SetSelectedClassroom(FindById(id), context);
        }

        public void Update(string id, string newId, string description, int seats, bool projector, bool board, bool smartBoard, OS operatingSystem)
        {
            if (id != newId)
            {
                Remove(id);
                Add(newId, description, seats, projector, board, smartBoard, operatingSystem);
            }
            else
                classrooms[id] = new Classroom(id, description, seats, projector, board, smartBoard, operatingSystem);
        }

        public void Update(string id, string newId, string description, int seats, bool projector, bool board, bool smartBoard, OS operatingSystem, MainWindow context)
        {
            Update(id, newId, description, seats, projector, board, smartBoard, operatingSystem);
            SetSelectedClassroom(newId, context);
            // may exist list change
            context.NotifyAll("Classrooms");
        }

        public bool Has(string id)
        {
            return classrooms.ContainsKey(id);
        }

        public Classroom FindById(string id)
        {
            return classrooms[id];
        }

        public void Remove(string id)
        {
            classrooms.Remove(id);
        }

        public void Remove(string id, MainWindow context)
        {
            Remove(id);
            context.NotifyAll("Classrooms");
        }
    }
}
