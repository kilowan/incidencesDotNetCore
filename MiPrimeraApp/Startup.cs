using Incidences.Business;
using Incidences.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddTransient<ISqlBase, SqlBase>();
            services.AddTransient<IBusinessBase, BusinessBase>();
            services.AddTransient<IPieceTypeBz, PieceTypeBz>();
            services.AddTransient<INoteBz, NoteBz>();
        }

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
    }
}
