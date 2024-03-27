using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Hotel
{
    public int IdHotel { get; set; }

    public string FullName { get; set; } = null!;

    public float? Rating { get; set; }

    public int CityIdCity { get; set; }

    public virtual City CityIdCityNavigation { get; set; } = null!;

    public virtual ICollection<Usertour> Usertours { get; set; } = new List<Usertour>();
}
