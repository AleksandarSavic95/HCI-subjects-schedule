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

        public List<Classroom> Classrooms
        {
            get
            {
                return classrooms.Values.ToList();
            }
        }

        private ClassroomHandler()
        {
            classrooms = new Dictionary<string, Classroom>();
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
        }

        public void Add(string id, string description, int seats, bool projector, bool board, bool smartBoard, OS operatingSystem)
        {
            classrooms.Add(id, new Classroom(id, description, seats, projector, board, smartBoard, operatingSystem));
        }

        public bool Has(string id)
        {
            return true ? classrooms[id] != null : false;
        }

        public Classroom FindById(string id)
        {
            return classrooms[id];
        }

        public void Remove(string id)
        {
            classrooms.Remove(id);
        }
    }
}
