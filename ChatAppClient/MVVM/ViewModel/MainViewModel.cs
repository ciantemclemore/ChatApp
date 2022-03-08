using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppClient.MVVM.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        /* Commands */
        public RelayCommand SendCommand { get; set; }


        //private User _user;
        //public User SelectedUser 
        //{
        //    get { return _user; }
        //    set 
        //    {
        //        _user = value;
        //        OnPropertyChanged();
        //    }
        //}

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public MainViewModel() 
        {
            // Add our messages into here
            SendCommand = new RelayCommand(rc => { });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
