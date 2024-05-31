using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private RelayCommand? showAdminPanel;
        public RelayCommand ShowAdminPanel
        {
            get
            {
                return showAdminPanel ??= new RelayCommand(obj =>
                {
                    PopupService.OpenWindow(typeof(AdminScreen));
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

        public Visibility IsAdmin
        {
            get
            {
                Userrole? role = AuthManager.CurrentRole;

                return role?.Name == "admin" ? Visibility.Visible : Visibility.Collapsed;
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
