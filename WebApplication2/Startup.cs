using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using WebApplication2.Controllers;

namespace WebApplication2
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      BsonClassMap.RegisterClassMap<Blog>(x =>
      {
        x.AutoMap();
        x.SetIgnoreExtraElements(true);
        x.MapIdMember(y => y.Id);
      });
      
      BsonClassMap.RegisterClassMap<Post>(x =>
      {
        x.AutoMap();
        x.SetIgnoreExtraElements(true);
      });
      
      BsonClassMap.RegisterClassMap<Author>(x =>
      {
        x.AutoMap();
        x.SetIgnoreExtraElements(true);
      });
      
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      // app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}