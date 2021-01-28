using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Umi.API.Database;
using Umi.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Umi.API
{
    

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // inject service: {custom, managed}
        public void ConfigureServices(IServiceCollection services)
        {
            // inject managed services
            services.AddControllers();
            // add service dependence: <interface, implementation>
            // 1. every request init independent data repo 
            // services.AddTransient<ITouristRouteRepository, MockTouristRouteRepository>();
            services.AddTransient<ITouristRouteRepository, TouristRouteRepository>();

            // 2. init only repository, shared data channel 
            // services.AddSingleton<>();
            // 3. transaction (grouped requests) - repo
            // services.AddScoped<>();
            
            services.AddDbContext<AppDbContext>(option =>
            {
                // conn for docker
                // option.UseSqlServer("server=localhost; Database=UmiDb; User Id=sa; Password=Gravi1984");
                // option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
                option.UseMySql(Configuration["DbContext:MySQLConn"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // HTTP request pipeline: e.g. request -> logging -> static files -> MVC(terminal)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // if in Development env, exception throw on Page, Prod will not. 
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                // endpoints.MapGet("/test", async context =>
                // {
                //     throw new Exception("test");
                //         await context.Response.WriteAsync("Hello test!");
                // });


                endpoints.MapControllers();
                
                
            });
        }
    }
}