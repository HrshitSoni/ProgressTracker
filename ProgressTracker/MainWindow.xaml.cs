using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if(isCollapsed == false)
            {
                ListViewColumn.Width = new GridLength(57);
                isCollapsed = true;
            }
            else
            {
                ListViewColumn.Width = new GridLength(200);
                isCollapsed = false;
            }
        }
    }
}