using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TourAssist.Model.Scaffold;

public partial class City
{
    public int IdCity { get; set; }

    public string FullName { get; set; } = null!;

    public int RegionIdRegion { get; set; }

    public virtual ICollection<PecularitiesCity> PecularitiesCities { get; set; } = new List<PecularitiesCity>();

    public virtual Region RegionIdRegionNavigation { get; set; } = null!;

    public virtual ICollection<Route> RouteFromIdCityNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteToIdCityNavigations { get; set; } = new List<Route>();

    public string AllPeculiarities
    {
        get
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var joins = new ObservableCollection<PecularitiesCity>(dbContext.PecularitiesCities
                    .Where(pc => pc.CityIdCity == IdCity));
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
}
