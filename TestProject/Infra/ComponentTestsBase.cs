namespace TestProject.Infra
{
    public class ComponentTestsBase : IDisposable
    {
        private readonly MongoTestFixture _mongoTestFixture;
        internal readonly ApiTestFixture _apiTest;
        private static int _tests = 0;

        public ComponentTestsBase()
        {
            _tests += 1;
            _mongoTestFixture = new MongoTestFixture(
                databaseContainerName: "mongodb-processamento-imagens-producao-component-test", port: "27021");
            _apiTest = new ApiTestFixture();
            Thread.Sleep(15000);
        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                _mongoTestFixture.Dispose();
                _apiTest.Dispose();
            }
        }
    }
}
