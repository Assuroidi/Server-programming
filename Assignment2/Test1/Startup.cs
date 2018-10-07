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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Test1
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // services.AddTransient<Models.PlayerPrdoocessor>();
            // services.AddTransient<Models.InMemoryRepository>();
            services.AddSingleton<Models.PlayerProcessor>();
            // services.AddSingleton<Models.IRepository<Models.Player, Models.ModifiedPlayer> , Models.InMemoryRepository>();
            services.AddSingleton<Models.ItemsProcessor>();
            // services.AddSingleton<Models.IRepository<Models.Item, Models.ModifiedItem>, Models.ItemRepository>();
            services.AddSingleton<Models.IRepository,Models.MongoRepo>();
            services.AddSingleton<ApiAuthKey>(
                new ApiAuthKey(Configuration.GetValue<string>("api-key"),
                Configuration.GetValue<string>("api-key-admin")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddMvc();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireClaim("user"));
                options.AddPolicy("admin", policy => policy.RequireClaim("admin"));

            });
            services.AddSingleton<Models.LogProcessor>();
            
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
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
