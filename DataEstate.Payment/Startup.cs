using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataEstate.Auth.Authorization;
using Stripe;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using DataEstate.Helpers;
using DataEstate.Stripe.Interfaces;
using DataEstate.Stripe.Services;

namespace DataEstate.Payment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authScopes = new string[] {
                "subscription:create", "subscription:read", "subscription:update", "subscription:delete"
            };
            var authAuthority = Configuration.GetSection("OAuth:Authority").Value;
            var authAudience = Configuration.GetSection("OAuth:Audience").Value;
            services.AddAuthorization(
                o =>
                {
                    foreach (var authScope in authScopes)
                    {
                        o.AddPolicy(
                            authScope,
                            p => p.Requirements.Add(
                            new OAuthRequirement(authScope, authAuthority)
                            )
                        );
                    }
                }
            );
            services.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                    )
                    .AddJwtBearer(o => 
                    {
                        o.Audience = authAudience;
                        o.Authority = authAuthority;
                    });
            services.AddSingleton<IAuthorizationHandler, OAuthClientCredentialHandler>();
            services.AddSingleton<ISubscriptionService, SubscriptionService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe:SecretKey").Value);
            EncryptionHelper.SetEncryptionKey(Configuration.GetSection("Encryption:Key").Value);
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
