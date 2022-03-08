using System.Windows;
using System.Windows.Input;

namespace ChatAppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows the user to drag the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                DragMove();
            }
        }

        /// <summary>
        /// Allows the user to minimize the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) 
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                MainContent.Margin = new Thickness(8);
            }
            else 
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                MainContent.Margin = new Thickness(0);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
           // Disconnect user from the service

            // Close the application
            Application.Current.Shutdown();
        }
    }
}
