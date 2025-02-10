namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models
{
    public class ProcessamentoImagemSendQueueModel
    {
        public Guid IdProcessamentoImagem { get; set; }
        public required string Usuario { get; set; }
        public DateTime DataEnviadoFila { get; set; }
        public required string NomeArquivo { get; set; }
        public long TamanhoArquivo { get; set; }
        public required string NomeArquivoZipDownload { get; set; }
    }
}
