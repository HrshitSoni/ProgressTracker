using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProgressTrackerLibrary.Models;
using ProgressTrackerLibrary.HelperMethods;
using ProgressTrackerLibrary.DatabasePopulator;
using System.Windows.Threading;


namespace ProgressTracker
{
    public partial class MainWindow : Window
    {
        // Flag for resizing the app list UI
        private bool isCollapsed = false;

        // List of Applications as Clickable Buttons
        private static ObservableCollection<Button> appList = new ObservableCollection<Button>();

        private TimeTracking timeTracking;

        private DispatcherTimer DatabaseUpdateTimer;

        private string appfileName = FileConnector.appFile;
        private string eachDayFileName = FileConnector.EachDayFile;

        private List<string> appNames;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = appList;

            ReadDatabase();
            RefreshAppList();
            AssignDayToUI();

            timeTracking = new TimeTracking();

            StartDatabaseUpdateTimer();
            FileConnector.RecordEachDay();

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            timeTracking.StopTracking();
        }

        // Timer for updating the database 
        public void StartDatabaseUpdateTimer()
        {
            DatabaseUpdateTimer = new DispatcherTimer();
            DatabaseUpdateTimer.Interval += TimeSpan.FromSeconds(2);
            DatabaseUpdateTimer.Tick += DatabaseUpdateTimer_Tick;
            DatabaseUpdateTimer.Start();
        }

        // update Database from focusTimes dictionary
        private void DatabaseUpdateTimer_Tick(object? sender, EventArgs e)
        {
            Dictionary<string,TimeSpan> focusTimes = timeTracking.focustimes;
            foreach(var nameTimePair in focusTimes)
            {
                foreach(AppModel app in eachDayFileName.ReadFile())
                {
                    if(app.appName == nameTimePair.Key)
                    {
                        // Update the active time of the app
                        TimeSpan currentTime = nameTimePair.Value;
                        app.activeTime = currentTime.ToString(@"hh\:mm\:ss");

                        // Update the time in the file
                        app.UpdateAppTime(eachDayFileName);

                        // Reset the time in focusTimes
                        timeTracking.focustimes[nameTimePair.Key] = TimeSpan.Zero;
                    }
                }
            }
        }

        // Update the UI Based on backend
        private void RefreshAppList()
        {
            AppList.ItemsSource = appList;
        }

        // Method to Resize the appList UI
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCollapsed == false)
            {
                ListViewColumn.Width = new GridLength(53);
                isCollapsed = true;
            }
            else
            {
                ListViewColumn.Width = new GridLength(200);
                isCollapsed = false;
            }
        }

        // Loading apps from file to the UI list && also load app in each day file
        private void ReadDatabase()
        {
            foreach(AppModel app in appfileName.ReadFile())
            {
                LoadAppInAppList(app);
            }

            FileConnector.UpdateEachDayFileBasedOnAppFile();
        }

        // Method for adding apps in the app list & Database
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Opening the app dialog box
            var app = HelpingMethods.OpenAppsDialogBox_AddApp();
            if (app == null) return;

            bool isInAppFile = app.PresentInFile(appfileName);
            bool isInDayFile = app.PresentInFile(eachDayFileName);

            if (isInAppFile && isInDayFile)
            {
                MessageBox.Show("App is already present in both files", "Info");
                return;
            }

            if (!isInAppFile)
            {
                app.SaveToAppFile(appfileName);
            }

            if (!isInDayFile)
            {
                app.SaveToAppFile(eachDayFileName);
            }

            LoadAppInAppList(app);
        }

        // Method to load the window
        private void LoadAppInAppList(AppModel app)
        {
            TextBlock AppNameTextBlock = new TextBlock
            {
                Height = 42,
                Width = 142,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.White),
                TextAlignment = TextAlignment.Center,
                Padding = new Thickness(8),
                Text = app.appName,
                Background = new SolidColorBrush(Colors.Transparent),
                ToolTip = app.appName,
            };

            System.Windows.Controls.Image appLogo = new()
            {
                Height = 42,
                Width = 50,
                Margin = new Thickness(0),
               
            };

            string filePath = app.appLogoPath;
            appLogo.Source = filePath.GetImage().ConvertBitmapToImageSource();
   

            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = new SolidColorBrush(Colors.Transparent),
            };


            panel.Children.Add(appLogo);
            panel.Children.Add(AppNameTextBlock);


            Button AppButton = new Button
            {
                Height = 50,
                Width = 191,
                Background = new SolidColorBrush(Colors.Transparent),
                Content = panel,
                BorderBrush = new SolidColorBrush(Colors.Transparent),
            };

            appList.Add(AppButton);
        }

        // Method to remove an Application from the app list & Database
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppList.SelectedItem != null)
            {
                var button = (Button)AppList.SelectedItem;
                var app = button.ExtractAppFromButton();

                // Remove the app from both files
                app.RemoveFromAppFile(appfileName);
                app.RemoveFromAppFile(eachDayFileName);

                // Remove the button from the UI list
                appList.Remove(button);
            }
            else
            {
                MessageBox.Show("Please select an app to remove. You can select an app by right-clicking on it.", "Error");
            }
        }

        // Method to show Active Time of the application
        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var app = button.ExtractAppFromButton();

            Dispatcher.BeginInvoke(new Action(()=>{
                AppList.SelectedItem = null;
            }));

            foreach(AppModel appModel in eachDayFileName.ReadFile())
            {
                if(app.appName == appModel.appName)
                {
                    TimeSpan time = TimeSpan.Zero;
                    TimeSpan.TryParse(appModel.activeTime,out time);

                    HourText.Text = time.Hours.ToString("D2");
                    MinutesText.Text = time.Minutes.ToString("D2");
                    SecondsText.Text = time.Seconds.ToString("D2");
                }
            }
        }

        // Putting todays day in Ui
        private void AssignDayToUI()
        {
            DayNameTextBox.Text = HelpingMethods.DayOfTheWeek();
        }
    }
}
