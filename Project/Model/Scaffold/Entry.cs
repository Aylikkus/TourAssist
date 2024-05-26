using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Entry
{
    public int IdEntry { get; set; }

    public int UserIdUser { get; set; }

    public int RouteIdRoute { get; set; }

    public float? Rating { get; set; }

    public virtual Route RouteIdRouteNavigation { get; set; } = null!;

    public virtual User UserIdUserNavigation { get; set; } = null!;
}
