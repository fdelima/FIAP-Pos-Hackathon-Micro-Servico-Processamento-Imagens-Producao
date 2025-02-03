using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        /// <summary>
        /// Retorna os ProcessamentoImagems cadastrados
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<PagingQueryResult<ProcessamentoImagem>> Get(int currentPage = 1, int take = 10)
        {
            PagingQueryParam<ProcessamentoImagem> param = new PagingQueryParam<ProcessamentoImagem>() { CurrentPage = currentPage, Take = take };
            return await _controller.GetItemsAsync(param, param.SortProp());
        }

        /// <summary>
        /// Recupera o ProcessamentoImagem cadastrado pelo seu Id
        /// </summary>
        /// <returns>ProcessamentoImagem encontrada</returns>
        /// <response code="200">ProcessamentoImagem encontrada ou nulo</response>
        /// <response code="400">Erro ao recuperar ProcessamentoImagem cadastrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> FindById(Guid id)
        {
            return ExecuteCommand(await _controller.FindByIdAsync(id));
        }

        /// <summary>
        ///  Consulta os ProcessamentoImagems cadastrados no sistema com o filtro informado.
        /// </summary>
        /// <param name="filter">Filtros para a consulta dos ProcessamentoImagems</param>
        /// <returns>Retorna as ProcessamentoImagems cadastrados a partir dos parametros informados</returns>
        /// <response code="200">Listagem dos ProcessamentoImagems recuperada com sucesso</response>
        /// <response code="400">Erro ao recuperar listagem dos ProcessamentoImagems cadastrados</response>
        [HttpPost("consult")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<PagingQueryResult<ProcessamentoImagem>> Consult(PagingQueryParam<ProcessamentoImagem> param)
        {
            return await _controller.ConsultItemsAsync(param, param.ConsultRule(), param.SortProp());
        }

        /// <summary>
        /// Inseri o video .mp4 de até 500Mb para Processamento de Imagem cadastrado.
        /// </summary>
        /// <param name="model">Objeto contendo as informações para inclusão.</param>
        /// <returns>Retorna o result do ProcessamentoImagem cadastrado.</returns>
        /// <response code="200">ProcessamentoImagem inserida com sucesso.</response>
        /// <response code="400">Erros de validação dos parâmetros para inserção do ProcessamentoImagem.</response>
        [HttpPost]
        [RequestSizeLimit(Util.MaxUploadBytesRequest)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post(ProcessamentoImagemUploadModel model)
        {
            return ExecuteCommand(await _controller.PostAsync(model));
        }

        /// <summary>
        /// Altera o ProcessamentoImagem cadastrado.
        /// </summary>
        /// <param name="id">Identificador do ProcessamentoImagem cadastrado.</param>
        /// <param name="model">Objeto contendo as informações para modificação.</param>
        /// <returns>Retorna o result do ProcessamentoImagem cadastrado.</returns>
        /// <response code="200">ProcessamentoImagem alterada com sucesso.</response>
        /// <response code="400">Erros de validação dos parâmetros para alteração do ProcessamentoImagem.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put(Guid id, ProcessamentoImagem model)
        {
            return ExecuteCommand(await _controller.PutAsync(id, model));
        }

        /// <summary>
        /// Deleta o ProcessamentoImagem cadastrado.
        /// </summary>
        /// <param name="id">Identificador do ProcessamentoImagem cadastrado.</param>
        /// <returns>Retorna o result do ProcessamentoImagem cadastrado.</returns>
        /// <response code="200">ProcessamentoImagem deletada com sucesso.</response>
        /// <response code="400">Erros de validação dos parâmetros para deleção do ProcessamentoImagem.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return ExecuteCommand(await _controller.DeleteAsync(id));
        }

    }
}