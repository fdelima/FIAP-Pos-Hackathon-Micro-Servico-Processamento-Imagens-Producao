namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain
{
    /// <summary>
    /// Paginação do resultado da query
    /// </summary>
    public class PagingQueryResult<TModel>
    {
        /// <summary>
        /// Registros retornados
        /// </summary>
        public IEnumerable<TModel> Content { get; set; }

        /// <summary>
        /// Total de elementos encontrados
        /// </summary>
        public int TotalElements { get; }

        /// <summary>
        /// Quantidade a ser retornada por página
        /// </summary>
        public int Take { get; } = 10;

        /// <summary>
        /// Número de elementos sendo retornado
        /// </summary>
        public int NumberOfElements { get; }

        /// <summary>
        /// Total de paginas
        /// </summary>
        public int TotalPages => Take.Equals(0) ? 1 : (int)Math.Ceiling((decimal)NumberOfElements / Take);

        /// <summary>
        /// Construtor da paginação do resultado da query
        /// </summary>
        public PagingQueryResult(List<TModel> content, int numberOfElements, int take)
        {
            Content = content;
            TotalElements = content.Count;
            Take = take;
            NumberOfElements = numberOfElements;
        }

        /// <summary>
        /// Construtor da paginação do resultado da query
        /// </summary>
        public PagingQueryResult(List<TModel> content)
        {
            Content = content;
            TotalElements = content.Count;
            NumberOfElements = content.Count;
        }

    }
}