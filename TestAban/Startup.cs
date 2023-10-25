using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Data;
using TestAban.Services;


namespace TestAban
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
            var connectionString = Configuration.GetConnectionString("SQLiteConnection");
            services.AddScoped<IDbConnection>((sp) => new SqliteConnection(connectionString));


            // Configuración de Serilog para escribir en archivo
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt") // Nombre del archivo
                .CreateLogger();

            // Configuración del LoggerFactory
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(); // Agregar Serilog al LoggerFactory
            });

            services.AddScoped<IClienteService, ClienteService>();


            // Configuración de Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API de Clientes", Version = "v1" });

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            // Habilita Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nombre de tu API v1");
                c.RoutePrefix = "swagger";
                //c.DocExpansion(DocExpansion.List);
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
