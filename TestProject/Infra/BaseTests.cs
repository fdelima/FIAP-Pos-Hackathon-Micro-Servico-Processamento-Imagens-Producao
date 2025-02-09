namespace TestProject.Infra
{
    public class BaseTests : IDisposable
    {
        private readonly AzuriteTestFixture _azuriteTestFixture;
        private static int _tests = 0;

        public BaseTests()
        {
            _tests += 1;

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
            }
        }
    }
}
