using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using MediatR;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands
{
    public class ProcessamentoImagemPutCommand : IRequest<ModelResult>
    {
        public ProcessamentoImagemPutCommand(Guid id, Domain.Entities.ProcessamentoImagem entity,
            string[]? businessRules = null)
        {
            Id = id;
            Entity = entity;
            BusinessRules = businessRules;
        }

        public Guid Id { get; private set; }
        public Domain.Entities.ProcessamentoImagem Entity { get; private set; }
        public string[]? BusinessRules { get; private set; }
    }
}