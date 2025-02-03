namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    /// <summary>
    /// Parametros de entrada para paginação do resultado da query
    /// </summary>
    public interface IPagingQueryParam
    {
        /// <summary>
        /// Página a ser retornada, contagem a partir do 1.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Quantidade a ser retornada por página
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Propriedade a ser ordenada
        /// </summary>
        public string? SortProperty { get; set; }

        /// <summary>
        /// Direção a ser ordenada
        /// </summary>
        public string? SortDirection { get; set; }

    }
}