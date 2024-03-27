using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Country
{
    public string Iso31661 { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public virtual ICollection<PecularitiesCountry> PecularitiesCountries { get; set; } = new List<PecularitiesCountry>();

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
}
