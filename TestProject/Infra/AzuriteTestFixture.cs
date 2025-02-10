namespace TestProject.Infra
{
    public class AzuriteTestFixture : IDisposable
    {
        private const string ImageName = "mcr.microsoft.com/azure-storage/azurite:latest";

        string _port; string _databaseContainerName;

        public AzuriteTestFixture(string databaseContainerName)
        {
            if (DockerManager.UseDocker())
            {
                if (!DockerManager.ContainerIsRunning(databaseContainerName))
                {
                    _databaseContainerName = databaseContainerName;
                    DockerManager.PullImageIfDoesNotExists(ImageName);
                    DockerManager.KillContainer(databaseContainerName);
                    DockerManager.KillVolume(databaseContainerName);

                    DockerManager.CreateNetWork();

                    DockerManager.RunContainerIfIsNotRunning(databaseContainerName,
                        $"run --name {databaseContainerName} " +
                        $"-p 10000:10000 " +
                        $"-p 10001:10001 " +
                        $"-p 10002:10002 " +
                        $"-v azurite_data_test:/data " +
                        $"--network {DockerManager.NETWORK} " +
                        $"-d {ImageName}");

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
