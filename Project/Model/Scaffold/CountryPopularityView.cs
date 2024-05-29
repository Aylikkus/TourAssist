using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model.Scaffold
{
    public partial class CountryPopularityView
    {
        public string Iso31661 { get; set; } = null!;
        public int ToCountryPopularity { get; set; }
    }
}
