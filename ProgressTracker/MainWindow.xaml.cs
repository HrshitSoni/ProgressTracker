using System.Windows;
using System.Windows.Controls;


namespace ProgressTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isCollapsed = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuButton_ClicK(object sender, RoutedEventArgs e)
        {
            if(isCollapsed == false)
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
            
        }
    }
}