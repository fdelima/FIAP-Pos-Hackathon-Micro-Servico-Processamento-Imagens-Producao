namespace TestProject.Infra
{
    public class ComponentTestsBase : BaseTests
    {
        internal readonly WorkerTestFixture _workerTest;
        private static int _tests = 0;

        public ComponentTestsBase()
        {
            _tests += 1;

            _workerTest = new WorkerTestFixture();

            Thread.Sleep(15000);

        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                _workerTest.Dispose();
            }
        }
    }
}
