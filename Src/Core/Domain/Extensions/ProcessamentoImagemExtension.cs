using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions
{
    /// <summary>
    /// Extensão da model para informar os campos de validação.
    /// </summary>
    public static class ProcessamentoImagemExtension
    {
        /// <summary>
        /// Retorna a regra de validação a ser utilizada na atualização.
        /// </summary>
        public static Expression<Func<ProcessamentoImagem, bool>> ConsultRule(this PagingQueryParam<ProcessamentoImagem> param)
        {
            return x => (x.IdProcessamentoImagem.Equals(param.ObjFilter.IdProcessamentoImagem) || param.ObjFilter.IdProcessamentoImagem.Equals(default)) &&
                        (param.ObjFilter.Data.Equals(default) || x.Data.Equals(param.ObjFilter.Data)) &&
                        (param.ObjFilter.Usuario == null || param.ObjFilter.Usuario.Equals(default) || x.Usuario.Equals(param.ObjFilter.Usuario)) &&
                        (param.ObjFilter.DataEnvio.Equals(default) || x.DataEnvio.Equals(param.ObjFilter.DataEnvio)) &&
                        (param.ObjFilter.DataEnviadoFila == null || param.ObjFilter.DataEnviadoFila.Equals(null) || param.ObjFilter.DataEnviadoFila.Equals(default) || x.DataEnviadoFila.Equals(param.ObjFilter.DataEnviadoFila)) &&
                        (param.ObjFilter.DataInicioProcessamento == null || param.ObjFilter.DataInicioProcessamento.Equals(default) || x.DataInicioProcessamento.Equals(param.ObjFilter.DataInicioProcessamento)) &&
                        (param.ObjFilter.DataFimProcessamento == null || param.ObjFilter.DataFimProcessamento.Equals(default) || x.DataFimProcessamento.Equals(param.ObjFilter.DataFimProcessamento)) &&
                        (param.ObjFilter.NomeArquivo == null || param.ObjFilter.NomeArquivo.Equals(default) || x.NomeArquivo.Equals(param.ObjFilter.NomeArquivo)) &&
                        (param.ObjFilter.TamanhoArquivo == null || param.ObjFilter.TamanhoArquivo.Equals(default) || x.TamanhoArquivo.Equals(param.ObjFilter.TamanhoArquivo));
        }

        /// <summary>
        /// Retorna a propriedade a ser ordenada
        /// </summary>
        public static Expression<Func<ProcessamentoImagem, object>> SortProp(this PagingQueryParam<ProcessamentoImagem> param)
        {
            switch (param?.SortProperty?.ToLower())
            {
                case "idprocessamentoimagem":
                    return fa => fa.IdProcessamentoImagem;
                case "usuario":
                    return fa => fa.Usuario;
                case "datarecebido":
                    return fa => fa.DataEnvio;
                case "dataenviadofila":
                    return fa => fa.DataEnviadoFila;
                case "datainicioprocessamento":
                    return fa => fa.DataInicioProcessamento;
                case "datafimprocessamento":
                    return fa => fa.DataFimProcessamento;
                case "nomearquivo":
                    return fa => fa.NomeArquivo;
                case "tamanhoarquivo":
                    return fa => fa.TamanhoArquivo;
                case "nomearquivozipdownload":
                    return fa => fa.NomeArquivoZipDownload;
                default: return fa => fa.Data;
            }
        }
    }
}
