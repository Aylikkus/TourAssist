using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class UsertourAttraction
{
    public int IdUserTourAttraction { get; set; }

    public int UserTourIdUserTour { get; set; }

    public int AttractionIdAttraction { get; set; }

    public virtual Attraction AttractionIdAttractionNavigation { get; set; } = null!;

    public virtual Usertour UserTourIdUserTourNavigation { get; set; } = null!;
}
