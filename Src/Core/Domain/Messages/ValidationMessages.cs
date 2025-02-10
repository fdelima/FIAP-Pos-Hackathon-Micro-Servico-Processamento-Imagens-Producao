using System.Diagnostics.CodeAnalysis;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages
{
    [ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
    /// <summary>
    /// Mensagens padrões de validação
    /// </summary>
    public struct ValidationMessages
    {
        /// <summary>
        /// O campo {PropertyName} é obrigatório.
        /// </summary>
        public const string RequiredField = "O campo {PropertyName} é obrigatório.";
        public static string RequiredFieldWhithPropertyName(string PropertyName) => RequiredField.Replace("{PropertyName}", PropertyName);

        /// <summary>
        /// Necessário informar pelo menos um item.
        /// </summary>
        public const string OneMandatoryItem = "Você deve inserir pelo menos um item.";

        /// <summary>
        /// O campo {PropertyName} deve ter pelo menos {MinLength} caracteres.
        /// </summary>
        public const string MinLength = "O campo {PropertyName} deve ter pelo menos {MinLength} caracteres.";

        /// <summary>
        /// O campo {PropertyName} deve ter no maxímo {MaxLength} caracteres.
        /// </summary>
        public const string MaxLength = "O campo {PropertyName} deve ter no máximo {MaxLength} caracteres.";

        /// <summary>
        /// Data inicio não pode ser maior que data fim.
        /// </summary>
        public const string DataMaior = "A data de início não pode ser posterior à data de término.";

        /// <summary>
        /// CPF inválido.
        /// </summary>
        public const string CpfInvalid = "CPF inválido.";

        /// <summary>
        /// Diferença entre valor atual e novo valor
        /// </summary>
        public const string MustBeDifferent = "O valor atual deve ser diferente do novo valor.";

        /// <summary>
        /// Arquivo necessário/obrigatório
        /// </summary>
        public const string inputFile = "Necessário inserir o arquivo.";

        /// <summary>
        /// Forma de arquivo desconhecido.
        /// </summary>
        public const string unknownFileFormat = "Formato de arquivo desconhecido.";

        /// <summary>
        /// Mensagem de email invalido
        /// </summary>
        public const string InvalidEmail = "E-mail inválido.";

        /// <summary>
        /// Mensagem de email invalido
        /// </summary>
        public const string InvalidUrl = "Url inválida.";

        /// <summary>
        /// Todos os campos devem estar preenchidos
        /// </summary>
        public const string RequiredAllFields = "Certifique-se de que todos os campos estão preenchidos.";

        /// <summary>
        /// Valor deve ser maior que 0
        /// </summary>
        public const string MustBeGreaterZero = "O campo {PropertyName} deve ser maior que 0.";

        /// <summary>
        /// Valor invalido
        /// </summary>
        public const string InvalidValue = "O campo {PropertyName} não permite o valor informado.";
        public static string InvalidValueWhithPropertyName(string PropertyName) => InvalidValue.Replace("{PropertyName}", PropertyName);
    }

}