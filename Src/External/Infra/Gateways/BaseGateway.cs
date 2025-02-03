using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways
{
    /// <summary>
    /// Implementação dos Gateways, classe reponsavel efetivamente pela realização da ação no banco de dados.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseGateway<TEntity> : IGateways<TEntity> where TEntity : class, IDomainEntity
    {
        /// <summary>
        /// contexto
        /// </summary>
        protected readonly Context Ctx;

        /// <summary>
        /// Entidade relacional mapeada do banco de dados
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// Construtor da implementação da interface regulamentando os métodos que precisam ser impementados pelos repositórios
        /// </summary>
        /// <param name="ctx">Contexto</param>
        /// <param name="userContext">Informações de contexto do usuário</param>
        public BaseGateway(Context ctx)
        {
            Ctx = ctx;
            DbSet = Ctx.Set<TEntity>();
        }

        /// <summary>
        /// Insere o objeto no banco de dados
        /// </summary>
        /// <param name="entity">Objeto relacional do banco de dados mapeado</param>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntry = await DbSet.AddAsync(entity);
            return dbEntry.Entity;
        }

        /// <summary>
        /// Insere o objeto no banco de dados
        /// </summary>
        /// <param name="entity">Objeto relacional do banco de dados mapeado</param>
        public virtual async Task<T> InsertAsync<T>(T entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry dbEntry = await Ctx.AddAsync(entity);
            return (T)dbEntry.Entity;
        }

        /// <summary>
        /// Atualiza o objeto no banco de dados
        /// </summary>
        /// <param name="entity">Objeto relacional do banco de dados mapeado</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntry = await DbSet.AddAsync(entity);
            dbEntry.State = EntityState.Modified;
        }

        /// <summary>
        /// Atualiza o objeto no banco de dados
        /// </summary>
        /// <param name="entity">Objeto relacional do banco de dados mapeado</param>
        public virtual async Task UpdateAsync(TEntity oldEntity, TEntity NewEntity)
        {
            await Task.Run(() =>
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntry = Ctx.Entry(oldEntity);
                dbEntry.CurrentValues.SetValues(NewEntity);
                dbEntry.State = EntityState.Modified;
            });
        }

        /// <summary>
        /// Deleta os registro do banco de dados de acordo com a condição informada
        /// </summary>
        /// <param name="expression">condição</param>
        public virtual async Task DeleteasyncByExpression(Expression<Func<TEntity, bool>> expression)
        {
            TEntity? entity = await DbSet.FirstOrDefaultAsync(expression);
            Delete(entity);
        }

        /// <summary>
        /// Deleta o registro no banco de dados pelo id informado
        /// </summary>
        /// <param name="id">id do registro no banco de dados</param>
        public virtual async Task DeleteAsync(Guid id)
        {
            TEntity? entity = await DbSet.FindAsync(id);
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntry = DbSet.Remove(entity);
        }

        /// <summary>
        /// Deleta o objeto do banco de dados
        /// </summary>
        /// <param name="entity">Objeto relacional do banco de dados mapeado</param>
        public virtual void Delete(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> dbEntry = DbSet.Remove(entity);
        }

        /// <summary>
        /// Deleta os objetos do banco de dados
        /// </summary>
        /// <param name="entities">Objetos da entidade relacional do banco de dados mapeado</param>
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Efetivas as alterações salvando no banco de dados
        /// </summary>
        public async Task<int> CommitAsync()
        {
            try
            {
                return await Ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retorna o objeto do bd
        /// </summary>
        /// <param name="entity">Objeto relacional do bd mapeado</param>
        public virtual async ValueTask<TEntity?> FindByIdAsync(Guid Id)
            => await DbSet.FindAsync(Id);

        /// <summary>
        /// Retorna o objeto do bd com a tabela solicitada inclusa
        /// </summary>
        /// <param name="tableInclude">Tabela a ser incluida</param>
        /// <param name="whereExpression">Condição que filtra os itens a serem retornados</param>
        public virtual async ValueTask<TEntity?> FirstOrDefaultWithIncludeAsync<TEntityToInclude>(
            Expression<Func<TEntity, ICollection<TEntityToInclude>>> tableInclude,
            Expression<Func<TEntity, bool>> whereExpression)
            => await DbSet.Include(tableInclude).FirstOrDefaultAsync(whereExpression);

        /// <summary>
        /// Retorna a query de acordo com o filtro informado
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="filter">filtro</param>
        public IQueryable<TEntity> SortQuery(IQueryable<TEntity> query,
            IPagingQueryParam filter, Expression<Func<TEntity, object>> sortProp)
        {
            if (sortProp == null)
                return query;

            return filter.SortDirection == null
                            ? query.OrderBy(sortProp)
                            : filter.SortDirection.Equals("Desc", StringComparison.CurrentCultureIgnoreCase)
                                ? query.OrderByDescending(sortProp)
                                : query.OrderBy(sortProp);
        }

        /// <summary>
        /// Retorna os objetos do bd
        /// </summary>
        public virtual async ValueTask<PagingQueryResult<TEntity>> GetItemsAsync(IPagingQueryParam filter,
            Expression<Func<TEntity, object>> sortProp)
        {
            int count = await DbSet.CountAsync();
            List<TEntity> items;
            if (filter.CurrentPage > 0 && filter.Take > 0)
            {
                int skipItems = (filter.CurrentPage - 1) * filter.Take;
                items = await SortQuery(DbSet, filter, sortProp).Skip(skipItems).Take(filter.Take).AsNoTracking().ToListAsync();
            }
            else
            {
                items = await SortQuery(DbSet, filter, sortProp).AsNoTracking().ToListAsync();
            }

            return new PagingQueryResult<TEntity>(items, count, filter.Take);
        }

        /// <summary>
        /// Retorna os objetos que atendem a expression do bd
        /// </summary>
        /// <param name="whereExpression">Condição que filtra os itens a serem retornados</param>
        public virtual async ValueTask<PagingQueryResult<TEntity>> GetItemsAsync(IPagingQueryParam filter,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> sortProp)
        {
            if (filter == null) throw new InvalidOperationException("Necessário informar o filtro da consulta");

            int count = await DbSet.Where(whereExpression).CountAsync();
            List<TEntity> items;
            if (filter.CurrentPage > 0 && filter.Take > 0)
            {
                int skipItems = (filter.CurrentPage - 1) * filter.Take;
                items = await SortQuery(DbSet, filter, sortProp).AsNoTracking().Where(whereExpression).Skip(skipItems).Take(filter.Take).ToListAsync();
            }
            else
            {
                items = await SortQuery(DbSet, filter, sortProp).AsNoTracking().Where(whereExpression).ToListAsync();
            }

            return new PagingQueryResult<TEntity>(items, count, filter.Take);

        }

        /// <summary>
        /// Retorna os objetos que atendem a expression do bd
        /// </summary>
        /// <param name="whereExpression">Condição que filtra os itens a serem retornados</param>
        public virtual async ValueTask<PagingQueryResult<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            int count = await DbSet.Where(whereExpression).CountAsync();
            List<TEntity> items = await DbSet.AsNoTracking().Where(whereExpression).ToListAsync();

            return new PagingQueryResult<TEntity>(items, count, count);
        }

        /// <summary>
        /// Retorna os objetos que atendem a expression do bd
        /// </summary>
        /// <param name="expression">Condição que filtra os itens a serem retornados</param>
        public virtual async ValueTask<PagingQueryResult<TEntity>> GetItemsAsync()
        {
            int count = await DbSet.CountAsync();
            List<TEntity> items = await DbSet.AsNoTracking().ToListAsync();

            return new PagingQueryResult<TEntity>(items, count, count);
        }

        /// <summary>
        /// Retorna verdadeiro ou falso para expressão informada
        /// </summary>
        /// <param name="filter">condição</param>
        public async ValueTask<bool> Any(Expression<Func<IDomainEntity, bool>> filter)
            => await DbSet.AsNoTracking().AnyAsync(filter);


    }
}