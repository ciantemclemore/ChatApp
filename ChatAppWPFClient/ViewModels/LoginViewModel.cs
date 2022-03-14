using ChatAppServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatAppWPFClient
{
    public class LoginViewModel : IWindow, IChatManagerServiceCallback
    {
        public RelayCommand LoginCommand { get; set; }

        public string Username { get; set; }

        public Action Close { get; set; }

        public Action Open { get; set; }

        public event Action OnRequestClose;

        public LoginViewModel() 
        {
            LoginCommand = new RelayCommand(async (o) => await ConnectToServer(o), o => !string.IsNullOrEmpty(Username));
        }

        private async Task ConnectToServer(object parameter)
        {
            // First create a context based on our main view
            //MainWindow mainWindow = new MainWindow();
            InstanceContext context = new InstanceContext(this);

            ChatManagerServiceClient tcpClient = new ChatManagerServiceClient(context, "ChatManagerSvc");

            // Create a client to pass to the server
            Client client = new Client()
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                Name = Username,
                TitleId = null
            };

            if (await tcpClient.LoginAsync(client))
            {
                // we can now show our main window and close this one
                //if (mainWindow.DataContext is MainViewModel vm) 
                //{
                //    vm.LocalClient = client;
                //    OnRequestClose?.Invoke();
                //}
                Console.WriteLine("Successful Login");
                CloseWindow();
            }
            else 
            {
                MessageBox.Show("Username already exists, try again!");
            }

            Username = string.Empty;
        }

        public void CloseWindow() 
        {
            Close?.Invoke();
        }

        public void OpenWindow() 
        {
            Open?.Invoke();
        }

        public void ReceiveMessage(Message message)
        {
            //throw new NotImplementedException();
        }

        public void UpdateOnlineClients(Client[] clients)
        {
            //throw new NotImplementedException();
        }

        public void UpdateAvailableTitles(Title[] titles)
        {
           // throw new NotImplementedException();
        }
    }
}
