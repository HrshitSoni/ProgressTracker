﻿using Microsoft.Win32;
using ProgressTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ProgressTrackerLibrary.HelperMethods
{
    public static class HelpingMethods
    {
        // Opening .exe files dialog box
        public static AppModel  OpenAppsDialogBox_AddApp()
        {
            var dialogBox = new OpenFileDialog
            {
               Filter = "Executable files (*.exe)|*.exe",
               Title = "Please select an .exe file",
               Multiselect = false,
               InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
            };

            if(dialogBox.ShowDialog() == true)
            {
                string appPath = dialogBox.FileName;
                string Name = Path.GetFileNameWithoutExtension(appPath);

                var app = new AppModel
                {
                    id = 1,
                    appName = Name,
                    appLogoPath = appPath,
                    activeTime = "00:00:00"
                };
                return app;
            }

            return null;
        }


        // Extracting Image from .exe file
        public static Bitmap GetImage(this string path)
        {
            Icon icon = Icon.ExtractAssociatedIcon(path);

            Bitmap image = icon.ToBitmap();

            return image;
        }
    }
}
