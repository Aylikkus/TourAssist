using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model
{
    public class Configuration
    {
        public static string MySQLServerIP { get; set; } = "127.0.0.1";
        public static string MySQLUser { get; set; } = "root";
        public static string MySQLPassword { get; set; } = "1234";
        public static Credentials? Credentials { get; set; }
    }
}
