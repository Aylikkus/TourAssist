using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Region
{
    public int IdRegion { get; set; }

    public string FullName { get; set; } = null!;

    public string CountryIso31661 { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country CountryIso31661Navigation { get; set; } = null!;

    public virtual ICollection<PecularitiesRegion> PecularitiesRegions { get; set; } = new List<PecularitiesRegion>();
}
