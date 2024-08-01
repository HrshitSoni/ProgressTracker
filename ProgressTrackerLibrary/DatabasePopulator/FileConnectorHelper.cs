using ProgressTrackerLibrary.Models;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace ProgressTrackerLibrary.Database
{
    public static class FileConnectorHelper
    {
        // These are extension methods 

        // Location of the file in the pc
        public static string FullFilePath(this string fileName)
        {
            string basePath = $"{ConfigurationManager.AppSettings["filePath"]}";
           // Trace.WriteLine(basePath);
            return Path.Combine(basePath, fileName);
        }

        // Loading the file if it exits otherwise create one
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            else
            {  
                return File.ReadAllLines(file).ToList();
            }
        }


        public static void SaveAppListToFile(this List<AppModel> apps, string fileName)
        {
            List<string> newLine = new List<string>();

            foreach(AppModel app in apps)
            {
                if(app != null)
                {
                    newLine.Add($"{app.appName},{app.appLogoPath},{app.activeTime},{app.DayOfTheWeek}");
                }
            }

            File.WriteAllLines(fileName.FullFilePath(), newLine);
        }

        public static List<AppModel> ConvertToAppModel(this List<string> file)
        {
            List<AppModel> apps = new List<AppModel>();
             
            foreach(string line in file)
            {
                var columns = line.Split(',');
                AppModel app = new AppModel
                {
                    appName = columns[0],
                    appLogoPath = columns[1],
                    activeTime = columns[2],
                    DayOfTheWeek = columns[3]
                };

                apps.Add(app);
            }

            return apps;
        }
    }
}
