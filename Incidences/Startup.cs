using Incidences.Business;
using Incidences.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Incidences
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IIncidenceBz, IncidenceBz>();
            services.AddTransient<IReportBz, ReportBz>();
            services.AddTransient<IPieceBz, PieceBz>();
            services.AddTransient<IEmployeeBz, EmployeeBz>();
            services.AddTransient<ICredentialsBz, CredentialsBz>();
            services.AddTransient<IEmployeeRangeBz, EmployeeRangeBz>();
            services.AddTransient<ICredentialsData, CredentialsData>();
            services.AddTransient<IIncidenceData, IncidenceData>();
            services.AddTransient<IEmployeeData, EmployeeData>();
            services.AddTransient<IEmployeeRangeData, EmployeeRangeData>();
            services.AddTransient<INoteData, NoteData>();
            services.AddTransient<IPieceData, PieceData>();
            services.AddTransient<IPieceTypeData, PieceTypeData>();
            services.AddTransient<IReportData, ReportData>();
            services.AddTransient<IPieceTypeBz, PieceTypeBz>();
            services.AddTransient<INoteBz, NoteBz>();
            services.AddTransient<IPasswordRecoveryData, PasswordRecoveryData>();
            services.AddTransient<IPasswordRecoveryBz, PasswordRecoveryBz>();
            // CONFIGURACIÓN DEL SERVICIO DE AUTENTICACIÓN JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:ClaveSecreta"])
                        )
                    };
                });
            services.AddDbContext<IncidenceContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")
                )
            );
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // AÑADIMOS EL MIDDLEWARE DE AUTENTICACIÓN
            // DE USUARIOS AL PIPELINE DE ASP.NET CORE
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
