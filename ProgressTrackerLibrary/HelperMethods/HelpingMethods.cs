using Microsoft.Win32;
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
using System.Globalization;
using System.Windows.Media;
using System.Data;
using ProgressTrackerLibrary.Database;

namespace ProgressTrackerLibrary.HelperMethods
{
    public static class HelpingMethods
    {
        // Opening .exe files dialog box with folder path being the program files
        public static AppModel? OpenAppsDialogBox_AddApp()
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

        // Extracting Icon of app from .exe file
        public static Bitmap? GetImage(this string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            Icon? icon = Icon.ExtractAssociatedIcon(path);
            Bitmap? image = null;
            if (icon is not null)
            {
                image = icon.ToBitmap(); 
            }

            return image;
        }

        // Method to convert the bitmap to image source
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

        // Method to extract app from the button in the app List
        public static AppModel? ExtractAppFromButton(this Button button)
        {
            StackPanel panel = (StackPanel)button.Content;
            TextBlock textBlock = (TextBlock)panel.Children[1];

            string appName = textBlock.Text;

            var apps = FileConnector.appFile.ReadFile();
            foreach (AppModel app in apps)
            {
                if (app.appName == appName)
                {
                    return app;
                }
            }

            return null;
        }

        // Method for assigning the day of the week 
        public static string DayOfTheWeek()
        {
            return DateTime.Today.DayOfWeek.ToString();
        }

        public static List<string> SortedListOfDates(this List<DateDayModel> dateDayModels)
        {
            List<DateTime> dateTimes = new List<DateTime>();

            foreach (var dateDay in dateDayModels)
            {
                string date = dateDay.date;
                DateTime actualDate = DateTime.ParseExact(date, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                dateTimes.Add(actualDate);
            }

            return dateTimes.OrderBy(date => date).Select(date => date.ToString("dd MMMM yyyy")).ToList();
        }

        public static void ColorTheClickedButtonDifferent(this Button button,Queue<Button>q)
        {
            if (q.Count != 0)
            {
                Button prevButton = q.Peek();
                prevButton.Background = new SolidColorBrush(Colors.Transparent);
                q.Dequeue();
                q.Enqueue(button);
            }
            else
            {
                q.Enqueue(button);
            }

            Button currButton = q.Peek();
            currButton.Background = new SolidColorBrush(Colors.Black);
        }

        public static Dictionary<string, TimeSpan> GetAppTimeData(this AppModel appModel)
        {
            var appTimeData = new Dictionary<string, TimeSpan>();

            foreach (var fileName in FileConnector.fileNamesList)
            {
                string filePath = fileName.FullFilePath();
                if (File.Exists(filePath))
                {
                    foreach (var app in fileName.ReadFile())
                    {
                        
                        if (appModel.appName == app?.appName && !string.IsNullOrEmpty(app?.DayOfTheWeek) && !string.IsNullOrEmpty(app?.activeTime))
                        {
                            
                            if (TimeSpan.TryParse(app.activeTime, out TimeSpan appTime))
                            {
                                if (appTimeData.ContainsKey(app.DayOfTheWeek))
                                {
                                    TimeSpan currentTime = appTimeData[app.DayOfTheWeek];
                                    appTimeData[app.DayOfTheWeek] = currentTime + appTime;
                                }
                                else
                                {
                                    appTimeData[app.DayOfTheWeek] = appTime;
                                }
                            }
                        }
                    }
                }
            }

            return appTimeData;
        }


        public static string DayOfTheWeekFromFileName(this string fileName)
        {
            string dayName = Path.GetFileNameWithoutExtension(fileName).Split('_')[0];

            return dayName;
        }

    }
}
