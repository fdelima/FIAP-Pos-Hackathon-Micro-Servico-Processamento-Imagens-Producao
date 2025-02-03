using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;

public partial class ProcessamentoImagem : IDomainEntity
{
    /// <summary>
    /// Retorna a regra de validação a ser utilizada na inserção.
    /// </summary>
    public Expression<Func<IDomainEntity, bool>> InsertDuplicatedRule()
    {
        return x => ((ProcessamentoImagem)x).Usuario.Equals(Usuario) &&
                    ((ProcessamentoImagem)x).NomeArquivo.Equals(NomeArquivo) &&
                    ((ProcessamentoImagem)x).DataFimProcessamento == null;
    }

    /// <summary>
    /// Retorna a regra de validação a ser utilizada na atualização.
    /// </summary>
    public Expression<Func<IDomainEntity, bool>> AlterDuplicatedRule()
    {
        return x => !((ProcessamentoImagem)x).IdProcessamentoImagem.Equals(IdProcessamentoImagem) &&
                    ((ProcessamentoImagem)x).Usuario.Equals(Usuario) &&
                    ((ProcessamentoImagem)x).NomeArquivo.Equals(NomeArquivo) &&
                    ((ProcessamentoImagem)x).DataFimProcessamento == null;
    }

    public Guid IdProcessamentoImagem { get; set; }

    public DateTime Data { get; set; }

    public required string Usuario { get; set; }

    public DateTime DataEnvio { get; set; }
    public DateTime? DataEnviadoFila { get; set; }
    public DateTime? DataInicioProcessamento { get; set; }
    public DateTime? DataFimProcessamento { get; set; }

    public required string NomeArquivo { get; set; }
    public long TamanhoArquivo { get; set; }
    public required string NomeArquivoZipDownload { get; set; }
}
