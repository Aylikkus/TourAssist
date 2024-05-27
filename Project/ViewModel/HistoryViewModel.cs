using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model;
using TourAssist.Model.Scaffold;
using TourAssist.ViewModel.Utility;

namespace TourAssist.ViewModel
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Entry> userEntries;
        public ObservableCollection<Entry> UserEntries
        {
            get { return userEntries; }
            private set
            {
                userEntries = value;
                OnPropertyChanged(nameof(UserEntries));
            }
        }

        private RelayCommand? refresh;
        public RelayCommand Refresh
        {
            get
            {
                return refresh ??= new RelayCommand(obj =>
                {
                    User? user = AuthManager.CurrentUser;

                    if (user == null) return;

                    using (TourismDbContext dbContext = new TourismDbContext())
                    {
                        UserEntries = new ObservableCollection<Entry>(
                            dbContext.Entries.Where((e) => e.UserIdUser == user.IdUser));
                    }
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public HistoryViewModel()
        {
            userEntries = new ObservableCollection<Entry>();
        }
    }
}
