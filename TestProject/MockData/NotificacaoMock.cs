using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;

namespace TestProject.MockData
{
    /// <summary>
    /// Mock de dados das a��es
    /// </summary>
    public class NotificacaoMock
    {
        /// <summary>
        /// Mock de dados v�lidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Mensagem de teste",
                };
        }

        /// <summary>
        /// Mock de dados inv�lidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosInvalidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    Guid.Empty,
                    null
                };
        }

        /// <summary>
        /// Mock de dados v�lidos
        /// </summary>
        /// Mock de dados v�lidos para consulta
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
            {
                var notificacoes = new List<Notificacao>();
                for (var index2 = 1; index2 <= quantidade; index2++)
                {
                    notificacoes.Add(new Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        Data = DateTime.Now,
                        Mensagem = "Mensagem de teste",
                        Usuario = "usuario@test.com"
                    });
                }
                var param = new PagingQueryParam<Notificacao>() { CurrentPage = 1, Take = 10 };
                yield return new object[]
                {
                    param,
                    param.SortProp(),
                    notificacoes
                };
            }
        }

        /// <summary>
        /// Mock de dados inv�lidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaInValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
            {
                var notificacoes = new List<Notificacao>();
                for (var index2 = 1; index2 <= quantidade; index2++)
                {
                    notificacoes.Add(new Notificacao
                    {
                        IdNotificacao = Guid.NewGuid(),
                        Data = DateTime.Now,
                        Mensagem = "Mensagem de teste",
                        Usuario = string.Empty
                    });
                }
                var param = new PagingQueryParam<Notificacao>() { CurrentPage = 1, Take = 10 };
                yield return new object[]
                {
                    param,
                    param.SortProp(),
                    notificacoes
                };
            }
        }

        /// <summary>
        /// Mock de dados V�lidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaPorIdValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    Guid.NewGuid()
                };
        }

        /// <summary>
        /// Mock de dados inv�lidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaPorIdInvalidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    Guid.Empty
                };
        }
    }
}
