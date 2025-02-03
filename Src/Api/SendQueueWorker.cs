using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using System.Text.Json;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api
{
    /// <summary>
    /// Serviço responsável por enviar os arquivos recebidos para fila de processamento
    /// Padrão de projeto Transactional outbox
    /// Saga Coreografada
    /// <see cref="https://microservices.io/patterns/data/transactional-outbox.html"/>
    /// <see cref="https://learn.microsoft.com/pt-br/azure/architecture/patterns/saga#choreography"/>
    /// </summary>
    public class SendQueueWorker : BackgroundService
    {
        IServiceProvider _serviceProvider;
        public SendQueueWorker(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _processamentoImagemController = scope.ServiceProvider.GetRequiredService<IProcessamentoImagemController>();
                    await Sender(_processamentoImagemController, stoppingToken);
                    await Receiver(_processamentoImagemController, stoppingToken);
                }
            }
        }

        private static async Task Sender(IProcessamentoImagemController _processamentoImagemController, CancellationToken stoppingToken)
        {

            try
            {
                Console.WriteLine("Worker Send Service executando...");
                var result = await _processamentoImagemController.SendMessageToQueueAsync();
                Console.WriteLine(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ops! Worker Send Service: " + ex.Message);
            }

            Console.WriteLine("Worker Send Service aguardando...");
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }

        private static async Task Receiver(IProcessamentoImagemController _processamentoImagemController, CancellationToken stoppingToken)
        {
            Console.WriteLine("Worker Receiver Service aguardando...");
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);

            try
            {
                Console.WriteLine("Worker Receiver Service executando...");
                var result = await _processamentoImagemController.ReceiverMessageInQueueAsync();
                Console.WriteLine(JsonSerializer.Serialize(result));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ops! Worker Receiver Service: " + ex.Message);
            }

        }
    }

}
