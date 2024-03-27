using System.ComponentModel;
using System.Runtime.CompilerServices;
using TourAssist.Model;
using TourAssist.View;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private string login = "guest";
        private string password = "1234";

        public string Login
        { 
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private RelayCommand? openRegistration;
        public RelayCommand OpenRegistration
        {
            get
            {
                return openRegistration ??= new RelayCommand(obj =>
                    {
                        var registrationScreen = new RegistrationScreen();
                        registrationScreen.Show();
                    });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AuthorizationViewModel()
        {
        }
    }
}
