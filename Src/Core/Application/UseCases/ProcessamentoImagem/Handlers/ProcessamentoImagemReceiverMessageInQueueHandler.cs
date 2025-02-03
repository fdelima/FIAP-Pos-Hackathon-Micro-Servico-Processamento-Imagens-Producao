using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using MediatR;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Handlers
{
    public class ProcessamentoImagemReceiverMessageInQueueHandler : IRequestHandler<ProcessamentoImagemReceiverMessageInQueueCommand, ModelResult>
    {
        private readonly IProcessamentoImagemService _service;

        public ProcessamentoImagemReceiverMessageInQueueHandler(IProcessamentoImagemService service)
        {
            _service = service;
        }

        public async Task<ModelResult> Handle(ProcessamentoImagemReceiverMessageInQueueCommand command, CancellationToken cancellationToken = default)
        {
            return await _service.ReceiverMessageInQueueAsync();
        }
    }
}
