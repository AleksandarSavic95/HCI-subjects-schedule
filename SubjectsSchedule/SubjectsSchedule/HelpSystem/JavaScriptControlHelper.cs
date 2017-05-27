using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace SubjectsSchedule
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        MainWindow prozor;
        public JavaScriptControlHelper(MainWindow w)
        {
            prozor = w;
        }

        /// <summary>
        /// Metoda se poziva iz Prezime.htm fajla, kao
        /// reakcija na klik na dugme "Press me".
        /// </summary>
        /// <param name="param">tekst koji postaje naslov prozora</param>
        public void RunFromJavascript(string param)
        {
            prozor.doThings(param);
        }
    }
}