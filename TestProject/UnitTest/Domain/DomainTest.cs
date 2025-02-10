using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.Text;

namespace TestProject.UnitTest.Domain
{
    public partial class DomainTest
    {
        [Fact]
        public void StringExtensionIsBlankTest()
        {
            //Arrange
            const string valor = " ";
            const bool expectedResult = true;
            //Act
            var result = StringExtension.IsBlank(valor);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void StringExtensionToSnakeCaseTest()
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
        public void ModelResultFactoryTest()
        {
            //Arrange
            var obj = new MessageModel
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageText = "Test",
                PopReceipt = Guid.NewGuid().ToString()
            };

            //Act / Assert
            var resut1 = ModelResultFactory.InsertSucessResult<MessageModel>(obj);
            Assert.Equal(resut1.Messages.First(), BusinessMessages.InsertSucess<MessageModel>());

            var resut2 = ModelResultFactory.UpdateSucessResult<MessageModel>(obj);
            Assert.Equal(resut2.Messages.First(), BusinessMessages.UpdateSucess<MessageModel>());
            
            var resut3 = ModelResultFactory.DeleteSucessResult<MessageModel>(obj);
            Assert.Equal(resut3.Messages.First(), BusinessMessages.DeleteSucess<MessageModel>());
           
            var resut4 = ModelResultFactory.DuplicatedResult<MessageModel>();
            Assert.Equal(resut4.Errors.First(), BusinessMessages.DuplicatedError<MessageModel>());
           
            var resut5 = ModelResultFactory.NotFoundResult<MessageModel>();
            Assert.Equal(resut5.Errors.First(), BusinessMessages.NotFoundError<MessageModel>());
           
            var resut6 = ModelResultFactory.DeleteFailResult<MessageModel>();
            Assert.Equal(resut6.Messages.First(), ErrorMessages.DeleteDatabaseError<MessageModel>());

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
            resut.AddMessage("addMessage");
            resut.AddMessage(new List<string> { "outra message" });

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
            resut.AddError("Erro");
            resut.AddError(new List<string> { "outro erro" });

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
