using CodingEventsAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodingEventsAPI {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();
      
      services.AddDbContext<CodingEventsDbContext>(o => o.UseSqlite("Data Source=sqlite.db;"));
      
      services.AddSwaggerGen(
        options => {
          options.SwaggerDoc(
            "v1",
            new OpenApiInfo {
              Version = "v1",
              Title = "Ricky",
              Description = "REST API for managing Coding Events"
            }
          );
        }
      );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

      app.UseSwagger();
      app.UseSwaggerUI(
        options => {
          options.RoutePrefix = "10.0.0.192:5001"; // root path of the server
          options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Ricky's Coding Events API Documentation"
          );
        }
      );

      // run migrations on startup
      var dbContext = app.ApplicationServices.CreateScope()
        .ServiceProvider.GetService<CodingEventsDbContext>();
      dbContext.Database.Migrate();
    }
  }
}
