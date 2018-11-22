using CopaDeFilmes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CopaDeFilmes.Api.Services.Abstracts;
using CopaDeFilmes.Api.Services.Concretes;

namespace CopaDeFilmes.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IFilmesService, FilmesService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Lista de Filmes",
                        Version = "v1",
                        Description = "API Rest para obter uma lista de Filmes",
                        Contact = new Contact
                        {
                            Name = "Filmes Emerson",
                            Url = "https://github.com/EmersonMeloMachado"
                        }
                    });
                
                c.IncludeXmlComments(System.String.Format(@"{0}\CopaDeFilmes.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
            });
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app,IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Lista de Filmes");
            });
        }
    }
}