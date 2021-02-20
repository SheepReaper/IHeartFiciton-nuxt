using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.IO;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Sheep.IHeartFiction.ApiServer
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "Allow-All";

        public void ConfigureServices(IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services
                .AddCors(options =>
                    options.AddPolicy(
                        DefaultCorsPolicyName,
                        builder => builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()))
                .AddSingleton(typeof(IStore<>), typeof(Store<>))
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "The Api", Version = "v1" });
                    options.UseOneOfForPolymorphism();
                    options.IncludeXmlComments(xmlPath);
                })
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app
                .UseRouting()
                .UseCors(DefaultCorsPolicyName)
                .UseSwagger()
                .UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"))
                .UseEndpoints(endpoints =>
                    endpoints.MapDefaultControllerRoute());
        }
    }
}