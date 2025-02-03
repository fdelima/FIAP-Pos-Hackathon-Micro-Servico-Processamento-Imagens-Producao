using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api.Controllers
{
    //TODO: Controller :: 1 - Duplicar esta controller de exemplo e trocar o nome da entidade.
    /// <summary>
    /// Controller dos ProcessamentoImagems cadastrados
    /// </summary>
    [Route("api/[Controller]")]
    public class ProcessamentoImagemController : ApiController
    {
        private readonly IProcessamentoImagemController _controller;

        /// <summary>
        /// Construtor do controller dos ProcessamentoImagems cadastrados
        /// </summary>
        public ProcessamentoImagemController(IProcessamentoImagemController controller)
        {
            _controller = controller;
        }
    }
}