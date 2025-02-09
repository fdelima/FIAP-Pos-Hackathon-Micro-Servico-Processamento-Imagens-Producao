using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.IoC;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage(Justification = "Arquivo de configuração")]
public class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        App.SetAtributesAppFromDll();

        // Add services to the container.
        builder.Services.AddHostedService(sp =>
        {
            return new QueueWorker(sp);
        }); // Adicione o seu Worker Service

        builder.Services.RegisterDependencies(builder.Configuration);

        var host = builder.Build();

        host.Run();
    }
}