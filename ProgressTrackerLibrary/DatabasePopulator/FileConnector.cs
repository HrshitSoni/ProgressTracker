using ProgressTrackerLibrary.Database;
using ProgressTrackerLibrary.Models;
using System.Windows;


namespace ProgressTrackerLibrary.DatabasePopulator
{
    public static class FileConnector
    {
        private const string appFile = "AppFile.csv";

        public static List<AppModel> ReadFile()
        {
            return appFile.FullFilePath().LoadFile().ConvertToAppModel();
        }

        public static void SaveToAppFile(AppModel app)
        {
            List<AppModel> appList = ReadFile();

            appList.Add(app);

            appList.SaveAppListToFile(appFile);
        }

        public static bool PresentInFile(this AppModel app)
        {
            List<AppModel> appList = ReadFile();

            if(app != null)
            {
                foreach (AppModel presentApp in appList)
                {
                    if (app.appName == presentApp.appName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void RemoveFromAppFile(this AppModel appModel)
        {
            List<AppModel> appList = ReadFile();
            if(appModel!= null)
            {

                foreach (AppModel app in appList)
                {
                    if (app.appName == appModel.appName)
                    {
                        appList.Remove(app);
                        break;
                    }
                }
            }

            appList.SaveAppListToFile(appFile);
        }

        public static void UpdateAppTime(AppModel appModel)
        {
            List<AppModel> appList = ReadFile();
            var currApp = appList.FirstOrDefault(app => app.appName == appModel.appName);

            if (currApp != null)
            {
                TimeSpan currAppTime,appModelTime;
                TimeSpan.TryParse(currApp.activeTime,out currAppTime);
                TimeSpan.TryParse(appModel.activeTime, out appModelTime);
                currAppTime += appModelTime;
                currApp.activeTime = currAppTime.ToString(@"hh\:mm\:ss");
                if(currApp.DayOfTheWeek != DateTime.Now.DayOfWeek.ToString())
                {
                    currApp.DayOfTheWeek = DateTime.Now.DayOfWeek.ToString();
                }
                appList.SaveAppListToFile(appFile);
            }
        }
    }
}
