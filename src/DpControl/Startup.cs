﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.SwaggerGen;
using DpControl.Domain.IRepository;
using DpControl.Domain.Repository;
using DpControl.Domain.EFContext;
using DpControl.Utility.ExceptionHandler;
using DpControl.Utility.Middlewares;

namespace DpControl
{
    public class Startup
    {
        private string pathToDoc ;
        public static IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            
            pathToDoc = env.MapPath("../../../artifacts/bin/DpControl/Debug/dnx451/DpControl.xml");
            //pathToDoc = env.MapPath("../DpControl.xml");
            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }
        
        
       

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFramework()
                .AddSqlServer();
            
            services.AddMvc();

            //services.Configure<MvcOptions>(options =>
            //{
            //    //options.Filters.Add(new GlobalExceptionFilter());
            //    options.ModelBinders.Insert(0, new QueryModelBinder());
            //});

            #region  swagger
            services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(options =>
            {

                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "DpControl WebApi v1",
                    Description = "A WebApi Test and Document For DpControl",
                    TermsOfService = "None"
                });
                options.OperationFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlActionComments(pathToDoc));
            });

            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
                options.ModelFilter(new Swashbuckle.SwaggerGen.XmlComments.ApplyXmlTypeComments(pathToDoc));
            });

            #endregion

            services.AddTransient<ShadingContext, ShadingContext>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            // Add Application Insights exceptions handling to the request pipeline.
            app.UseApplicationInsightsExceptionTelemetry();

            //捕获全局异常消息
            app.UseExceptionHandler(errorApp =>GlobalExceptionBuilder.ExceptionBuilder(errorApp));
            //HTTP方法覆盖
            app.UseMiddleware<XHttpHeaderOverrideMiddleware>();

            app.UseStaticFiles();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerGen();

            app.UseSwaggerUi();

            

            DbInitialization.Initialize(app.ApplicationServices);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
