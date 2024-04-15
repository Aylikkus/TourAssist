using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Peculiarity
{
    public int IdPeculiarity { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<PecularitiesCity> PecularitiesCities { get; set; } = new List<PecularitiesCity>();

    public virtual ICollection<PecularitiesCountry> PecularitiesCountries { get; set; } = new List<PecularitiesCountry>();

    public virtual ICollection<PecularitiesRegion> PecularitiesRegions { get; set; } = new List<PecularitiesRegion>();
}
