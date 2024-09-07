using ProgressTrackerLibrary.DatabasePopulator;
using ProgressTrackerLibrary.HelperMethods;
using ProgressTrackerLibrary.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ProgressTracker
{
    /// <summary>
    /// Interaction logic for GraphPage.xaml
    /// </summary>
    public partial class GraphPage : Page
    {
        private AppModel appModel;
        public GraphPage(AppModel model)
        {
            InitializeComponent();

            appModel = model;

            MakeGraph(appModel);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
        }

        public void MakeGraph(AppModel appModel)
        {
            Dictionary<string, TimeSpan> appTimeData = appModel.GetAppTimeData();
            const double maxMinutes = 24 * 60;
            foreach (var entry in appTimeData)
            {
                Debug.WriteLine($"{entry.Key}: {entry.Value}");
            }
            foreach (var fileName in FileConnector.fileNamesList)
            {
                string dayName = fileName.DayOfTheWeekFromFileName();

                var progressBar = FindName($"ProgressBar_{dayName}") as ProgressBar;

                if (progressBar != null)
                {
                    if (appTimeData.ContainsKey(dayName))
                    {
                        var timeSpan = appTimeData[dayName];
                        Debug.WriteLine($"Retrieved TimeSpan for {dayName}: {timeSpan}");
                        double percentage = (timeSpan.TotalMinutes / maxMinutes) * 100;
                        progressBar.Value = percentage;
                        Debug.WriteLine($"Calculated Percentage for {dayName}: {percentage}");
                    }
                    else
                    {
                        progressBar.Value = 0;
                    }
                }
            }  
        }
    }
}
