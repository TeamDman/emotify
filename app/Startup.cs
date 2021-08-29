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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
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

            // services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddIdentity<Models.EmotifyUser, IdentityRole>()
                .AddEntityFrameworkStores<EmotifyDbContext>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(
                    options =>
                    {
                        options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    });
            services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = $"/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                }
            );

            services.AddAuthorization(
                options =>
                {
                    // options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    // .Require
                    // .RequireAuthenticatedUser()
                    // .Build();
                }
            );
            services.AddAuthentication(
                    options =>
                    {

                    }
                )
                .AddDiscord(
                    "scheme.discord",
                    options =>
                    {
                        options.ClientId = Configuration["Discord:ClientId"];
                        options.ClientSecret = Configuration["Discord:ClientSecret"];
                        options.SaveTokens = true;
                        options.Scope.Add("guilds");
                        options.Events.OnCreatingTicket = ctx =>
                        {
                            ctx.RunClaimActions();
                            var tokens = ctx.Properties.GetTokens().ToList();
                            tokens.Add(new AuthenticationToken()
                            {
                                Name = "TicketCreated",
                                Value = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
                            });
                            ctx.Properties.StoreTokens(tokens);
                            return Task.CompletedTask;
                        };
                    }
                );
            services.AddScoped<IAuthorizationHandler, EmoteAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, DiscordGuildEnrollmentAuthorizationHandler>();


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