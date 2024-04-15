using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class PecularitiesCity
{
    public int CityIdCity { get; set; }

    public int PeculiarityIdPeculiarity { get; set; }

    public int IdCityPecularity { get; set; }

    public virtual City CityIdCityNavigation { get; set; } = null!;

    public virtual Peculiarity PeculiarityIdPeculiarityNavigation { get; set; } = null!;
}
