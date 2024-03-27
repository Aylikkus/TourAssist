using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class PecularitiesRegion
{
    public int RegionIdRegion { get; set; }

    public int PeculiarityIdPeculiarity { get; set; }

    public int IdRegionPecularity { get; set; }

    public virtual Peculiarity PeculiarityIdPeculiarityNavigation { get; set; } = null!;

    public virtual Region RegionIdRegionNavigation { get; set; } = null!;
}
