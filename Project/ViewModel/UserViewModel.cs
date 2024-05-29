using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model;
using TourAssist.Model.Scaffold;
using TourAssist.View;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private RelayCommand? logOut;
        public RelayCommand LogOut
        {
            get
            {
                return logOut ??= new RelayCommand(obj =>
                {
                    AuthManager.LogOut();
                    PopupService.OpenWindow(typeof(LoginScreen));
                    WindowClose?.Invoke(this, new EventArgs());
                });
            }
        }

        public User? User
        {
            get
            {
                return AuthManager.CurrentUser;
            }
        }

        public string RoleName
        {
            get
            {
                Userrole? role = AuthManager.CurrentRole;

                if (role == null) return "";

                return role.Name == "admin" ? "Администратор" : "Пользователь";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event EventHandler? WindowClose;
    }
}
