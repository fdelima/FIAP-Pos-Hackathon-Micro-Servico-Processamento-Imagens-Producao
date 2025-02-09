using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;

namespace TestProject.UnitTest.Domain
{
    public partial class DomainTest
    {

        [Fact]
        public void StringExtensionTest()
        {
            //Arrange
            const string valor = "ToSnakeCaseTest";
            const string expectedResult = "to_snake_case_test";
            //Act
            var result = StringExtension.ToSnakeCase(valor);

            //Assert
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public void NoneResultTest()
        {
            //Arrange
            //Act
            var resut = ModelResultFactory.None();

            //Assert
            Assert.True(resut.ListErrors().Count() == 0);
            Assert.True(resut.ListMessages().Count() == 0);
        }

        [Fact]
        public void MessageResultTest()
        {
            //Arrange
            const string msg = "mensagem";

            //Act
            var resut = ModelResultFactory.Message(msg);

            //Assert
            Assert.Contains(msg, resut.ListMessages());
        }

        [Fact]
        public void ErrorResultTest()
        {
            //Arrange
            const string msg = "erro";

            //Act
            var resut = ModelResultFactory.Error(msg);

            //Assert
            Assert.Contains(msg, resut.ListErrors());
        }

        [Fact]
        public void ProcessamentoImagemProcessModelTest()
        {
            //Arrange
            Guid idProcessamentoImagem = Guid.NewGuid();
            string usuario = "usuario";
            DateTime dataInicioProcessamento = DateTime.Now;
            string nomeArquivo = "NomeArquivo";
            int tamanhoArquivo = 0;
            string nomeArquivoZipDownload = "NomeArquivoZipDownload.zip";

            //Act
            var resut = new ProcessamentoImagemProcessModel
            {
                IdProcessamentoImagem = idProcessamentoImagem,
                Usuario = usuario,
                DataFimProcessamento = dataInicioProcessamento,
                NomeArquivo = nomeArquivo,
                TamanhoArquivo = tamanhoArquivo,
                NomeArquivoZipDownload = nomeArquivoZipDownload
            };

            //Assert
            Assert.NotNull(resut);
        }

        [Fact]
        public void ProcessamentoImagemSendQueueModelTest()
        {
            //Arrange
            Guid idProcessamentoImagem = Guid.NewGuid();
            string usuario = "usuario";
            DateTime dataEnviadoFila = DateTime.Now;
            string nomeArquivo = "NomeArquivo";
            int tamanhoArquivo = 0;
            string nomeArquivoZipDownload = "NomeArquivoZipDownload.zip";

            //Act
            var resut = new ProcessamentoImagemSendQueueModel
            {
                IdProcessamentoImagem = idProcessamentoImagem,
                Usuario = usuario,
                DataEnviadoFila = dataEnviadoFila,
                NomeArquivo = nomeArquivo,
                TamanhoArquivo = tamanhoArquivo,
                NomeArquivoZipDownload = nomeArquivoZipDownload
            };

            //Assert
            Assert.NotNull(resut);
        }
        [Fact]
        public void ProcessamentoImagemUploadModelTest()
        {
            //Arrange
            string usuario = "usuario";
            DateTime data = DateTime.Now;
            IFormFile formFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("conteudo")), 0, Encoding.UTF8.GetBytes("conteudo").Length, "FormFile", "arquivo.txt");

            //Act
            var resut = new ProcessamentoImagemUploadModel
            {
                Usuario = usuario,
                Data = data,
                FormFile = formFile,
            };

            //Assert
            Assert.NotNull(resut);
        }
    }
}
