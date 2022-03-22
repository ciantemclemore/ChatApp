using ChatAppWPFClient.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatAppWPFClient.Views
{
    /// <summary>
    /// Interaction logic for ChatAppControl.xaml
    /// </summary>
    public partial class ChatAppControl : UserControl
    {
        public ChatAppControl()
        {
            InitializeComponent();
            Loaded += async (s, e) => await ChatAppControl_Loaded(s, e);
        }

        private Task ChatAppControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += async (s, o) => await Window_Closing(s, o);
            return Task.CompletedTask;
        }

        private async Task Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ChatAppViewModel vm)
            {
                await vm.TcpClient.LogoutAsync(vm.LocalClient.Name);
                vm.TcpClient.Close();
            }
        }
    }
}
