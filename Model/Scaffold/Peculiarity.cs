using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Peculiarity
{
    public int IdPeculiarity { get; set; }

    public string Decription { get; set; } = null!;

    public virtual ICollection<PecularitiesCountry> PecularitiesCountries { get; set; } = new List<PecularitiesCountry>();

    public virtual ICollection<PecularitiesRegion> PecularitiesRegions { get; set; } = new List<PecularitiesRegion>();
}
