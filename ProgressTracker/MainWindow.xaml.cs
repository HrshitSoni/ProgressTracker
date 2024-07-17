using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using ProgressTrackerLibrary.Models;

namespace ProgressTracker
{
    public partial class MainWindow : Window
    {
        private bool isCollapsed = false;
        private static ObservableCollection<Button> appList = new ObservableCollection<Button>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = appList;

            RefreshAppList();
            AssignDayToUI();
        }

        private void RefreshAppList()
        {
            AppList.ItemsSource = appList;
        }

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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Name of the application that is added 
            TextBlock AppNameTextBlock = new TextBlock
            {
                Height = 42,
                Width = 142,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 25,
                Foreground = new SolidColorBrush(Colors.White),
                TextAlignment = TextAlignment.Center,
                Text = "First App",
                Background = new SolidColorBrush(Colors.Transparent)
            };

            // Image to the application
            Image appLogo = new Image
            {
                Height = 42,
                Width = 50,
                Margin = new Thickness(2)
            };

            // Panel that holds both image and Name textBlock
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Background = new SolidColorBrush(Colors.Transparent),
            };

            panel.Children.Add(appLogo);
            panel.Children.Add(AppNameTextBlock);

            // Lastly its a button so user clicks where ever it will trigger 
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


        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppList.SelectedItem != null)
            {
                var button = (Button)AppList.SelectedItem;
                appList.Remove(button);
             
            }
            else
            {
                MessageBox.Show("Please select an item to remove. You can select item by right clicking on it","Error");
            }
        }

        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This method is not implemented yet");
        }

        private void AssignDayToUI()
        {
            DayOfWeek day = DateAndTime.Today.DayOfWeek;
            DayNameTextBox.Text = day.ToString();
        }
    }
}
