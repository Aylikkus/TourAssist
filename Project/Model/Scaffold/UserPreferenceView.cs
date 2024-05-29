using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model.Scaffold
{
    public partial class UserPreferenceView
    {
        public int IdPeculiarity { get; set; }
        public int IdUser { get; set; }
        public double? TotalPreference { get; set; }
    }
}
