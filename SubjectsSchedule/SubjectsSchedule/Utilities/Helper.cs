using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsSchedule.Model;
using System.Windows.Controls;

namespace SubjectsSchedule.Utilities
{
    class Helper
    {

        public static int ParseStringToInt(string str)
        {
            return int.Parse(str == null ? "0" : str);
        }

        public static double ParseStringToDouble(string str)
        {
            return double.Parse(str == null ? "0.0" : str);
        }

        public static OS GetOSFromString(string os)
        {
            if (os.ToLower().StartsWith("win"))
                return OS.WINDOWS;
            else if (os.ToLower().StartsWith("lin"))
                return OS.LINUX;
            else if (os.ToLower().StartsWith("oba"))
                return OS.C_BOTH;
            else if (os.ToLower().StartsWith("cross"))
                return OS.SOFT_CROSS_PLATFORM;
            else
                return OS.SUBJ_WHATEVER;
        }

        public static bool CheckBoxToBool(CheckBox checkbox)
        {
            return checkbox.IsChecked ?? false;
        }
    }
}
