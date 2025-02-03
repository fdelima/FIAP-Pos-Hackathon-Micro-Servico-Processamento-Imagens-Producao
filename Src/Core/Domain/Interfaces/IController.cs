using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    /// <summary>
    /// Interface regulamentando os métodos que precisam ser impementados pelos serviços da aplicação
    /// </summary>
    public interface IController<TEntity> where TEntity : IDomainEntity
    {
        /// <summary>
        /// Valida o objeto
        /// </summary>
        /// <param name="entity">Objeto relacional do bd mapeado</param>
        Task<ModelResult> ValidateAsync(TEntity entity);

        /// <summary>
        /// Insere o objeto
        /// </summary>
        /// <param name="entity">Objeto relacional do bd mapeado</param>
        Task<ModelResult> PostAsync(TEntity entity);

        /// <summary>
        /// Atualiza o objeto
        /// </summary>
        /// <param name="entity">Objeto relacional do bd mapeado</param>
        Task<ModelResult> PutAsync(Guid id, TEntity entity);

        /// <summary>
        /// Deleta o objeto
        /// </summary>
        /// <param name="id">id do objeto relacional do bd mapeado</param>
        Task<ModelResult> DeleteAsync(Guid id);

        /// <summary>
        /// Retorna o objeto do bd
        /// </summary>
        /// <param name="id">id do objeto relacional do bd mapeado</param>
        Task<ModelResult> FindByIdAsync(Guid id);

        /// <summary>
        /// Retorna os objetos do bd
        /// </summary>
        /// <param name="filter">filtro a ser aplicado</param>
        ValueTask<PagingQueryResult<TEntity>> GetItemsAsync(IPagingQueryParam filter, Expression<Func<TEntity, object>> sortProp);


        /// <summary>
        /// Retorna os objetos que atendem a expression do bd
        /// </summary>
        /// <param name="expression">Condição que filtra os itens a serem retornados</param>
        /// <param name="filter">filtro a ser aplicado</param>
        ValueTask<PagingQueryResult<TEntity>> ConsultItemsAsync(IPagingQueryParam param, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> sortProp);

    }
}
