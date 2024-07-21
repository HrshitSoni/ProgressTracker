﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using ProgressTrackerLibrary.Models;
using ProgressTrackerLibrary.HelperMethods;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ProgressTracker
{
    public partial class MainWindow : Window
    {
        // Flag for resizing the app list UI
        private bool isCollapsed = false;

        // List of Applications as Clickable Buttons
        private static ObservableCollection<Button> appList = new ObservableCollection<Button>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = appList;

            RefreshAppList();
            AssignDayToUI();
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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Opening the app dialog box
            var app = HelpingMethods.OpenAppsDialogBox();
            
            // Adding the app to the Window/Ui
            LoadWindow(app);
        }
        
        // Method to load the window
        private void LoadWindow(AppModel app)
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
                Text = app.appName,
                Background = new SolidColorBrush(Colors.Transparent)
            };

            // Image to the application
            System.Windows.Controls.Image appLogo = new()
            {
                Height = 42,
                Width = 50,
                Margin = new Thickness(0),
               
            };

            // Extracting Image from .exe file
            string filePath = app.appLogoPath;
            var bitmap = filePath.GetImage();
            // MemoryStream is a class that uses unmanaged resources (memory), which need to be released when the stream is no longer needed.
            using (MemoryStream memory = new MemoryStream()) // This line creates a new MemoryStream object, which is a stream of bytes stored in memory. It is used here to temporarily store the image data.
            {
                bitmap.Save(memory, ImageFormat.Png); // Saving the image to the memory
                memory.Position = 0;// resets the memory position to 0 to get ready for the read
                BitmapImage bitmapImage = new BitmapImage(); // creating a new bitmap image
                bitmapImage.BeginInit(); // begin initialization 
                bitmapImage.StreamSource = memory; // set the stream source to memory stream
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // cache the image data on load
                bitmapImage.EndInit(); // End initialization
                appLogo.Source = bitmapImage; // setting the source to bitmap image;
            }

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

        // Method to remove an Application from the app list
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

        // Method to show Active Time of the application
        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This method is not implemented yet");
        }

        // Putting todays day in Ui
        private void AssignDayToUI()
        {
            DayOfWeek day = DateAndTime.Today.DayOfWeek;
            DayNameTextBox.Text = day.ToString();
        }
    }
}
