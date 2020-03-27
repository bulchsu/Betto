using Betto.DataAccessLayer;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Helpers.Configuration;
using Betto.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddControllers()
                .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            ConfigureDatabaseConnection(services);
            SetCustomConfiguration(services);
            ConfigureHelpers(services);
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

            app.UseCors("CorsPolicy");

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

        private void SetCustomConfiguration(IServiceCollection services)
        {
            services.Configure<RapidApiConfiguration>(Configuration.GetSection("RapidApiConfiguration"));
            services.Configure<LoggingConfiguration>(Configuration.GetSection("LoggingConfiguration"));
        }

        private void ConfigureHelpers(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddScoped<ITeamParser, TeamParser>();
            services.AddScoped<ILeagueParser, LeagueParser>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
        }

        private void ConfigureBettoServices(IServiceCollection services)
        {
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<ILeagueService, LeagueService>();
            services.AddScoped<ITeamService, TeamService>();
        }

    }
}