using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Emotify.Authorization;
using Emotify.Authorization.Discord;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Emotify
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env
        )
        {
            Environment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            if (Environment.IsDevelopment())
            {
            }

            services.AddDbContext<EmotifyDbContext>(
                options => options.UseSqlite(Configuration.GetConnectionString("EmotifyContext"))
            );

            services.AddRazorPages().AddRazorRuntimeCompilation();

            // for url highlight tag helper
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
            
            // services.AddIdentity<Models.EmotifyUser, IdentityRole>()
            //     .AddEntityFrameworkStores<EmotifyDbContext>();
            
            // services.AddAuthorization(
            //     options =>
            //     {
            //         // options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //         // .Require
            //         // .RequireAuthenticatedUser()
            //         // .Build();
            //     }
            // );
            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = "scheme.discord";
                        // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                    }
                )
                .AddCookie()
                .AddDiscord(
                    "scheme.discord",
                    options =>
                    {
                        options.ClientId = Configuration["Discord:ClientId"];
                        options.ClientSecret = Configuration["Discord:ClientSecret"];
                        options.SaveTokens = true;
                        options.Scope.Add("guilds");
                        // options.Events.OnCreatingTicket = ctx =>
                        // {
                        //     var tokens = ctx.Properties.GetTokens().ToList();
                        //     tokens.Add(new AuthenticationToken()
                        //     {
                        //         Name = "TicketCreated",
                        //         Value = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                        //     });
                        //     tokens.Add(new AuthenticationToken()
                        //     {
                        //         Name = "DiscordToken",
                        //         Value = ctx.AccessToken
                        //     });
                        //     ctx.Properties.StoreTokens(tokens);
                        //     return Task.CompletedTask;
                        // };
                        // options.Events.OnTicketReceived = ctx =>
                        // {
                        //     if (!ctx.Principal.HasClaim(c => c.Type == "DiscordToken"))
                        //     {
                        //         ctx.Principal.Identities.First().AddClaim(new Claim("DiscordToken", "12#"));
                        //     };
                        //     return Task.CompletedTask;
                        // };
                    }
                );
            services.AddScoped<IAuthorizationHandler, EmoteAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ManageGuildEmotesAuthorizationHandler>();

            services.AddScoped<UserHelper>();
            services.AddSingleton<UserGuildStore>();

            var discordClient = new DiscordSocketClient();
            discordClient.LoginAsync(TokenType.Bot, Configuration["Discord:Token"]).GetAwaiter().GetResult();
            discordClient.StartAsync().GetAwaiter().GetResult();
            services.AddSingleton(discordClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}