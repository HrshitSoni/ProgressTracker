using ProgressTrackerLibrary.Database;
using ProgressTrackerLibrary.HelperMethods;
using ProgressTrackerLibrary.Models;
using System.Windows;


namespace ProgressTrackerLibrary.DatabasePopulator
{
    public static class FileConnector
    {
        public const string appFile = "AppFile.csv";
        public const string dayRecord = "dayRecord.csv";
        public static string EachDayFile = $"{HelpingMethods.DayOfTheWeek()}_AppFile.csv";

        // Method for loading the file in form of list<appModel>
        public static List<AppModel> ReadFile(this string fileName)
        {
            return fileName.FullFilePath().LoadFile().ConvertToAppModel();
        }

        // TODO - Save app to both the file
        // Method for saving the app to the file
        public static void SaveToAppFile(AppModel app)
        {
            List<AppModel> appList = appFile.ReadFile();

            appList.Add(app);

            appList.SaveAppListToFile(appFile);
        }

        //TODO - Check if the app is present in both the file
        // Method for checking app in file to remove repetition of apps
        public static bool PresentInFile(this AppModel app)
        {
            List<AppModel> appList = appFile.ReadFile();

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

        // TODO - Need to remove app from both EachDayFile and app file
        // Method for removing app from the file
        public static void RemoveFromAppFile(this AppModel appModel)
        {
            List<AppModel> appList = appFile.ReadFile();
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

        //TODO - Update the time in the each day file
        // Method to update time in file of each day
        public static void UpdateAppTime(AppModel appModel)
        {
            List<AppModel> appList = EachDayFile.ReadFile();
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
