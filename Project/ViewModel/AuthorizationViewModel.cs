using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TourAssist.Model;
using TourAssist.Model.Scaffold;
using TourAssist.View;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private string login = null!;
        private string password = null!;
        private string name = null!;
        private string surname = null!;
        private bool rememberMe = false;
        private DateOnly dateOfBirth = DateOnly.FromDateTime(DateTime.Now).AddYears(-18);

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

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public bool RememberMe
        {
            get { return rememberMe; }
            set
            {
                rememberMe = value;
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        public DateOnly DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(DateTimeOfBirth));
            }
        }

        public DateTime DateTimeOfBirth
        {
            get { return dateOfBirth.ToDateTime(new TimeOnly()); }
            set
            {
                dateOfBirth = DateOnly.FromDateTime(value);
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(DateTimeOfBirth));
            }
        }

        private RelayCommand? openRegistration;
        public RelayCommand OpenRegistration
        {
            get
            {
                return openRegistration ??= new RelayCommand(obj =>
                    {
                        PopupService.OpenWindow(typeof(RegistrationScreen));
                    });
            }
        }

        private RelayCommand? register;
        public RelayCommand Register
        {
            get
            {
                return register ??= new RelayCommand(obj =>
                {
                    if (AuthManager.IsValidCredentials(Login, Password) == false)
                    {
                        PopupService.ShowMessage("Логин и пароль должны содержать минимум 4 символа.");
                        return;
                    }

                    if (AuthManager.Register(Login, Password, Name, Surname, DateOfBirth))
                    {
                        PopupService.ShowMessage("Регистрация успешно пройдена.");
                    }
                    else
                    {
                        PopupService.ShowMessage("Такой логин уже существует или вам нет 18-и лет.");
                    }
                });
            }
        }

        private RelayCommand? tryLogIntoApp;
        public RelayCommand TryLogIntoApp
        {
            get
            {
                return tryLogIntoApp ??= new RelayCommand(obj =>
                {
                    if (Login == null || Password == null)
                    {
                        PopupService.ShowMessage("Введите логин и пароль.");
                        return;
                    }

                    if (AuthManager.Authorize(Login, Password, RememberMe) == false)
                    {
                        PopupService.ShowMessage("Не удалось войти. Указан неверный логин/пароль.");
                        return;
                    }

                    if (RememberMe) Configuration.Save();

                    logWhenAuthorized();
                });
            }
        }

        private void logWhenAuthorized()
        {
            var user = AuthManager.CurrentUser;
            var role = AuthManager.CurrentRole;

            if (user == null || role == null)
                return;

            var userScreen = new UserScreen();
            userScreen.Show();
            foreach (Window w in Application.Current.Windows)
                if (w is not UserScreen) w.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AuthorizationViewModel()
        {
            if (AuthManager.TryAuthorizeFromCredentials())
            {
                logWhenAuthorized();
            }
        }
    }
}
