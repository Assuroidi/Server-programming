using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Test1
{
    public class AuthMiddleware
    {
        RequestDelegate _next;
        ApiAuthKey _apiAuthKey;

        public AuthMiddleware(RequestDelegate next, ApiAuthKey apiAuthKey)
        {
            _next = next;
            _apiAuthKey = apiAuthKey;
        }

        public async Task Invoke(HttpContext context)
        {
            var claimsPrincipal = new System.Security.Claims.ClaimsPrincipal();
            var headers = context.Request.Headers;
            Claim claim = null;
            bool claimReady = false;

            if(headers.ContainsKey("x-api-key"))
            {
                context.User = claimsPrincipal;
                context.Response.StatusCode = 400;
            }
            else
            {
                if (headers["x-api-key"] == _apiAuthKey.key)
                {
                    claim = new Claim(ClaimTypes.Role, "User");
                    claimReady = true;
                }
                else if (headers["x-api-key"] == _apiAuthKey.adminKey)
                {
                    claim = new Claim(ClaimTypes.Role, "Admin");
                    claimReady = true;
                }
                else
                {
                    context.User = claimsPrincipal;
                    context.Response.StatusCode = 403;
                }


                if (claimReady)
                {
                    var identity = new ClaimsIdentity();
                    identity.AddClaim(claim);
                    claimsPrincipal.AddIdentity(identity);
                    context.User = claimsPrincipal;
                    
                    await _next(context);
                    return;
                }


            }
        }


    }
}