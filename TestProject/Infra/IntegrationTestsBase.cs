namespace TestProject.Infra
{
    public class IntegrationTestsBase : IDisposable
    {
        private static int _tests = 0;

        public IntegrationTestsBase()
        {
            _tests += 1;
            Thread.Sleep(15000);
        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {

            }
        }
    }
}
