using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.IoC
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    internal static class GatewaysRegistry
    {
        public static void RegisterGateways(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGateways<>), typeof(BaseGateway<>));
            services.AddScoped<IStorageGateway, AzureBlobStorageGateway>();
            services.AddScoped<IMessagerGateway, AzureQueueStorageGateway>();
        }
    }
}