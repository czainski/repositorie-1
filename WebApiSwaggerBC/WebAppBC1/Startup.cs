using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.OpenApi.Models;


namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration config) =>  Configuration = config;

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opts => {
                opts.UseSqlServer(Configuration[
                    "ConnectionStrings:ProductConnection"]);
                opts.EnableSensitiveDataLogging(true);
            });

            services.AddControllers();
            services.Configure<JsonOptions>(opts => {
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            });
            //    services.AddSwaggerGen();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, Context context)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            app.UseDeveloperExceptionPage();
            app.UseRouting();
                       
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                   await context.Response.WriteAsync("Hello! WebApiSwaggerBC!");
                });
                endpoints.MapControllers();
            });
            SeedData.SeedDatabase(context);
        }
    }
}
//
