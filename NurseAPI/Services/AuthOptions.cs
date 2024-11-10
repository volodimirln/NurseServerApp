using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NurseAPI.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "NurseAPI";
        public const string AUDIENCE = "NurseClient";
        const string KEY = "abcofbadtastesingkomsomolsk";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
