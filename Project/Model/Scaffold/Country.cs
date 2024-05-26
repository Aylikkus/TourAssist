using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TourAssist.Model.Scaffold;

public partial class Country
{
    public string Iso31661 { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public virtual ICollection<PecularitiesCountry> PecularitiesCountries { get; set; } = new List<PecularitiesCountry>();

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    public string AllPeculiarities
    {
        get
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var joins = new ObservableCollection<PecularitiesCountry>(dbContext.PecularitiesCountries
                    .Where(pc => pc.CountryIso31661 == Iso31661));
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
