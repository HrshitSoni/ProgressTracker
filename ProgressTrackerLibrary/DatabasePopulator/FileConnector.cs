using ProgressTrackerLibrary.Database;
using ProgressTrackerLibrary.Models;


namespace ProgressTrackerLibrary.DatabasePopulator
{
    public static class FileConnector
    {
        private const string appFile = "AppFile.csv";
        
        public static void SaveToAppFile(AppModel app)
        {
            List<AppModel> appList = appFile.FullFilePath().LoadFile().ConvertToAppModel();

            appList.Add(app);

            appList.SaveAppListToFile(appFile);
        }
    }
}
