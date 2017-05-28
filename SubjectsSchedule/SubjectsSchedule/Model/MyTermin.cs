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
            _kept = true;
        }

        /// <summary>
        /// Indicate wether an appointment is kept or cancelled.
        /// </summary>
        public bool Kept
        {
            get { return _kept; }
            set { _kept = value; }
        }

        private bool _kept;

        /** Override the SaveTo and LoadFrom methods of the Appointment class
        * in order to serialize the custom property Kept. */
        public override void SaveTo(XmlElement element, XmlSerializationContext context)
        {
            base.SaveTo(element, context);
            context.WriteBool(_kept, "Kept", element);
        }

        public override void LoadFrom(XmlElement element, XmlSerializationContext context)
        {
            base.LoadFrom(element, context);
            _kept = context.ReadBool("Kept", element);
        }

    }
}
