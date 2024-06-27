using System.Configuration;
using System.IO;

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
    }
}
