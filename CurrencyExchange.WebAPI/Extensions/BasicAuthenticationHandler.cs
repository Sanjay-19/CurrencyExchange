using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace CurrencyExchange.WebAPI
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string user = "user";
        private const string pass = "pass";
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock):base(options,logger,encoder, clock)
        {

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header wasn't found");
            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (!string.IsNullOrEmpty(authenticationHeaderValue.Parameter))
                {
                    var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                    string[] credentials = Encoding.UTF8.GetString(bytes).Split(':');
                    string userName = credentials[0];
                    string password = credentials[1];
                    if (user.Equals(userName) && pass.Equals(password))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, user) };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                }
                return AuthenticateResult.Fail("Invalid username or password");
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error occured while authenticating");
            }
        }
    }
}
