using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatAppWPFClient.ViewModels
{
    public class LoginViewModel : ViewModelBase, IWindow
    {
        public RelayCommand LoginCommand { get; set; }

        public ICommand NavigateChatAppControl { get; set; }

        private string _userName;
        public string Username 
        {
            get => _userName;
            set 
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public Action Close { get; set; }

        public Action Open { get; set; }

        private readonly NavigationStore _navigationStore;

        public LoginViewModel(NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;
            LoginCommand = new RelayCommand(async (o) => await ConnectToServer(), o => !string.IsNullOrEmpty(Username));
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private async Task ConnectToServer()
        {
            ChatAppViewModel chatAppViewModel = new ChatAppViewModel(_navigationStore);
            InstanceContext context = new InstanceContext(chatAppViewModel);

            //create a tcp client that will have an open channel with the chat app view model
           ChatManagerServiceClient tcpClient = new ChatManagerServiceClient(context, "ChatManagerSvc");

            //Create a client to pass to the server
            Client client = new Client()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                Name = Username
            };

            // assign the current client to the view model
            chatAppViewModel.LocalClient = client;
            chatAppViewModel.TcpClient = tcpClient;

            if (await tcpClient.LoginAsync(client))
            {
                NavigateChatAppControl = new NavigateCommand<ChatAppViewModel>(_navigationStore, () => chatAppViewModel);
                NavigateChatAppControl.Execute(null);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow,"Username already exists, try again!");
                chatAppViewModel.LocalClient = null;
                Username = string.Empty;
            }
        }

        public void CloseWindow() 
        {
            Close?.Invoke();
        }

        public void OpenWindow() 
        {
            Open?.Invoke();
        }
    }
}
