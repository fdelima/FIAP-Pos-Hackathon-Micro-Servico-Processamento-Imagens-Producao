using TestProject.Infra;
using Xunit.Gherkin.Quick;

namespace TestProject.ComponenteTest
{
    //TODO: Seguir como cliente no micro serviço cadastro
    [FeatureFile("./BDD/Features/ControlarProcessamentoImagens.feature")]
    public class ProcessamentoImagemControllerTest : Feature, IClassFixture<ComponentTestsBase>
    {
    }
}
