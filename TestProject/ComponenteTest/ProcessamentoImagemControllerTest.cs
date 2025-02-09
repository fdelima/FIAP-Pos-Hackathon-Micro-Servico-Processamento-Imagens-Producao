using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TestProject.Infra;
using Xunit.Gherkin.Quick;

namespace TestProject.ComponenteTest
{
    [FeatureFile("./BDD/Features/ControlarProcessamentoImagens.feature")]
    public class ProcessamentoImagemControllerTest : Feature, IClassFixture<ComponentTestsBase>
    {
        private ModelResult expectedResult;
        ProcessamentoImagemUploadModel _ProcessamentoImagemUpload;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemControllerTest(ComponentTestsBase data)
        {
        }
        private class ActionResult
        {
            public List<string> Messages { get; set; }
            public List<string> Errors { get; set; }
            public ProcessamentoImagemUploadModel Model { get; set; }
            public bool IsValid { get; set; }
        }

        [Given(@"Prepando um ProcessamentoImagem")]
        public void PrepararProcessamentoImagem()
        {
            var msgSendModel = new ProcessamentoImagemSendQueueModel
            {
                IdProcessamentoImagem = Guid.NewGuid(),
                Usuario = "usuario_teste",
                NomeArquivo = "video_test.mp4",
                TamanhoArquivo = 1024,
                NomeArquivoZipDownload = "frames.zip"
            };

        }

        [And(@"Enviar o ProcessamentoImagem")]
        public async Task SendProcessamentoImagem()
        {
            throw new NotImplementedException();
        }

        [When(@"Receber o ProcessamentoImagem")]
        public async Task ReceiverProcessamentoImagem()
        {
            throw new NotImplementedException();
        }


        [Then(@"posso deletar o ProcessamentoImagem")]
        public async Task DeletarProcessamentoImagem()
        {
            throw new NotImplementedException();
        }
    }
}
