using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Services.Services.ImportService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Betto.Api
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
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();

            services.Configure<RapidApiConfiguration>(Configuration.GetSection("RapidApiConfiguration"));

            ConfigureDatabaseConnection(services);
            ConfigureRepositories(services);
            ConfigureBettoServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDatabaseConnection(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DevelopmentConnectionString");
            services.AddDbContext<BettoDbContext>(options => options.UseSqlServer(connectionString,
                o => o.MigrationsAssembly("Betto.DataAccessLayer")));
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
        }

        private void ConfigureBettoServices(IServiceCollection services)
        {
            services.AddScoped<IImportService, ImportService>();
        }

    }
}