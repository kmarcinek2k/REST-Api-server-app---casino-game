using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Data;
using Game.Options.Services;
using Game.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IBoardService, BoardService>();
        }
    }
}
