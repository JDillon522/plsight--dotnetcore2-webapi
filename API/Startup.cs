using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions;

namespace API
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(opts => opts.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()
                ));
            /// Change the way JSON is serialized. Pascal Vs Camel
            // .AddJsonOptions(obj => {
            //     if (obj.SerializerSettings.ContractResolver != null) {
            //         var castResolver = obj.SerializerSettings.ContractResolver as DefaultContractResolver;

            //         castResolver.NamingStrategy = null;
            //     }
            // });
// These are compiler conditionals
#if DEBUG
                services.AddTransient<IMailService, LocalMailService>();
#else
                services.AddTransient<IMailService, CloudMailService>();
#endif

            string connectionString = Startup.Configuration["connectionStrings:cityInfoContextLocalDb"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>(); // Add repo to services

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    new ExceptionHandlerOptions
                    {
                        ExceptionHandler = async context =>
                        {
                            context.Response.ContentType = "text/plain";
                            await context.Response.WriteAsync( "Welcome to the error page." );
                        }
                    }
                );
            }

            // Map entities to Dtos
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<City, CityWithoutPointsDto>();
                config.CreateMap<City, CityDto>();
                config.CreateMap<Point, PointOfInterestDto>();
                config.CreateMap<PointOfInterestForCreationDto, Point>();
                config.CreateMap<PointOfInterestForUpdateDto, Point>();
                config.CreateMap<Point, PointOfInterestForUpdateDto>();
                config.CreateMap<CityForCreationDto, City>();
            });
            // app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
