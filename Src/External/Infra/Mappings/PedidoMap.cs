using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Mappings;

internal class PedidoMap : IEntityTypeConfiguration<ProcessamentoImagem>
{
    public void Configure(EntityTypeBuilder<ProcessamentoImagem> builder)
    {
        builder.ToCollection("processamentoimagem");
        builder.HasKey(e => e.IdProcessamentoImagem);

        builder.Property(e => e.IdProcessamentoImagem)
            .ValueGeneratedNever()
            .HasElementName("_id");
        builder.Property(e => e.Data)
            .HasElementName("data");
        builder.Property(e => e.Usuario)
            .HasElementName("usuario");
        builder.Property(e => e.DataEnvio)
            .HasElementName("data_recebido");
        builder.Property(e => e.DataEnviadoFila)
            .HasElementName("data_enviado_fila");
        builder.Property(e => e.DataInicioProcessamento)
            .HasElementName("data_inicio_processamento");
        builder.Property(e => e.DataFimProcessamento)
            .HasElementName("data_fim_processamento");
        builder.Property(e => e.NomeArquivo)
            .HasElementName("nome_arquivo");
        builder.Property(e => e.TamanhoArquivo)
            .HasElementName("tamanho_arquivo");
        builder.Property(e => e.NomeArquivoZipDownload)
            .HasElementName("nome_arquivo_zip_download"); ;
    }
}
