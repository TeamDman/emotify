using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Emotify.Areas.Identity.Data;

[assembly: HostingStartup(typeof(Emotify.Areas.Identity.IdentityHostingStartup))]
namespace Emotify.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EmotifyIdentityDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("EmotifyIdentityDbContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<EmotifyIdentityDbContext>();
            });
        }
    }
}