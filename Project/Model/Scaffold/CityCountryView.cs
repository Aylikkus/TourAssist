using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class CityCountryView
{
    public int IdCity { get; set; }

    public string FullName { get; set; } = null!;

    public string CountryIso31661 { get; set; } = null!;

    public string RegionName { get; set; } = null!;
}
