using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Model
{
    /// <summary>
    /// Svaki smer je opisan preko:
    /// jedinstvene, ljudski-čitljive oznake smera,
    /// naziva smera,
    /// datuma uvođenja smera, i
    /// opisa smera
    /// </summary>
    [Serializable()]
    public class FieldOfStudy : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #region private fields

        private string _id;
        private string _name;
        /// <summary>
        /// Datum uvođenja smjera
        /// </summary>
        private DateTime _since;
        private string _description;
        
        #endregion

        #region Public properties, all of which implement OnPropChange

        public string Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        
        public DateTime Since
        {
            get { return _since; }
            set
            {
                if (value != _since)
                {
                    _since = value;
                    OnPropertyChanged("Since");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        
        #endregion

        public FieldOfStudy() { }

        public FieldOfStudy(string id, string name,
            DateTime since, string description)
        {
            this._id = id;
            this._name = name;
            this._since = since;
            this._description = description;
        }

        public override string ToString()
        {
            return String.Format("Field of study <<{0}>> {1}", _id, _name);
        }
    }
}
