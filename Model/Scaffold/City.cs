using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class City
{
    public int IdCity { get; set; }

    public string FullName { get; set; } = null!;

    public int RegionIdRegion { get; set; }

    public virtual ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();

    public virtual Region RegionIdRegionNavigation { get; set; } = null!;

    public virtual ICollection<Route> RouteFromIdCityNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteToIdCityNavigations { get; set; } = new List<Route>();
}
