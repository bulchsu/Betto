using System.Text;
using System.Threading.Tasks;
using Betto.DataAccessLayer;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Helpers.Configuration;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            ConfigureJwtAuthentication(services);
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

            app.UseAuthentication();
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
            services.Configure<ApplicationMainConfiguration>(Configuration.GetSection("ApplicationMainConfiguration"));
        }

        private void ConfigureHelpers(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddScoped<ITeamParser, TeamParser>();
            services.AddScoped<ILeagueParser, LeagueParser>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IObjectValidator, ObjectValidator>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
        }

        private void ConfigureBettoServices(IServiceCollection services)
        {
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<ILeagueService, LeagueService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            var applicationMainConfiguration = Configuration
                .GetSection(nameof(ApplicationMainConfiguration))
                .Get<ApplicationMainConfiguration>();

            var authenticationSecretKey = Encoding.ASCII.GetBytes(applicationMainConfiguration.AuthenticationSecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
        }

        private async Task OnTokenValidatedAsync(TokenValidatedContext context)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var username = context.Principal.Identity.Name;
            var doesUserExist = await userService.CheckIsUsernameAlreadyTakenAsync(username);

            if (!doesUserExist)
            {
                context.Fail("Unauthorized");
            }
        }
    }
}