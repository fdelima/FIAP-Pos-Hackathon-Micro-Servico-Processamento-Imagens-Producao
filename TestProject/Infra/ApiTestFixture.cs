using System.Net.Http.Headers;

namespace TestProject.Infra
{
    public class ApiTestFixture : IDisposable
    {
        const string port = "5000";
        const string network = "network-pagamento-test";

        //api
        private const string ImageName = "fdelima/FIAP-Pos-Hackathon-Micro-Servico-Processamento-Imagens-Producao-gurpo-71-api:fase5";
        private const string DatabaseContainerName = "api-pagamento-test";
        private const string DataBaseName = "hackathon-microservico-processamento-imagens-producao-grupo-71";
        private HttpClient _client;

        public ApiTestFixture()
        {
            if (DockerManager.UseDocker())
            {
                if (!DockerManager.ContainerIsRunning(DatabaseContainerName))
                {
                    DockerManager.PullImageIfDoesNotExists(ImageName);
                    DockerManager.KillContainer(DatabaseContainerName);
                    DockerManager.KillVolume(DatabaseContainerName);

                    DockerManager.CreateNetWork(network);

                    DockerManager.RunContainerIfIsNotRunning(DatabaseContainerName,
                        $"run --name {DatabaseContainerName} " +
                        $"-e ASPNETCORE_ENVIRONMENT=Test " +
                        $"-p {port}:8082 " +
                        $"--network {network} " +
                        $"-d {ImageName}");

                    Thread.Sleep(3000);
                }
            }
        }

        public HttpClient GetClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"http://localhost:{port}/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _client;
        }

        public void Dispose()
        {
            if (DockerManager.UseDocker())
            {
                DockerManager.KillContainer(DatabaseContainerName);
                DockerManager.KillVolume(DatabaseContainerName);
            }
            GC.SuppressFinalize(this);
        }
    }
}
