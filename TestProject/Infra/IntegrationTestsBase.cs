namespace TestProject.Infra
{
    public class IntegrationTestsBase : IDisposable
    {
        internal readonly MongoTestFixture _mongoTestFixture;
        private static int _tests = 0;

        public IntegrationTestsBase()
        {
            _tests += 1;
            _mongoTestFixture = new MongoTestFixture(
                databaseContainerName: "mongo-db-pagamento-test", port: "27019");
            Thread.Sleep(15000);
        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                _mongoTestFixture.Dispose();
            }
        }
    }
}
