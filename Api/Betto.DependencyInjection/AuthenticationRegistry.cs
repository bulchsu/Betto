using System.Text;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Betto.DependencyInjection
{
    internal static class AuthenticationRegistry
    {
        internal static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSecretKey = GetAuthenticationSecretKey(configuration);
            
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Events = new JwtBearerEvents { OnTokenValidated = OnTokenValidatedAsync };
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(authenticationSecretKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        private static async Task OnTokenValidatedAsync(TokenValidatedContext context)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var username = context.Principal.Identity.Name;
            var doesUserExist = await userService.CheckIsUsernameAlreadyTakenAsync(username);

            if (!doesUserExist)
            {
                context.Fail("Unauthorized");
            }
        }

        private static byte[] GetAuthenticationSecretKey(IConfiguration configuration)
        {
            var applicationMainConfiguration = configuration
                .GetSection(nameof(ApplicationMainConfiguration))
                .Get<ApplicationMainConfiguration>();

            return Encoding.ASCII.GetBytes(applicationMainConfiguration.AuthenticationSecretKey);
        }
    }
}
