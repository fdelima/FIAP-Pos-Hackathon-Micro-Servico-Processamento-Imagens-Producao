
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace TestProject.MockData
{
    /// <summary>
    /// Mock de dados das ações
    /// </summary>
    public class ProcessamentoImagemSendQueueModelMock
    {
        /// <summary>
        /// Mock de dados válidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    $"usuario{index}@fiap.com.br",
                    DateTime.Now,
                    $"Nome Arquivo {index}",
                    1000,
                    $"Nome Arquivo Zip Download{index}"
                };
        }

        /// <summary>
        /// Mock de dados inválidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosInvalidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
                yield return new object[]
                {
                    string.Empty,
                    null,
                    null,
                    null,
                    null
                };
        }

        /// <summary>
        /// Mock de dados válidos
        /// </summary>
        /// Mock de dados válidos para consulta
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
            {
                var processamentoImagens = new List<ProcessamentoImagemSendQueueModel>();
                for (var index2 = 1; index2 <= quantidade; index2++)
                {
                    processamentoImagens.Add(new ProcessamentoImagemSendQueueModel
                    {
                        IdProcessamentoImagem = Guid.NewGuid(),
                        Usuario = $"usuario{index}@fiap.com.br",
                        DataEnviadoFila = DateTime.Now,
                        NomeArquivo = $"Nome Arquivo {index}",
                        TamanhoArquivo = 1000,
                        NomeArquivoZipDownload = $"Nome Arquivo Zip Download{index}"
                    }); 
                }
                yield return new object[]
                {
                    processamentoImagens
                };
            }
        }

        /// <summary>
        /// Mock de dados inválidos
        /// </summary>
        public static IEnumerable<object[]> ObterDadosConsultaInValidos(int quantidade)
        {
            for (var index = 1; index <= quantidade; index++)
            {
                var processamentoImagens = new List<ProcessamentoImagemSendQueueModel>();
                for (var index2 = 1; index2 <= quantidade; index2++)
                {
                    processamentoImagens.Add(new ProcessamentoImagemSendQueueModel
                    {
                        IdProcessamentoImagem = Guid.NewGuid(),
                        Usuario = $"usuario{index}@fiap.com.br",
                        DataEnviadoFila = DateTime.Now,
                        NomeArquivo = $"Nome Arquivo {index}",
                        TamanhoArquivo = 1000,
                        NomeArquivoZipDownload = $"Nome Arquivo Zip Download{index}"
                    });
                }
                yield return new object[]
                {
                    processamentoImagens
                };
            }
        }

        /// <summary>
        /// Mock de dados Válidos
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
        /// Mock de dados inválidos
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
