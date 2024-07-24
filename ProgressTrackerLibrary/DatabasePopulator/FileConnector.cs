using ProgressTrackerLibrary.Database;
using ProgressTrackerLibrary.Models;


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

        public static bool PresentInFile(AppModel app)
        {
            List<AppModel> file = ReadFile();

            foreach(AppModel presentApp in file)
            {
                if(app.appName == presentApp.appName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
