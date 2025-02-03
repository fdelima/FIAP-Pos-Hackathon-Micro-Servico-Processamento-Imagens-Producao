using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    public static class Swagger
    {
        static ILogger _logger = App.CreateLogger();

        /// <summary>
        /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        /// </summary>
        internal static void AddSwagger(this IServiceCollection services, string description)
        {
            _logger.LogInformation("[{AppName}]: Swagger Add Service", App.Name);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            string? startUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').First();
            bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(App.Version, new OpenApiInfo
                {
                    Version = App.Version,
                    Title = App.Title,
                    Description = description,
                    Contact = IsDevelopment ? new OpenApiContact { Url = new Uri($"{startUrl}/swagger") } : null
                });
            });
        }

        /// <summary>
        /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        /// </summary>
        internal static void ConfigureSwagger(this WebApplication app)
        {
            _logger.LogInformation("[{AppName}]: Swagger Configure App", App.Name);

            app.UseSwagger();

            //Se for desenvolvimento
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{App.Version}/swagger.json", $"{App.Name} {App.Version}");
                options.DocumentTitle = App.Title;
            });
            }

        }

        internal static void ConfigureReDoc(this WebApplication app)
        {
            _logger.LogInformation("[{AppName}]: ReDoc Configure App", App.Name);

            app.UseReDoc(reDoc =>
            {
                reDoc.DocumentTitle = $"Documentação: {App.Name}";
                reDoc.RoutePrefix = "api-docs";
                reDoc.SpecUrl = $"/swagger/{App.Version}/swagger.json";
            });
        }
    }
}
