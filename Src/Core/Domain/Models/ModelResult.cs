using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FluentValidation.Results;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models
{
    /// <summary>
    /// Result para os commands
    /// </summary>
    public class ModelResult : IModelResult
    {
        /// <summary>
        /// construtor vazio do result para os commands
        /// </summary>
        public ModelResult() { }

        /// <summary>
        /// contrutor do result para os commands
        /// </summary>
        /// <param name="model">model</param>
        public ModelResult(object model) => Model = model;

        /// <summary>
        /// lista de mensagens
        /// </summary>
        public List<string> Messages { get; } = new List<string>();

        /// <summary>
        /// lista de erros
        /// </summary>
        public List<string> Errors { get; } = new List<string>();


        /// <summary>
        /// Model
        /// </summary>
        public object Model { get; }

        /// <summary>
        /// Retorna se é válido
        /// </summary>
        public virtual bool IsValid => Errors == null || Errors.Count == 0;

        /// <summary>
        /// Adiciona as validações
        /// </summary>
        public void Add(ModelResult model)
        {
            AddError(model.ListErrors());
            AddMessage(model.ListMessages());
        }

        /// <summary>
        /// Adiciona a lista mensagens
        /// </summary>
        /// <param name="message">mensagem</param>
        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Adiciona a lista de erros
        /// </summary>
        /// <param name="error">erro</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Adiciona a lista de erros
        /// </summary>
        /// <param name="errors">erros</param>
        public void AddError(IReadOnlyList<string> errors)
        {
            Errors.AddRange(errors);
        }

        /// <summary>
        /// Adiciona a lista de mensagens
        /// </summary>
        /// <param name="messages">mensagens</param>
        public void AddMessage(IReadOnlyList<string> messages)
        {
            Messages.AddRange(messages);
        }

        /// <summary>
        /// Retorna a lista de mensagens
        /// </summary>
        public IReadOnlyList<string> ListMessages()
        {
            return Messages;
        }

        /// <summary>
        /// Retorna a lista de erros
        /// </summary>
        public IReadOnlyList<string> ListErrors()
        {
            return Errors;
        }

        public void AddValidations(ValidationResult validations)
        {
            validations.Errors.ForEach(item => { Errors.Add(item.ErrorMessage); });
        }
    }
}
