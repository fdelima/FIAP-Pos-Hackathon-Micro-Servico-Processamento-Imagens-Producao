using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions
{
    /// <summary>
    /// Extensão da model para informar os campos de validação.
    /// </summary>
    public static class NotificacaoExtension
    {
        /// <summary>
        /// Retorna a regra de validação a ser utilizada na atualização.
        /// </summary>
        public static Expression<Func<Notificacao, bool>> ConsultRule(this PagingQueryParam<Notificacao> param)
        {
            return x => (x.IdNotificacao.Equals(param.ObjFilter.IdNotificacao) || param.ObjFilter.IdNotificacao.Equals(default)) &&
                        (x.Usuario.Equals(param.ObjFilter.Usuario) || param.ObjFilter.Usuario.Equals(default)) &&
                        (x.Mensagem.Contains(param.ObjFilter.Mensagem) || string.IsNullOrWhiteSpace(param.ObjFilter.Mensagem)) &&
                        (x.Data.Equals(param.ObjFilter.Data) || param.ObjFilter.Data.Equals(default));
        }

        /// <summary>
        /// Retorna a propriedade a ser ordenada
        /// </summary>
        public static Expression<Func<Notificacao, object>> SortProp(this PagingQueryParam<Notificacao> param)
        {
            switch (param?.SortProperty?.ToLower())
            {
                case "idnotificacao":
                    return fa => fa.IdNotificacao;
                case "iddispositivo":
                    return fa => fa.Usuario;
                case "mensagem":
                    return fa => fa.Mensagem;
                default: return fa => fa.Data;
            }
        }
    }
}
