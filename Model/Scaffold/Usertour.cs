using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Usertour
{
    public int IdUserTour { get; set; }

    public float Rating { get; set; }

    public int ArrivalIdEntry { get; set; }

    public int DepartureIdEntry { get; set; }

    public int HotelIdHotel { get; set; }

    public virtual Entry ArrivalIdEntryNavigation { get; set; } = null!;

    public virtual Entry DepartureIdEntryNavigation { get; set; } = null!;

    public virtual Hotel HotelIdHotelNavigation { get; set; } = null!;

    public virtual ICollection<UsertourAttraction> UsertourAttractions { get; set; } = new List<UsertourAttraction>();
}
