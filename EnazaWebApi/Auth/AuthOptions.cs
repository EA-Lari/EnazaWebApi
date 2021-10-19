using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EnazaWebApi.Auth
{
    public class AuthOptions
    {
        const string KEY = "testjwttoken";  
        public const int LIFETIME = 10; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
