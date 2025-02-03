using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    /// <summary>
    /// Mensagens padrões de negócio
    /// </summary>
    public struct BusinessMessages
    {
        /// <summary>
        /// Retorna mensagem genérica de sucesso
        /// </summary>
        public static string GenericSucess() => "Operação realizada com sucesso.";

        /// <summary>
        /// Retorna mensagem genérica de sucesso para insert
        /// </summary>
        public static string InsertSucess<T>() => GenericSucess();

        /// <summary>
        /// Retorna mensagem genérica de sucesso para update
        /// </summary>
        public static string UpdateSucess<T>() => GenericSucess();

        /// <summary>
        /// Retorna mensagem genérica de sucesso para delete
        /// </summary>
        public static string DeleteSucess<T>() => GenericSucess();

        /// <summary>
        /// Retorna mensagem de erro - registro em duplicidade
        /// </summary>
        public static string DuplicatedError<T>() => $"Já existe um registro {typeof(T).Name}.";

        /// <summary>
        /// Retorna mensagem de erro - registro não encontrado
        /// </summary>
        public static string NotFoundError<T>() => $"Nenhum registro de {typeof(T).Name} encontrado.";

        /// <summary>
        /// Retorna mensagem de erro - registro não encontrado
        /// </summary>
        public static string NotFoundInError<T>(Guid id) => $"{typeof(T).Name} informado {id} não encontrado.";

    }
}