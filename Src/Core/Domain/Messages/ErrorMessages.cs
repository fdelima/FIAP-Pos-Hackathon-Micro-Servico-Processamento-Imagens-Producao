using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    /// <summary>
    /// Mensagens de erro do sistema
    /// </summary>
    public struct ErrorMessages
    {
        /// <summary>
        /// Mensage de erro interno do sistema.
        /// </summary>
        public const string InternalServerError = "Oops! ocorreu um erro inesperado.";

        /// <summary>
        /// Mensage de erro timeout em um requisição.
        /// </summary>
        public const string TimeOutRequestError = "Solicitar tempo limite.";

        /// <summary>
        /// Mensage de erro timeout em uma requisição ao banco de dados.
        /// </summary>
        public const string TimeOutDatabaseError = "Tempo limite do banco de dados.";

        /// <summary>
        /// Mensage de erro timeout em uma requisição ao banco de dados.
        /// </summary>
        public static string DeleteDatabaseError<T>() => $"Erro ao deletar {typeof(T).Name} do banco de dados.";
    }
}
