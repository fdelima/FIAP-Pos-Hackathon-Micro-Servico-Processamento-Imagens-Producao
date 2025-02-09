namespace TestProject.Infra
{
    public class IntegrationTestsBase : BaseTests
    {
        private static int _tests = 0;

        public IntegrationTestsBase()
        {
            _tests += 1;
        }

        public void Dispose()
        {
            _tests -= 1;
            if (_tests == 0)
            {
                //coloque aqui os serviços a serem disposed
            }
        }
    }
}
