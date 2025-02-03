using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages;
using FluentValidation;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Validator
{
    /// <summary>
    /// Regras de validação da model
    /// </summary>
    public class NotificacaoValidator : AbstractValidator<Notificacao>
    {
        /// <summary>
        /// Contrutor das regras de validação da model
        /// </summary>
        public NotificacaoValidator()
        {
            RuleFor(c => c.Mensagem).NotEmpty().WithMessage(ValidationMessages.RequiredField);
        }
    }
}
