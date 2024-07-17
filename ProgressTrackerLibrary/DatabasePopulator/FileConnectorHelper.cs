using ProgressTrackerLibrary.Models;
using System.Configuration;
using System.IO;
using System.Windows.Media;

namespace ProgressTrackerLibrary.Database
{
    public static class FileConnectorHelper
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

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

        private static void SaveToAppFile(this List<AppModel> apps, string fileName)
        {
            List<string> newLine = new List<string>();

            foreach(AppModel app in apps)
            {
                newLine.Add($"{app.id}, {app.appName}, {app.appLogoPath}, {app.activeTime}");
            }

            File.WriteAllLines(fileName.FullFilePath(), newLine);
        }

        public static List<AppModel> LoadApplication(this List<string> file)
        {
            List<AppModel> apps = new List<AppModel>();
             
            foreach(string line in file)
            {
                var columns = line.Split(',');
                AppModel app = new AppModel
                {
                    id = int.Parse(columns[0]),
                    appName = columns[1],
                    appLogoPath = columns[2],
                    activeTime = TimeSpan.Parse(columns[3])
                };

                apps.Add(app);
            }

            return apps;
        }
    }
}
