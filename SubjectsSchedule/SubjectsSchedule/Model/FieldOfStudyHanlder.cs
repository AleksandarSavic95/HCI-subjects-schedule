using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    class FieldOfStudyHanlder
    {
        private static FieldOfStudyHanlder instance;

        public List<FieldOfStudy> fieldsOfStudy;

        public static FieldOfStudyHanlder Instance
        {
            get
            {
                if (instance == null)
                    instance = new FieldOfStudyHanlder();
                return instance;
            }
        }

        private FieldOfStudyHanlder()
        {
            fieldsOfStudy = new List<FieldOfStudy>();
        }

        public void Serialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, fieldsOfStudy);
            }
        }

        public void Deserialize(string fileName)
        {
            using (Stream file = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                fieldsOfStudy = ( List<FieldOfStudy> ) formatter.Deserialize(file);
            }
        }
    }
}
