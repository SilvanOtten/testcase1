using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Fallback connection string for running via localhost
            var connectionString = Environment.GetEnvironmentVariable("AnimalDb") ??
                                   @"Server=localhost,1433;Database=AnimalDb;User Id=sa;Password=Geheim_101";

            //// AddDbContext registers DbContext types with a scoped lifetime by default
            services.AddDbContext<AnimalDbContext>(options => options.UseSqlServer(connectionString));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Await Database creation with Exponential backoff
            bool created = false;
            int maxRetries = 10;

            while (!created)
            {
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                        serviceScope.ServiceProvider.GetService<AnimalDbContext>().Database.EnsureCreated();

                        // Seeds database with animals in case there are no animals
                        serviceScope.ServiceProvider.GetService<AnimalDbContext>().EnsureDataSeeded();

                        created = true;
                    }
                    catch
                    {
                        Thread.Sleep((int)Math.Pow(2, i));
                    }
                }
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
