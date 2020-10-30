using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DutchContext>(cfg => 
            {
                cfg.UseSqlServer(config.GetConnectionString("DutchConnectionString"));
            });
            
            services.AddTransient<DutchSeeder>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IMailService, NullMailService>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddControllersWithViews().AddNewtonsoftJson(options => 
                                options.SerializerSettings.ReferenceLoopHandling= ReferenceLoopHandling.Ignore);
            services.AddRazorPages();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopmenC:\Users\ahcruzado\Documents\GitHub\AspNetCore3\DutchTreat\wwwroot\css\site.csst())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});            

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("<h1>Hello World!</h1>");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                //https://www.learnrazorpages.com/middleware
            }
            //app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNodeModules();

            app.UseRouting();
            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Fallback", 
                    "{controller}/{action}/{id?}", 
                    new { controller = "App", action = "Index" });

                cfg.MapRazorPages();
            });

            //app.UseMvc(cfg =>
            //{
            //    cfg.MapRoute("Default",
            //      "{controller}/{action}/{id?}",
            //      new { controller = "App", Action = "Index"});
            //});            
        }
    }
}
