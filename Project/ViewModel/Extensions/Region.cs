using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourAssist.ViewModel.Utility;

namespace TourAssist.Model.Scaffold
{
    public partial class Region : INotifyPropertyChanged
    {
        [NotMapped]
        public string AllPeculiarities
        {
            get
            {
                using (TourismDbContext dbContext = new TourismDbContext())
                {
                    var joins = new ObservableCollection<PecularitiesRegion>(dbContext.PecularitiesRegions
                        .Where(pc => pc.RegionIdRegion == IdRegion));
                    var pecs = new ObservableCollection<Peculiarity>(dbContext.Peculiarities.ToList()
                        .Where(p => joins.Where(j => j.PeculiarityIdPeculiarity == p.IdPeculiarity).Count() > 0));
                    StringBuilder sb = new StringBuilder();
                    foreach (var p in pecs)
                    {
                        sb.Append(p.Description + ", ");
                    }
                    if (sb.Length > 1)
                        sb.Remove(sb.Length - 2, 2);
                    return sb.ToString();
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
