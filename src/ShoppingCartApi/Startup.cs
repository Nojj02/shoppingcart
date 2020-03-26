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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();

            var connectionString = Configuration.GetConnectionString(ConfigurationKeys.ConnectionString.Postgres);

            services.AddScoped<IItemTypeRepository, ItemTypeRepository>(x => new ItemTypeRepository(connectionString));
            services.AddScoped<IItemRepository, ItemRepository>(x => new ItemRepository(connectionString));
            services.AddScoped<ICouponRepository, CouponRepository>(x => new CouponRepository(connectionString));
            services.AddScoped<IItemTypeReadRepository, ItemTypeReadRepository>(x => new ItemTypeReadRepository(connectionString));
            services.AddScoped<IItemReadRepository, ItemReadRepository>(x => new ItemReadRepository(connectionString));
            services.AddScoped<ICouponReadRepository, CouponReadRepository>(x => new CouponReadRepository(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
