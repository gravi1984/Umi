using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Umi.API.Database;
using Umi.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

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
            // return 406 when request with unsupported Accept header
            services.AddControllers(setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true;
                    // setupAction.OutputFormatters.Add(
                    //     new XmlDataContractSerializerOutputFormatter()
                    //     );
                })
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters()
                // configure data validation fail response 422
                .ConfigureApiBehaviorOptions(
                    setupAction => setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        // info to shown to FE
                        var problemDetail = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "no matter",
                            Title = "data validation fail",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "please look clarification",
                            Instance = context.HttpContext.Request.Path
                        };
                        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemDetail)
                        {
                            ContentTypes = {"application/problem+json"}
                        };
                    });


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

            // register AutoMapper services
            // scan profile file, to do auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // inject authenticate service and read configuration parameters
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    var secretByte = Encoding.UTF8.GetBytes(Configuration["Authenticate:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer =  Configuration["Authenticate:Issuer"],
                        
                        ValidateAudience = true,
                        ValidAudience = Configuration["Authenticate:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey =  new SymmetricSecurityKey(secretByte)

                    };
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

            // where are u?
            app.UseRouting();

            // apply AUTH framework
            // who u are?
            app.UseAuthentication();
            
            // what u can do?
            app.UseAuthorization();

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