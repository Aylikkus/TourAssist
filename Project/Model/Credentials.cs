using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TourAssist.Model
{
    public struct Credentials
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
