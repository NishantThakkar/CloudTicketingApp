using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingAppCloud.Service
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var secret = config.GetSection("JwtConfig").GetSection("secret").Value;

            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = "localhost",
                    ValidAudience = "localhost"
                };
                x.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // Add the access_token as a claim, as we may actually need it
                        //if (context.SecurityToken is JwtSecurityToken accessToken)
                        //{
                        //    if (context.Principal.Identity is ClaimsIdentity identity)
                        //    {
                        //        if (accessToken != null)
                        //        {
                        //            if (accessToken.Claims != null)
                        //            {
                        //                identity.AddClaim(new Claim("UserId", accessToken?.Claims?.FirstOrDefault(x => x.Type == "oid")?.Value.ToString()));
                        //                identity.AddClaim(new Claim("UserName", accessToken?.Claims?.FirstOrDefault(x => x.Type.ToLower() == "username")?.Value.ToString()));
                        //                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                        //            }
                        //        }
                        //    }
                        //}

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
