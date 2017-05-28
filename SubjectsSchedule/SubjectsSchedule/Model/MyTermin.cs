using MindFusion.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SubjectsSchedule.Model
{
    class MyTermin : MindFusion.Scheduling.Appointment
    {
        public MyTermin()
        {
            // da li mi treba neka inicijalizacija??
        }

        /** Override the SaveTo and LoadFrom methods of the Appointment class
        * in order to serialize the custom property Kept. */
        public override void SaveTo(XmlElement element, XmlSerializationContext context)
        {
            base.SaveTo(element, context);
            context.WriteObject(_forSubject, "ForSubject", element);
            context.WriteObject(_inClassroom, "InClassroom", element);
        }

        public override void LoadFrom(XmlElement element, XmlSerializationContext context)
        {
            base.LoadFrom(element, context);
            _forSubject = (Subject) context.ReadObject("ForSubject", element);
            _inClassroom = (Classroom) context.ReadObject("InClassroom", element);
        }

        /// <summary>
		/// Nepotrebno? A Clone override that enables interactive item cloning.
		/// </summary>
		public override object Clone()
        {
            MyTermin clone = new MyTermin();

            // The following code replicates the code used in
            // the Appointment's Clone method
            clone.AllDayEvent = this.AllDayEvent;
            clone.DescriptionText = this.DescriptionText;
            clone.EndTime = this.EndTime;
            clone.HeaderText = this.HeaderText;
            clone.Location = this.Location;
            clone.Locked = this.Locked;
            clone.Priority = this.Priority;
            clone.Reminder = this.Reminder;
            //clone.SelectedStyle = this.SelectedStyle.Clone() as Style;
            clone.StartTime = this.StartTime;
            //clone.Style = this.Style.Clone() as Style;
            clone.Tag = this.Tag;
            clone.Task = this.Task;
            clone.Visible = this.Visible;

            foreach (Resource resource in this.Resources)
                clone.Resources.Add(resource);

            foreach (Contact contact in this.Contacts)
                clone.Contacts.Add(contact);

            // Now copy the custom fields
            clone.InClassroom = this.InClassroom;
            clone.ForSubject = this.ForSubject;

            return clone;
        }

        /// <summary>
        /// Učionica u kojoj se održava termin.
        /// </summary>
        public Classroom InClassroom
        {
            get { return _inClassroom; }
            set { _inClassroom = value; }
        }

        /// <summary>
        /// Predmet kome termin pripada.
        /// </summary>
        public Subject ForSubject
        {
            get { return _forSubject; }
            set { _forSubject = value; }
        }

        private Classroom _inClassroom;
        private Subject _forSubject;
    }
}
