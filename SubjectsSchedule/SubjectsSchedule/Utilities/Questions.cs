using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsSchedule.Utilities
{
    public static class Questions
    {
        public static System.Windows.MessageBoxResult BrisanjeTermina(string additionalInfo = "")
        {
            /// Prevod teksta na dugmadima <see cref="MainWindow()"/>
            System.Windows.Forms.MessageBoxManager.Yes = "Da";
            System.Windows.Forms.MessageBoxManager.No = "Ne";

            return System.Windows.MessageBox
                .Show(additionalInfo + "Da li želite da obrišete termin(e) koji već postoje?",
                "Zauzet termin", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);
        }
    }
}
