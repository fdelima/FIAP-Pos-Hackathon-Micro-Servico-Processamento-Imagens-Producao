using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using MediatR;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands
{
    public class ProcessamentoImagemPostCommand : IRequest<ModelResult>
    {
        public ProcessamentoImagemPostCommand(Domain.Entities.ProcessamentoImagem entity,
            string[]? businessRules = null)
        {
            Entity = entity;
            BusinessRules = businessRules;
        }

        public Domain.Entities.ProcessamentoImagem Entity { get; private set; }
        public string[]? BusinessRules { get; private set; }
    }
}