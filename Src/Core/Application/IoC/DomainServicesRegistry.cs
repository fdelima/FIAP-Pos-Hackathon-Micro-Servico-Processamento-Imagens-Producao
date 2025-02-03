using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.IoC
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    internal static class DomainServicesRegistry
    {
        public static void RegisterDomainServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped(typeof(IService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IService<Notificacao>), typeof(NotificacaoService));
            services.AddScoped(typeof(IProcessamentoImagemService), typeof(ProcessamentoImagemService));
            services.AddScoped(typeof(IMessagerService), typeof(MessageService));
            services.AddScoped(typeof(IStorageService), typeof(AzureBlobStorageService));
        }
    }
}