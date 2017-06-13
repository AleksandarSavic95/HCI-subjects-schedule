using MindFusion.Scheduling;
using MindFusion.Scheduling.Wpf;
using SubjectsSchedule.Schedules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SubjectsSchedule.Model
{
    //[ItemPresenter(typeof(CustomAppointmentPresenter))]
    class MyTermin : Appointment
    {
        public MyTermin()
        {
            // zabrana resize-a
            this.AllowChangeStart = false;
            this.AllowChangeEnd = false;

            this.PropertyChanged += TimeChangedDescriptionUpdate;

            // još neka inicijalizacija??
        }
        private void TimeChangedDescriptionUpdate(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Time"))
            {
                Console.WriteLine("vrijeme se promijenilo!");
                // TODO: pogledati formatiranje stringova:
                // http://blog.stevex.net/string-formatting-in-csharp/
                // https://stackoverflow.com/q/644017
                this.DescriptionText = string.Format("{0}\n{1} -- {2}",
                    DescriptionCentering(StartTime.DayOfWeek),
                    StartTime.ToString("HH:mm"), EndTime.ToString("HH:mm"));
            }
        }

        /// <summary>
        /// Prevod naziva dana na srpski jezik.
        /// </summary>
        private static CultureInfo culture = new CultureInfo("sr");

        /// <summary>
        /// Pravi novi termin od proslijeđenih informacija.
        /// Poziva <see cref="MyTermin()"/>!
        /// </summary>
        /// <param name="header">Naslov</param>
        /// <param name="description">Opis</param>
        /// <param name="start">početak</param>
        /// <param name="end">kraj</param>
        public MyTermin(string header, DateTime start, DateTime end) : this() // poziva se base konstruktor!
        {
            this.HeaderText = header;
            this.StartTime = start;
            this.EndTime = end;
            // TODO: pogledati formatiranje stringova...
            this.DescriptionText = string.Format("{0}\n{1} -- {2}", DescriptionCentering(start.DayOfWeek),
                    StartTime.ToString("HH:mm"), EndTime.ToString("HH:mm"));
        }
        /*
            |22:00 -- 22:00|  14 karaktera
            |  ponedeljak  | ponedeljak ima 2 razmaka
            |    utorak    | utorak, srEda, petak i subota imaju 4
            |   četvrtak   | četvrtak ima 3
        */
        private string DescriptionCentering(DayOfWeek dayOfWeek)
        {
            string dayOfWeekSerbian = culture.DateTimeFormat.GetDayName(dayOfWeek);
            int space = 0;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    space = 2;
                    break;
                case DayOfWeek.Tuesday: case DayOfWeek.Wednesday:
                case DayOfWeek.Friday: case DayOfWeek.Saturday:
                    space = 4;
                    break;
                case DayOfWeek.Thursday:
                    space = 3;
                    break;
                default:  // neće nikada biti nedelja
                    break;
            }
            // TODO: pogledati formatiranje stringova...
            // Font nije *monospace* pa mora veći left-padding..
            return "        ".Substring(0, space*2-2) + dayOfWeekSerbian;
        }

        public MyTermin(string header, DateTime start, DateTime end,
            Classroom classroom, Subject subject) : this(header, start, end)
        {
            this._inClassroom = classroom;
            this._forSubject = subject;
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


        /** podrska za poredjenje i rad sa kolekcijom (?) */
        public override bool Equals(object obj)
        {
            return this.Id.Equals(((MyTermin)obj).Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
