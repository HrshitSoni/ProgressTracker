﻿using Microsoft.Win32;
using ProgressTrackerLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Controls;
using ProgressTrackerLibrary.DatabasePopulator;

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

                AppModel app = new AppModel
                {
                    appName = Name,
                    appLogoPath = appPath,
                    activeTime = "00:00:00",
                    DayOfTheWeek = DayOfTheWeek(),
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

        public static BitmapImage ConvertBitmapToImageSource(this Bitmap bitmap)
        {
            // Converting bitmap to bitmapImageSource
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public static AppModel ExtractAppFromButton(this Button button)
        {
            StackPanel panel = (StackPanel)button.Content;
            TextBlock textBlock = (TextBlock)panel.Children[1];

            string appName = textBlock.Text;

            var apps = FileConnector.ReadFile();
            foreach (AppModel app in apps)
            {
                if (app.appName == appName)
                {
                    return app;
                }
            }

            return null;
        }

        public static string DayOfTheWeek()
        {
            return DateTime.Today.DayOfWeek.ToString();
        }
    }
}
