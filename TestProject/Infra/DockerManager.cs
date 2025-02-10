namespace TestProject.Infra
{
    internal class DockerManager
    {
        public const string NETWORK = "network-processamento-imagens-producao-test";
        public static void CreateNetWork()
        {
            var networkId = ProcessManager.ExecuteCommand("docker", $"network inspect -f '{{.Id}}' {NETWORK}");

            if (string.IsNullOrEmpty(networkId))
                ProcessManager.ExecuteCommand("docker", $"network create {NETWORK}");
        }
        public static bool ImageExists(string imageName, string version = "latest")
        {
            var imageId = ProcessManager.ExecuteCommand("docker", $"images -q {imageName}:{version}");

            if (string.IsNullOrEmpty(imageId))
                return false;

            return true;
        }

        public static void PullImageIfDoesNotExists(string imageName, string version = "latest")
        {
            if (!ImageExists(imageName, version))
                ProcessManager.ExecuteCommand("docker", $"pull {imageName}:{version}");
        }

        public static void KillContainer(string containerName)
            => ProcessManager.ExecuteCommand("docker", $"rm {containerName} -f");

        internal static void KillVolume(string volumeName)
            => ProcessManager.ExecuteCommand("docker", $"volume rm {volumeName} -f");

        public static void RunContainerIfIsNotRunning(string containerName, string command)
        {
            var containerId = ProcessManager.ExecuteCommand("docker", $"ps -q --filter name={containerName}");

            if (string.IsNullOrEmpty(containerId))
                ProcessManager.ExecuteCommand("docker", command);
        }

        public static bool ContainerIsRunning(string containerName)
        {
            var containerId = ProcessManager.ExecuteCommand("docker", $"ps -q --filter name={containerName}");

            if (string.IsNullOrEmpty(containerId))
                return false;

            return true;
        }

        public static bool UseDocker()
        {
            try
            {
                Console.WriteLine(ProcessManager.ExecuteCommand("docker", "-v"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
