using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAssist.Model
{
    public struct Credentials
    {
        public string Login { get; }
        public string Password { get; }

        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
