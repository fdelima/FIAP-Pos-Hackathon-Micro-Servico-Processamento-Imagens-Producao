using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Validator;
using FluentValidation;
using NSubstitute;
using System.Linq.Expressions;
using TestProject.MockData;

namespace TestProject.UnitTest.Domain
{
    /// <summary>
    /// Classe de teste.
    /// </summary>
    public partial class NotificacaoServiceTest
    {
        private readonly IValidator<Notificacao> _validator;
        private readonly IGateways<Notificacao> _notificacaoGatewayMock;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public NotificacaoServiceTest()
        {
            _validator = new NotificacaoValidator();
            _notificacaoGatewayMock = Substitute.For<IGateways<Notificacao>>();
        }

        /// <summary>
        /// Testa a inserção com dados válidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Inclusao, true, 3)]
        public async Task InserirComDadosValidos(string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                Usuario = usuario,
                Mensagem = mensagem
            };
            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Act
            var result = await domainService.InsertAsync(notificacao);

            //Assert
            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Testa a inserção com dados inválidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Inclusao, false, 3)]
        public async Task InserirComDadosInvalidos(string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                Usuario = usuario,
                Mensagem = mensagem
            };
            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Act
            var result = await domainService.InsertAsync(notificacao);

            //Assert
            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Testa a alteração com dados válidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 3)]
        public async Task AlterarComDadosValidos(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem
            };

            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Mockando retorno do método interno do UpdateAsync
            _notificacaoGatewayMock.FirstOrDefaultWithIncludeAsync(Arg.Any<Expression<Func<Notificacao, ICollection<object>>>>(), Arg.Any<Expression<Func<Notificacao, bool>>>()).
                Returns(new ValueTask<Notificacao>(notificacao));

            //Mockando retorno do método interno do UpdateAsync
            _notificacaoGatewayMock.UpdateAsync(Arg.Any<Notificacao>())
                .Returns(Task.FromResult(notificacao));

            //Act
            var result = await domainService.UpdateAsync(notificacao);

            //Assert
            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Testa a alteração com dados inválidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, false, 3)]
        public async Task AlterarComDadosInvalidos(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem
            };

            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Mockando retorno do método interno do UpdateAsync
            _notificacaoGatewayMock.FirstOrDefaultWithIncludeAsync(Arg.Any<Expression<Func<Notificacao, ICollection<object>>>>(), Arg.Any<Expression<Func<Notificacao, bool>>>()).
                Returns(new ValueTask<Notificacao>(notificacao));

            //Act
            var result = await domainService.UpdateAsync(notificacao);

            //Assert
            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Testa consulta por id 
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 3)]
        public async Task DeletarNotificacao(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem
            };

            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Mockando retorno do método interno do FindByAsync
            _notificacaoGatewayMock.FindByIdAsync(idNotificacao)
                .Returns(new ValueTask<Notificacao>(notificacao));

            _notificacaoGatewayMock.DeleteAsync(idNotificacao)
                .Returns(Task.FromResult(ModelResultFactory.SucessResult()));

            //Act
            var result = await domainService.DeleteAsync(idNotificacao);

            //Assert
            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Testa a consulta por id
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 3)]
        public async Task ConsultarNotificacaoPorIdComDadosValidos(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem
            };

            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            // Mockando retorno do método interno do FindByIdAsync
            _notificacaoGatewayMock.FindByIdAsync(idNotificacao)
                .Returns(new ValueTask<Notificacao>(notificacao));

            // Act
            var result = await domainService.FindByIdAsync(idNotificacao);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }

        /// <summary>
        /// Testa a consulta por id
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 3)]
        public async Task ConsultarNotificacaoPorIdComDadosInvalidos(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem
            };

            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Act
            var result = await domainService.FindByIdAsync(idNotificacao);

            //Assert
            Assert.False(result.IsValid);
        }

        /// <summary>
        /// Testa a consulta Valida
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Consulta, true, 3)]
        public async Task ConsultarNotificacao(IPagingQueryParam filter, Expression<Func<Notificacao, object>> sortProp, List<Notificacao> notificacoes)
        {
            ///Arrange
            var domainService = new NotificacaoService(_notificacaoGatewayMock, _validator);

            //Mockando retorno do metodo interno do GetItemsAsync
            _notificacaoGatewayMock.GetItemsAsync(Arg.Any<PagingQueryParam<Notificacao>>(),
                    Arg.Any<Expression<Func<Notificacao, object>>>())
                .Returns(new ValueTask<PagingQueryResult<Notificacao>>(new PagingQueryResult<Notificacao>(new List<Notificacao>(notificacoes))));

            //Act
            var result = await domainService.GetItemsAsync(filter, sortProp);

            //Assert
            Assert.True(result.Content.Any());
        }

        /// <summary>
        /// Testa a consulta com condição de pesquisa
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Consulta, true, 3)]
        public async Task ConsultarNotificacaoComCondicao(IPagingQueryParam filter, Expression<Func<Notificacao, object>> sortProp, List<Notificacao> notificacoes)
        {
            ///Arrange
            var param = new PagingQueryParam<Notificacao>() { CurrentPage = 1, Take = 10 };

            //Mockando retorno do metodo interno do GetItemsAsync
            _notificacaoGatewayMock.GetItemsAsync(Arg.Any<PagingQueryParam<Notificacao>>(),
                    Arg.Any<Expression<Func<Notificacao, bool>>>(),
                    Arg.Any<Expression<Func<Notificacao, object>>>())
                .Returns(new ValueTask<PagingQueryResult<Notificacao>>(new PagingQueryResult<Notificacao>(new List<Notificacao>(notificacoes))));

            //Act
            var result = await _notificacaoGatewayMock.GetItemsAsync(filter, param.ConsultRule(), sortProp);

            //Assert
            Assert.True(result.Content.Any());
        }

        /// <summary>
        /// Testa a consulta sem condição de pesquisa
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Consulta, true, 3)]
        public async Task ConsultarNotificacaoSemCondicao(IPagingQueryParam filter, Expression<Func<Notificacao, object>> sortProp, IEnumerable<Notificacao> notificacoes)
        {
            ///Arrange

            //Mockando retorno do metodo interno do GetItemsAsync
            _notificacaoGatewayMock.GetItemsAsync(filter, sortProp)
                .Returns(new ValueTask<PagingQueryResult<Notificacao>>(new PagingQueryResult<Notificacao>(new List<Notificacao>(notificacoes))));

            //Act
            var result = await _notificacaoGatewayMock.GetItemsAsync(filter, sortProp);

            //Assert
            Assert.True(result.Content.Any());
        }

        #region [ Xunit MemberData ]

        /// <summary>
        /// Mock de dados
        /// </summary>
        public static IEnumerable<object[]> ObterDados(enmTipo tipo, bool dadosValidos, int quantidade)
        {
            switch (tipo)
            {
                case enmTipo.Inclusao:
                    if (dadosValidos)
                        return NotificacaoMock.ObterDadosValidos(quantidade);
                    else
                        return NotificacaoMock.ObterDadosInvalidos(quantidade);
                case enmTipo.Alteracao:
                    if (dadosValidos)
                        return NotificacaoMock.ObterDadosValidos(quantidade)
                            .Select(i => new object[] { Guid.NewGuid() }.Concat(i).ToArray());
                    else
                        return NotificacaoMock.ObterDadosInvalidos(quantidade)
                            .Select(i => new object[] { Guid.NewGuid() }.Concat(i).ToArray());
                case enmTipo.Consulta:
                    if (dadosValidos)
                        return NotificacaoMock.ObterDadosConsultaValidos(quantidade);
                    else
                        return NotificacaoMock.ObterDadosConsultaInValidos(quantidade);
                default:
                    return null;
            }
        }

        #endregion
    }
}
