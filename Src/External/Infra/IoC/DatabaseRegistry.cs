using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.IoC
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    internal static class DatabaseRegistry
    {
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //DB Context
            if (bool.Parse(configuration["UseInMemoryDatabase"] ?? "false"))
                services.AddDbContext<Context>(options =>
                    options.UseInMemoryDatabase("MyInMemoryDatabase"));
            else
                services.AddDbContext<Context>(options =>
                    options.UseMongoDB(
                        configuration.GetSection("MongoDb")["ConnectionString"] ?? "",
                        configuration.GetSection("MongoDb")["DatabaseName"] ?? ""));
        }
    }
}
