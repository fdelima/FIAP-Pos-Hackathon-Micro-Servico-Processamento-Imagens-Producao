using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain
{
    /// <summary>
    /// Parametros de entrada para paginação do resultado da query
    /// </summary>
    public class PagingQueryParam<TModel> : IPagingQueryParam
    {
        /// <summary>
        /// Página a ser retornada, contagem a partir do 1.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Quantidade a ser retornada por página
        /// </summary>
        public int Take { get; set; } = 10;

        /// <summary>
        /// Propriedade a ser ordenada
        /// </summary>
        public string? SortProperty { get; set; }

        /// <summary>
        /// Direção a ser ordenada
        /// </summary>
        public string? SortDirection { get; set; } = "Asc";

        /// <summary>
        /// Objeto com as propriedades preenchidas que será utilizado como filtro
        /// </summary>
        public TModel ObjFilter { get; set; }


    }
}