using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class RouteCitiesView
{
    public int IdRoute { get; set; }

    public int FromCityId { get; set; }

    public string FromCityName { get; set; } = null!;

    public int ToCityId { get; set; }

    public string ToCityName { get; set; } = null!;

    public DateTime Departure { get; set; }

    public DateTime Arrival { get; set; }

    public decimal Price { get; set; }

    public int TransportId { get; set; }

    public string TransportName { get; set; } = null!;
}
