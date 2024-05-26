using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TourAssist.Model.Scaffold;

public partial class Region
{
    public int IdRegion { get; set; }

    public string FullName { get; set; } = null!;

    public string CountryIso31661 { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country CountryIso31661Navigation { get; set; } = null!;

    public virtual ICollection<PecularitiesRegion> PecularitiesRegions { get; set; } = new List<PecularitiesRegion>();

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
}
