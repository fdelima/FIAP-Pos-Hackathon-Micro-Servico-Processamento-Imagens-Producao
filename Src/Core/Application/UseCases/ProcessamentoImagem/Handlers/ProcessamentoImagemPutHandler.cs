using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using MediatR;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Handlers
{
    public class ProcessamentoImagemPutHandler : IRequestHandler<ProcessamentoImagemPutCommand, ModelResult>
    {
        private readonly IProcessamentoImagemService _service;

        public ProcessamentoImagemPutHandler(IProcessamentoImagemService service)
        {
            _service = service;
        }

        public async Task<ModelResult> Handle(ProcessamentoImagemPutCommand command, CancellationToken cancellationToken = default)
        {
            return await _service.UpdateAsync(command.Entity, command.BusinessRules);
        }
    }
}
