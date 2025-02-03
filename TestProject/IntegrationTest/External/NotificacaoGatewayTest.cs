using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways;
using System.Linq.Expressions;
using TestProject.Infra;
using TestProject.MockData;

namespace TestProject.IntegrationTest.External
{
    /// <summary>
    /// Classe de teste.
    /// </summary>
    public partial class NotificacaoGatewayTest : IClassFixture<IntegrationTestsBase>
    {
        internal readonly MongoTestFixture _mongoTestFixture;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public NotificacaoGatewayTest(IntegrationTestsBase data)
        {
            _mongoTestFixture = data._mongoTestFixture;
        }

        /// <summary>
        /// Testa a inserção com dados válidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Inclusao, true, 3)]
        public async void InserirComDadosValidos(string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = Guid.NewGuid(),
                Usuario = usuario,
                Mensagem = mensagem,
                Data = DateTime.Now
            };

            //Act
            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());
            var result = await _notificacaoGateway.InsertAsync(notificacao);

            //Assert
            try
            {
                await _notificacaoGateway.CommitAsync();
                Assert.True(true);
            }
            catch (InvalidOperationException)
            {
                Assert.True(false);
            }
        }

        /// <summary>
        /// Testa a alteração com dados válidos
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 3)]
        public async void AlterarComDadosValidos(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem,
                Data = DateTime.Now
            };

            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());
            var result = await _notificacaoGateway.InsertAsync(notificacao);
            await _notificacaoGateway.CommitAsync();

            //Alterando
            notificacao.Mensagem = mensagem + " ALTERADA !!! ";

            var dbEntity = await _notificacaoGateway.FindByIdAsync(idNotificacao);

            //Act
            await _notificacaoGateway.UpdateAsync(dbEntity, notificacao);
            await _notificacaoGateway.UpdateAsync(notificacao);

            try
            {
                await _notificacaoGateway.CommitAsync();
                Assert.True(true);
            }
            catch (InvalidOperationException)
            {
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        /// <summary>
        /// Testa a deletar por id
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 1)]
        public async void DeletarNotificacao(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem,
                Data = DateTime.Now
            };

            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());
            await _notificacaoGateway.InsertAsync(notificacao);
            await _notificacaoGateway.CommitAsync();

            //Act
            await _notificacaoGateway.DeleteAsync(idNotificacao);

            //Assert
            try
            {
                await _notificacaoGateway.CommitAsync();
                Assert.True(true);
            }
            catch (InvalidOperationException)
            {
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        /// <summary>
        /// Testa a consulta por id
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Alteracao, true, 1)]
        public async void ConsultarNotificacaoPorId(Guid idNotificacao, string usuario, string mensagem)
        {
            ///Arrange
            var notificacao = new Notificacao
            {
                IdNotificacao = idNotificacao,
                Usuario = usuario,
                Mensagem = mensagem,
                Data = DateTime.Now
            };

            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());
            await _notificacaoGateway.InsertAsync(notificacao);
            await _notificacaoGateway.CommitAsync();

            //Act
            var result = await _notificacaoGateway.FindByIdAsync(idNotificacao);

            //Assert
            Assert.True(result != null);
        }

        /// <summary>
        /// Testa a consulta Valida
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Consulta, true, 3)]
        public async Task ConsultarNotificacao(IPagingQueryParam filter, Expression<Func<Notificacao, object>> sortProp, IEnumerable<Notificacao> notificacoes)
        {
            ///Arrange
            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());

            foreach (var notificacao in notificacoes)
            {
                await _notificacaoGateway.InsertAsync(notificacao);
                await _notificacaoGateway.CommitAsync();
            }

            //Act
            var result = await _notificacaoGateway.GetItemsAsync(filter, sortProp);

            //Assert
            Assert.True(result.Content.Any());
        }

        /// <summary>
        /// Testa a consulta com condição de pesquisa
        /// </summary>
        [Theory]
        [MemberData(nameof(ObterDados), enmTipo.Consulta, true, 3)]
        public async Task ConsultarNotificacaoComCondicao(IPagingQueryParam filter, Expression<Func<Notificacao, object>> sortProp, IEnumerable<Notificacao> notificacoes)
        {
            ///Arrange
            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());

            foreach (var notificacao in notificacoes)
            {
                await _notificacaoGateway.InsertAsync(notificacao);
                await _notificacaoGateway.CommitAsync();
            }

            var param = new PagingQueryParam<Notificacao>() { CurrentPage = 1, Take = 10, ObjFilter = notificacoes.ElementAt(0) };

            //Act
            var result = await _notificacaoGateway.GetItemsAsync(filter, param.ConsultRule(), sortProp);

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
            var _notificacaoGateway = new BaseGateway<Notificacao>(_mongoTestFixture.GetDbContext());

            foreach (var notificacao in notificacoes)
            {
                await _notificacaoGateway.InsertAsync(notificacao);
                await _notificacaoGateway.CommitAsync();
            }

            //Act
            var result = await _notificacaoGateway.GetItemsAsync(filter, sortProp);

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
