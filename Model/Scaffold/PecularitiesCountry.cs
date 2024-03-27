using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class PecularitiesCountry
{
    public string CountryIso31661 { get; set; } = null!;

    public int PeculiarityIdPeculiarity { get; set; }

    public int IdCountryPecularity { get; set; }

    public virtual Country CountryIso31661Navigation { get; set; } = null!;

    public virtual Peculiarity PeculiarityIdPeculiarityNavigation { get; set; } = null!;
}
