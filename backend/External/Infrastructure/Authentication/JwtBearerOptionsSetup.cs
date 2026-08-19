using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Authentication
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtConfigurations _jwtConfigurations;

        public JwtBearerOptionsSetup(IOptions<JwtConfigurations> jwtConfigurations)
        {
            _jwtConfigurations = jwtConfigurations.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            options.MapInboundClaims = false;

            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtConfigurations.Issuer,
                ValidAudience = _jwtConfigurations.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey))
            };
        }
    }
}
