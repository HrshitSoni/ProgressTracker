         using ProgressTrackerLibrary.HelperMethods;
using ProgressTrackerLibrary.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace ProgressTracker
{
    /// <summary>
    /// Interaction logic for TimePage.xaml
    /// </summary>
    public partial class TimePage : Page
    {
        private AppModel? appModel;

        public TimePage(AppModel? model)
        {
            InitializeComponent();

            appModel = model;
            AssignDayToUi();
            if (appModel != null)
            {
                UpdateTime(appModel);
            }
        }

        public TimePage()
        {
            InitializeComponent();
            AssignDayToUi();
        }

        private void GraphButton_click(object sender, RoutedEventArgs e)
        {
            ShowGraphPage(appModel);
        }

        public void UpdateTime(AppModel? appModel)
        {
            TimeSpan time = TimeSpan.Zero;
            if (appModel != null)
            {
                TimeSpan.TryParse(appModel.activeTime, out time);
            }
            HourText.Text = time.Hours.ToString("D2");
            MinutesText.Text = time.Minutes.ToString("D2");
            SecondsText.Text = time.Seconds.ToString("D2");
        }

        private void AssignDayToUi()
        {
            DayNameTextBox.Text = HelpingMethods.DayOfTheWeek();
        }

        public void ShowGraphPage(AppModel? appModel)
        {
            if (appModel != null)
            {
                GraphPage gp = new GraphPage(appModel);
                NavigationService.Navigate(gp);
            }
            else
            {
                MessageBox.Show("Please select an application of your choice", "Caution");
            }
        }
    }
}
