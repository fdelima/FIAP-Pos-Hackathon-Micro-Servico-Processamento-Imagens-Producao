using Microsoft.AspNetCore.Http;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models
{
    public class ProcessamentoImagemUploadModel
    {
        public DateTime Data { get; set; }

        public required string Usuario { get; set; }

        public required IFormFile FormFile { get; set; }
    }
}
