using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Transport
{
    public int IdTransport { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
