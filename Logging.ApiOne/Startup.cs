using Logging.ApiOne.Controllers;
using Logging.Common;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Text;

namespace Logging.ApiOne
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
            services.AddHttpContextAccessor();
            services.AddTransient<HttpClientLoggingHandler>();
            services.AddHttpClient(nameof(OneController))
                .AddHttpMessageHandler<HttpClientLoggingHandler>();

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });

            //services.AddScoped<HttpClientLoggingHandler, HttpClientLoggingHandler>();
            var appName = Configuration.GetValue<string>("AppName");
            services.AddSingleton(new ApiSettings { AppName = appName });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CorrelationIdEnricher>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Map("/", async (ctx) =>
                {
                    await ctx.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("OK. 200!"));
                });
            });
        }
    }
}
