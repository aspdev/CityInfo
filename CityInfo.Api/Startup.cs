using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo.Api
{
    public class Startup
    {

        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));
            /*.AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver
                        as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            });*/
            
            services.AddTransient<IMailService, MailService>();

            var connectionString = @"Server=(localdb)\MSSQLlocalDB;Database=CityInfoDB;Trusted_Connection=True";
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CityInfoContext cityInfoContext)
        {

            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog();
            cityInfoContext.EnsureSeedDataForContext();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseMvc();

            /*app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!"); 

            });*/

            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                    cfg.CreateMap<Entities.City, Models.CityDto>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                    cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Models.PointOfInterestUpdateDto, Entities.PointOfInterest>();
                });


        }
    }
}
