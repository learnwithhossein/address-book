using Desktop.Model;
using Desktop.Properties;

namespace Desktop.Repository
{
    public class AuthRepository : GenericRepository<LoginResult>
    {
        public LoginResult Login()
        {
            var body = new { email = Settings.Default.Email, password = Settings.Default.Password };
            return Post("auth/login", body);
        }
    }
}
