namespace TestProject.Infra
{
    public class WorkerTestFixture : IDisposable
    {
        private const string _imageName = "fdelima/fiap-pos-hackathon-micro-servico-processamento-imagens-producao-gurpo-71-worker:fase5";
        private const string _databaseContainerName = "worker-producao-test";

        public WorkerTestFixture()
        {
            if (DockerManager.UseDocker())
            {
                if (!DockerManager.ContainerIsRunning(_databaseContainerName))
                {
                    DockerManager.PullImageIfDoesNotExists(_imageName);
                    DockerManager.KillContainer(_databaseContainerName);
                    DockerManager.KillVolume(_databaseContainerName);

                    DockerManager.CreateNetWork();

                    DockerManager.RunContainerIfIsNotRunning(_databaseContainerName,
                        $"run --name {_databaseContainerName} " +
                        $"-e ASPNETCORE_ENVIRONMENT=Test " +
                        $"--network {DockerManager.NETWORK} " +
                        $"-d {_imageName}");

                    Thread.Sleep(3000);
                }
            }
        }

        public void Dispose()
        {
            if (DockerManager.UseDocker())
            {
                DockerManager.KillContainer(_databaseContainerName);
                DockerManager.KillVolume(_databaseContainerName);
            }
            GC.SuppressFinalize(this);
        }
    }
}
