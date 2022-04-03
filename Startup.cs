using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mvc_Apteka
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(ConfigureMvc).AddRazorRuntimeCompilation().AddJsonOptions(ConfigureJson);
            string sqlServerConnectionString = Configuration.GetConnectionString("SqlServer");
            if (String.IsNullOrWhiteSpace(sqlServerConnectionString))
                throw new KeyNotFoundException("Не найдена строка соединения с SqlServer в конфигурации приложения " +
                    "(секция ConnectionStrings). Внесите исправления в appsettings.json и повторите попытку.");
            services.AddDbContext<AppDbContext>(options=>
                options.UseSqlServer(sqlServerConnectionString));
        }

        private void ConfigureJson(JsonOptions jsonOptions)
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            jsonOptions.JsonSerializerOptions.WriteIndented = true; 
            jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        }

        private void ConfigureMvc(MvcOptions mvcOptions)
        {
            //TODO
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
