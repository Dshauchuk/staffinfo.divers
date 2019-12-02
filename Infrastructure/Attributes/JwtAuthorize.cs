using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Staffinfo.Divers.Infrastructure.Attributes
{

    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// <remarks>ONLY FOR BEARER AUTH SCHEMA</remarks>
    /// </summary>
    public class JwtAuthorize : AuthorizeAttribute
    {
        public JwtAuthorize() : base()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
