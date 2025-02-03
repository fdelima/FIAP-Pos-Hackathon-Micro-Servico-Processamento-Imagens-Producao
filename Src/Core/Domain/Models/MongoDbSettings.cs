using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    public class MongoDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }

}
