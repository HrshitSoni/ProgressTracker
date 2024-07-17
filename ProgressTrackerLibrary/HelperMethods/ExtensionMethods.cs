using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressTrackerLibrary.HelperMethods
{
    public static class ExtensionMethods
    {
        public static void  OpenAppsDialogBox()
        {
            var dialogBox = new OpenFileDialog
            {
               Filter = "Executable files (*.exe)|*.exe",
               Title = "Please select an .exe file",
               Multiselect = false
            };

            bool? result = dialogBox.ShowDialog();

           
        }
    }
}
