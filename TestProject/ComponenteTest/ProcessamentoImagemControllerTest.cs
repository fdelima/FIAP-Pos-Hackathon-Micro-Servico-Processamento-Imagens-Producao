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
        private readonly WorkerTestFixture _workerTest;
        private ModelResult expectedResult;
        ProcessamentoImagemUploadModel _ProcessamentoImagemUpload;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemControllerTest(ComponentTestsBase data)
        {
            _workerTest = data._apiTest;
        }
        private class ActionResult
        {
            public List<string> Messages { get; set; }
            public List<string> Errors { get; set; }
            public ProcessamentoImagemUploadModel Model { get; set; }
            public bool IsValid { get; set; }
        }

        [Given(@"Recebendo um ProcessamentoImagem")]
        public void PrepararProcessamentoImagem()
        {
            _ProcessamentoImagemUpload = new ProcessamentoImagemUploadModel
            {
                Data = DateTime.Now,
                Usuario = "fiap@tech.com",
                FormFile = new FormFile(
                    baseStream: new MemoryStream(Encoding.UTF8.GetBytes("conteúdo do arquivo")),
                    baseStreamOffset: 0,
                    length: Encoding.UTF8.GetBytes("conteúdo do arquivo").Length,
                    name: "file",
                    fileName: "teste.txt"
                )
            };

        }

        [And(@"Adicionar o ProcessamentoImagem")]
        public async Task AdicionarProcessamentoImagem()
        {
            expectedResult = ModelResultFactory.InsertSucessResult<ProcessamentoImagemUploadModel>(_ProcessamentoImagemUpload);

            var client = _workerTest.GetClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            var filePath = "video_test.mp4";
            var fileBytes = File.ReadAllBytes(filePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("video/mp4");

            var conteudo = new MultipartFormDataContent();
            conteudo.Add(fileContent, "FormFile", "video_test.mp4");
            conteudo.Add(new StringContent(_ProcessamentoImagemUpload.Data.ToString("o")), "Data");
            conteudo.Add(new StringContent(_ProcessamentoImagemUpload.Usuario), "Usuario");

            HttpResponseMessage response = await client.PostAsync("api/ProcessamentoImagem", conteudo);

            var responseContent = await response.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<ActionResult>(responseContent);
            _ProcessamentoImagemUpload = actualResult.Model;

            Assert.Equal(expectedResult.IsValid, actualResult.IsValid);
            Assert.Equal(expectedResult.Messages, actualResult.Messages);
            Assert.Equal(expectedResult.Errors, actualResult.Errors);
        }

        [And(@"Encontrar o ProcessamentoImagem")]
        public async Task EncontrarProcessamentoImagem()
        {
            expectedResult = ModelResultFactory.SucessResult(_ProcessamentoImagemUpload);

            var client = _workerTest.GetClient();
            HttpResponseMessage response = await client.GetAsync(
                $"api/ProcessamentoImagem/{_ProcessamentoImagemUpload.Data}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<ActionResult>(responseContent);
            _ProcessamentoImagemUpload = actualResult.Model;

            Assert.Equal(expectedResult.IsValid, actualResult.IsValid);
            Assert.Equal(expectedResult.Messages, actualResult.Messages);
            Assert.Equal(expectedResult.Errors, actualResult.Errors);
        }

        [And(@"Alterar o ProcessamentoImagem")]
        public async Task AlterarProcessamentoImagem()
        {
            expectedResult = ModelResultFactory.UpdateSucessResult<ProcessamentoImagemUploadModel>(_ProcessamentoImagemUpload);

            var client = _workerTest.GetClient();
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/ProcessamentoImagem/{_ProcessamentoImagemUpload.Data}", _ProcessamentoImagemUpload);

            var responseContent = await response.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<ActionResult>(responseContent);
            _ProcessamentoImagemUpload = actualResult.Model;

            Assert.Equal(expectedResult.IsValid, actualResult.IsValid);
            Assert.Equal(expectedResult.Messages, actualResult.Messages);
            Assert.Equal(expectedResult.Errors, actualResult.Errors);
        }

        [When(@"Consultar o ProcessamentoImagem")]
        public async Task ConsultarProcessamentoImagem()
        {
            var client = _workerTest.GetClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"api/ProcessamentoImagem/consult", _ProcessamentoImagemUpload);

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic actualResult = JsonConvert.DeserializeObject(responseContent);

            Assert.True(actualResult.content != null);
        }


        [Then(@"posso deletar o ProcessamentoImagem")]
        public async Task DeletarProcessamentoImagem()
        {
            expectedResult = ModelResultFactory.DeleteSucessResult<ProcessamentoImagemUploadModel>();

            var client = _workerTest.GetClient();
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/ProcessamentoImagem/{_ProcessamentoImagemUpload.Data}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<ActionResult>(responseContent);
            _ProcessamentoImagemUpload = null;

            Assert.Equal(expectedResult.IsValid, actualResult.IsValid);
            Assert.Equal(expectedResult.Messages, actualResult.Messages);
            Assert.Equal(expectedResult.Errors, actualResult.Errors);
        }
    }
}
