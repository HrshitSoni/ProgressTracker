using ProgressTrackerLibrary.Database;
using ProgressTrackerLibrary.HelperMethods;
using ProgressTrackerLibrary.Models;
using System.Globalization;
using System.IO;
using System.Windows;


namespace ProgressTrackerLibrary.DatabasePopulator
{
    public static class FileConnector
    {
        public const string appFile = "AppFile.csv";
        public const string dayRecord = "dayRecord.csv";
        public static string EachDayFile = $"{HelpingMethods.DayOfTheWeek()}_AppFile.csv";
        public static List<string> fileNamesList = new()
        {
                "Sunday_AppFile.csv",
                "Monday_AppFile.csv",
                "Tuesday_AppFile.csv",
                "Wednesday_AppFile.csv",
                "Thursday_AppFile.csv",
                "Friday_AppFile.csv",
                "Saturday_AppFile.csv"
        };

        // Method for loading the file in form of list<appModel>
        public static List<AppModel> ReadFile(this string fileName)
        {
            return fileName.FullFilePath().LoadFile().ConvertToAppModel();
        }

        // Method for saving the app to the file
        public static void SaveToAppFile(this AppModel app, string fileName)
        {
            List<AppModel> appList = fileName.ReadFile();

            appList.Add(app);

            appList.SaveAppListToFile(fileName);
        }

        // Method for checking app in file to remove repetition of apps
        public static bool PresentInFile(this AppModel app,string fileName)
        {
            List<AppModel> appList = fileName.ReadFile();

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

        // Method for removing app from the file
        public static void RemoveFromAppFile(this AppModel appModel,string fileName)
        {
            List<AppModel> appList = fileName.ReadFile();

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

            appList.SaveAppListToFile(fileName);
        }

        // Method to update time in file of each day
        public static void UpdateAppTime(this AppModel appModel,string fileName)
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
                appList.SaveAppListToFile(fileName);
            }
        }

        // Function for keeping record of 7 days 
        public static void RecordEachDay()
        {
            string dateToday = DateTime.Today.Date.ToString("dd MMMM yyyy");
            string dayToday = HelpingMethods.DayOfTheWeek();
            bool exists = false;

            List<DateDayModel> dateDayModels = dayRecord.FullFilePath().LoadFile().ConvertToDateDayModel();

            if (dateDayModels.Count > 0)
            {
                string earliestDateString = dateDayModels.SortedListOfDates().First();
                DateTime earliestDate = DateTime.ParseExact(earliestDateString, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                DateTime todayDate = DateTime.ParseExact(dateToday, "dd MMMM yyyy", CultureInfo.InvariantCulture);

                int differenceInDates = Math.Abs((todayDate - earliestDate).Days);

                if (differenceInDates >= 7)
                {
                    dateDayModels.Clear();

                    DeleteAllDayFileAfter7Days();
                }
            }

            foreach (var entry in dateDayModels)
            {
                if (entry.day == dayToday && entry.date == dateToday)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                DateDayModel model = new()
                {
                    day = dayToday,
                    date = dateToday
                };

                dateDayModels.Add(model);
            }

            dateDayModels.SaveDayListToFile(dayRecord);
        }

        public static void UpdateEachDayFileBasedOnAppFile()
        {
            var eachDayFile = EachDayFile.ReadFile();
            foreach(var app in appFile.ReadFile())
            {
                if (!app.PresentInFile(EachDayFile))
                {
                    app.DayOfTheWeek = HelpingMethods.DayOfTheWeek();
                    eachDayFile.Add(app);
                }
            }
            eachDayFile.SaveAppListToFile(EachDayFile);
        }

        public static void DeleteAllDayFileAfter7Days()
        {
            foreach(string filename in fileNamesList)
            {
                string fullpath = filename.FullFilePath();
                if (File.Exists(fullpath))
                {
                    File.Delete(fullpath);
                }
            }
        }
    }
}
