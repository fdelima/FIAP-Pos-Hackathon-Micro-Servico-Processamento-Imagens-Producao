using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    public static class ModelValidations
    {
        internal static void ConfigureModelValidations(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
