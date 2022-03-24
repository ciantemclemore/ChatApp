using ChatAppServiceLibrary.DataContracts;
using ChatAppWPFClient.Commands;
using ChatAppWPFClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatAppWPFClient.ViewModels
{
    public class CreatePublicRoomViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public RelayCommand CreatePublicRoomCommand { get; set; }
        public ChatManagerServiceClient TcpClient { get; set; }
        public ICommand NavigateChatAppControl { get; set; }
        public ChatAppViewModel ReturnToViewModel { get; set; }
        public Client LocalClient { get; set; }
        private string _roomName;
        public string RoomName
        {
            get => _roomName;
            set
            {
                _roomName = value;
                OnPropertyChanged();
            }
        }

        public CreatePublicRoomViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            CreatePublicRoomCommand = new RelayCommand(async (o) => await CreatePublicRoom(), (o) => !string.IsNullOrEmpty(RoomName));
        }

        private async Task CreatePublicRoom() 
        {
            ChatRoomRequest chatRoomRequest = new ChatRoomRequest()
            {
                Clients = new List<Client>() { LocalClient },
                DisplayName = RoomName,
                IsPublic = true,
                ServerName = RoomName
            };

            // let the chat app know the name of the room you created so you can select it
            ReturnToViewModel.CreatePublicRoomName = chatRoomRequest.DisplayName;

            if (await TcpClient.CreatePublicChatRoomAsync(chatRoomRequest))
            {
                NavigateChatAppControl = new NavigateCommand<ChatAppViewModel>(_navigationStore, () => ReturnToViewModel);
                NavigateChatAppControl.Execute(null);
            }
            else 
            {
                MessageBox.Show(Application.Current.MainWindow, "This room already exists, try again!");
                RoomName = null;
            }
        }
    }
}
