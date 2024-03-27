using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TourAssist.Model.Scaffold;

namespace TourAssist.Model
{
    public static class AuthManager
    {
        public static User? CurrentUser { get; private set; }

        public static bool Authorize(string login, string password, bool saveCredentials = false)
        {
            Configuration configuration = Configuration.GetConfiguration();
            string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var user = dbContext.Users.Where(u => u.Login == login).FirstOrDefault();

                if (user == null || user.PasswordSha256 != hash)
                {
                    return false;
                }

                CurrentUser = user;

                if (saveCredentials)
                {
                    configuration.Credentials = new Credentials(login, password);
                }

                return true;
            }
        }

        public static bool TryAuthorizeFromCredentials() 
        {
            Configuration configuration = Configuration.GetConfiguration();
            if (configuration.Credentials == null) return false;

            Credentials credentials = configuration.Credentials.Value;

            return Authorize(credentials.Login, credentials.Password);
        }

        public static bool Register(string login, string password)
        {
            string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            using (TourismDbContext dbContext = new TourismDbContext())
            {
                if (dbContext.Users.FirstOrDefault((u) => u.Login == login) != null)
                    return false;

                Userrole? guestRole = dbContext.Userroles.FirstOrDefault((r) => r.Name == "guest");

                var newUser = new User();
                newUser.Login = login;
                newUser.PasswordSha256 = hash;

                if (guestRole != null)
                    newUser.UserRoleIdUserRole = guestRole.IdUserRole;
                else
                    return false;

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                return true;
            }
        }

        static AuthManager()
        {
            using (TourismDbContext dbContext = new TourismDbContext())
            {
                var adminRole = dbContext.Userroles.FirstOrDefault((r) => r.Name == "admin");

                if (adminRole == null)
                {
                    Userrole admin = new Userrole();
                    admin.Name = "admin";
                    dbContext.Userroles.Add(admin);
                    dbContext.SaveChanges();
                }

                var guestRole = dbContext.Userroles.FirstOrDefault((r) => r.Name == "guest");

                if (guestRole == null)
                {
                    Userrole guest = new Userrole();
                    guest.Name = "guest";
                    dbContext.Userroles.Add(guest);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
