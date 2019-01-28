using Alexinea.Autofac.Extensions.DependencyInjection;
using AntiTruble.Equipment.Core;
using AntiTruble.Equipment.Models;
using AntiTruble.Person.Core;
using AntiTruble.Person.Models;
using AntiTruble.Repairs.Core;
using AntiTruble.Repairs.Models;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AntiTruble.Person
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<AntiTruble_PersonContext>(options => options
               .UseSqlServer(Configuration.GetConnectionString("LocalDB")));
            // установка конфигурации подключения
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/Person/Login");
                });
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return BuildAutofacContainer(services);
        }

        private IServiceProvider BuildAutofacContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AntiTruble_EquipmentContext>()
                           .UseSqlServer(Configuration.GetConnectionString("EquipmentDb"));
                return new AntiTruble_EquipmentContext(optionsBuilder.Options);
            }).As<AntiTruble_EquipmentContext>().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AntiTruble_RepairsContext>()
                           .UseSqlServer(Configuration.GetConnectionString("RepairsDb"));
                return new AntiTruble_RepairsContext(optionsBuilder.Options);
            }).As<AntiTruble_RepairsContext>().InstancePerLifetimeScope();
            builder.RegisterType<RepairsRepository>().As<IRepairsRepository>().InstancePerDependency();
            builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>().InstancePerDependency();

            builder.Populate(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
