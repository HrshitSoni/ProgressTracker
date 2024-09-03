         using ProgressTrackerLibrary.HelperMethods;
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
        private GraphPage gp;
        private bool isGraphPage = false;

        public TimePage()
        {
            InitializeComponent();

            AssignDayToUi();

            gp = new GraphPage(this);
        }

        private void GraphButton_click(object sender, RoutedEventArgs e)
        {
            ShowGraphPage();
        }

        public void UpdateTime(TimeSpan time)
        {
            HourText.Text = time.Hours.ToString("D2");
            MinutesText.Text = time.Minutes.ToString("D2");
            SecondsText.Text = time.Seconds.ToString("D2");
        }

        private void AssignDayToUi()
        {
            DayNameTextBox.Text = HelpingMethods.DayOfTheWeek();
        }

        private void ShowGraphPage()
        {
            NavigationService.Navigate(gp);
        }
    }
}
