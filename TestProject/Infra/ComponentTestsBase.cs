namespace TestProject.Infra
{
    public class ComponentTestsBase : IDisposable
    {
        private readonly AzuriteTestFixture _azuriteTestFixture;
        private readonly WorkerTestFixture _workerTest;
        private static int _tests = 0;

        public ComponentTestsBase()
        {
            _tests += 1;

            _workerTest = new WorkerTestFixture();

            _azuriteTestFixture = new AzuriteTestFixture(
                databaseContainerName: "azurite-processamento-imagens-producao-component-test");

            Thread.Sleep(15000);

        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                _azuriteTestFixture.Dispose();
                _workerTest.Dispose();
            }
        }
    }
}
