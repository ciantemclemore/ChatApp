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

        public ICommand NavigateChatAppControl { get; }

        public string Username { get; set; }

        public Action Close { get; set; }

        public Action Open { get; set; }

        public LoginViewModel(NavigationStore navigationStore) 
        {
            NavigateChatAppControl = new NavigateCommand<ChatAppViewModel>(navigationStore, () => new ChatAppViewModel(navigationStore));
            //LoginCommand = new RelayCommand(async (o) => await ConnectToServer(o), o => !string.IsNullOrEmpty(Username));
        }

        private async Task ConnectToServer(object parameter)
        {
            // Create a context based on the main view model
            //MainViewModel mainViewModel = new MainViewModel();
            //InstanceContext context = new InstanceContext(mainViewModel);
            
            // create a tcp client that will have an open channel with the main view model
            //ChatManagerServiceClient tcpClient = new ChatManagerServiceClient(context, "ChatManagerSvc");

            // Create a client to pass to the server
            //Client client = new Client()
            //{
            //    Id = Guid.NewGuid(),
            //    CreatedOn = DateTime.Now,
            //    Name = Username,
            //    TitleId = null
            //};

            //if (await tcpClient.LoginAsync(client))
            //{
            //    // now we can pass our view model as a context to the view
            //    mainViewModel.LocalClient = client;
            //    MainWindow mainWindow = new MainWindow(mainViewModel);
            //    mainViewModel.OpenWindow();
            //    //CloseWindow();
            //}
            //else 
            //{
            //    MessageBox.Show("Username already exists, try again!");
            //}

            //Username = string.Empty;
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
