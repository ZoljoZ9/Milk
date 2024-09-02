using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Milk.Models;
using SQLite;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Milk.ViewModels
{
    public class PersonalizeViewModel : INotifyPropertyChanged
    {
        private GroceryUsers _user;
        private SQLiteAsyncConnection _connection;

        public PersonalizeViewModel()
        {
            _user = App.LoggedInUser;
            System.Diagnostics.Debug.WriteLine($"ViewModel User: {_user?.username}");

            if (_user != null)
            {
                Username = _user.username;
                Email = _user.email;
                Password = _user.password;
            }
        }


        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public string Email
        {
            get { return _user.email; }
            set
            {
                _user.email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _user.password; }
            set
            {
                if (_user.password != value)
                {
                    _user.password = value;
                    OnPropertyChanged();
                }
            }
        }

        // Add properties for the rest of the user data here

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
