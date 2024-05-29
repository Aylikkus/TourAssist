using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.ViewModel.Utility;

namespace TourAssist.Model.Scaffold
{
    public partial class RouteCitiesView : INotifyPropertyChanged
    {
        private RelayCommand? addToHistory;
        public RelayCommand AddToHistory
        {
            get
            {
                return addToHistory ??= new RelayCommand(obj =>
                {
                    using (TourismDbContext dbContext = new TourismDbContext())
                    {
                        User? user = AuthManager.CurrentUser;

                        if (user == null) return;

                        Entry entry = new Entry();
                        entry.UserIdUser = user.IdUser;
                        entry.RouteIdRoute = IdRoute;

                        if (dbContext.Entries.Where((e) => e.UserIdUser == user.IdUser
                            && e.RouteIdRoute == IdRoute).FirstOrDefault() == null)
                        {
                            dbContext.Entries.Add(entry);
                            dbContext.SaveChanges();
                        }
                    }
                });
            }
        }

        [NotMapped]
        public List<Peculiarity> AllPeculiarities
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    var joins = new List<PecularitiesCity>(dbContext.PecularitiesCities
                        .Where(pc => pc.CityIdCity == ToCityId));
                    var pecs = new List<Peculiarity>(dbContext.Peculiarities.ToList()
                        .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));

                    return pecs;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
