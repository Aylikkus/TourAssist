using System;
using System.Collections.Generic;

namespace TourAssist.Model.Scaffold;

public partial class Route
{
    public int IdRoute { get; set; }

    public int FromIdCity { get; set; }

    public int ToIdCity { get; set; }

    public DateTime Departure { get; set; }

    public DateTime Arrival { get; set; }

    public decimal Price { get; set; }

    public int TransportIdTransport { get; set; }

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();

    public virtual City FromIdCityNavigation 
    {
        get
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                return dbContext.Cities.Where(c => c.IdCity == FromIdCity).First();
            }
        }
        set
        {

        }
    }

    public virtual City ToIdCityNavigation
    {
        get
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                return dbContext.Cities.Where(c => c.IdCity == ToIdCity).First();
            }
        }
        set
        {

        }
    }

    public virtual Transport TransportIdTransportNavigation { get; set; } = null!;
}
