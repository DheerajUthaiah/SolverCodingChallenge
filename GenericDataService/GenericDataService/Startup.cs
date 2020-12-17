using DataService.Common;
using DataService.Data;
using DataService.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace GenericDataService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        const string corsPolicy = "CORS_POLICY";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy(corsPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            services.AddSingleton<Serilog.ILogger>(Log.Logger);
            services.AddTransient<ISolverDataService, SolverDataService>();
            services.AddTransient<ISolverRepository, SolverRepository>();
            services.AddTransient<IRepositoryService<ISolverRepository>, RepositoryService<ISolverRepository, SolverRepository>>();
            ConfigureSqlConnections(services);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SolverData API", Version = "v1" });
            });
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors(corsPolicy);
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolverData API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSqlConnections(IServiceCollection services)
        {
            SqlConfigurationList sqlConfigurationList = new SqlConfigurationList();
            Configuration.Bind("SqlConfigurationList", sqlConfigurationList);
            services.AddSingleton(sqlConfigurationList);
        }
    }
}
