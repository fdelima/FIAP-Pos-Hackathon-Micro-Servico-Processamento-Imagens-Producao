using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using MediatR;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands
{
    public class ProcessamentoImagemGetItemsCommand : IRequest<PagingQueryResult<Domain.Entities.ProcessamentoImagem>>
    {
        public ProcessamentoImagemGetItemsCommand(IPagingQueryParam filter, Expression<Func<Domain.Entities.ProcessamentoImagem, object>> sortProp)
        {
            Filter = filter;
            SortProp = sortProp;
        }

        public ProcessamentoImagemGetItemsCommand(IPagingQueryParam filter,
            Expression<Func<Domain.Entities.ProcessamentoImagem, bool>> expression, Expression<Func<Domain.Entities.ProcessamentoImagem, object>> sortProp)
            : this(filter, sortProp)
        {
            Expression = expression;
        }

        public IPagingQueryParam Filter { get; }
        public Expression<Func<Domain.Entities.ProcessamentoImagem, bool>> Expression { get; }

        public Expression<Func<Domain.Entities.ProcessamentoImagem, object>> SortProp { get; }
    }
}