namespace TestProject.Infra
{
    public class ComponentTestsBase : IDisposable
    {
        internal readonly WorkerTestFixture _apiTest;
        private static int _tests = 0;

        public ComponentTestsBase()
        {
            _tests += 1;
            _apiTest = new WorkerTestFixture();
            Thread.Sleep(15000);
        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                _apiTest.Dispose();
            }
        }
    }
}
