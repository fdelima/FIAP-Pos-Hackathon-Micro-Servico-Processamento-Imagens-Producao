using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using System.Text.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services
{
    public class ProcessamentoImagemService : IProcessamentoImagemService
    {
        protected readonly IMessagerService _messagerService;
        protected readonly IStorageService _storageService;

        /// <summary>
        /// Lógica de negócio referentes ao pedido.
        /// </summary>
        public ProcessamentoImagemService(IMessagerService messagerService, IStorageService storageService)
        {
            _messagerService = messagerService;
            _storageService = storageService;
        }

        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            var result = ModelResultFactory.SucessResult();

            var message = await _messagerService.ReceiveMessageAsync();

            try
            {
                if (message != null)
                {
                    var msgReceive = JsonSerializer.Deserialize<ProcessamentoImagemSendQueueModel>(message.MessageText);

                    //inicio do processamento
                    var msg = new ProcessamentoImagemProcessModel
                    {
                        IdProcessamentoImagem = msgReceive.IdProcessamentoImagem,
                        Usuario = msgReceive.Usuario,
                        DataInicioProcessamento = DateTime.Now,
                        NomeArquivo = msgReceive.NomeArquivo,
                        TamanhoArquivo = msgReceive.TamanhoArquivo,
                        NomeArquivoZipDownload = msgReceive.NomeArquivoZipDownload
                    };

                    //TODO: iniciar trabalho
                    await ExecuteProcess(msgReceive);


                    //Fim do processamento
                    msg.DataFimProcessamento = DateTime.Now;
                    await SendMessageToQueueAsync(msg);
                    await _messagerService.DeleteMessageAsync(message);

                    result = ModelResultFactory.SucessResult(msgReceive);
                }
            }
            catch (Exception ex)
            {
                result.AddError($"{ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Envia as mensagens dos arquivos recebidos para a fila.
        /// </summary>
        public async Task<ModelResult> SendMessageToQueueAsync(ProcessamentoImagemProcessModel msg)
        {
            await _messagerService.SendMessageAsync(JsonSerializer.Serialize(msg));
            return ModelResultFactory.SucessResult(msg);
        }

        private async Task ExecuteProcess(ProcessamentoImagemSendQueueModel msgReceive)
        {
            var tempPath = $"/app/temp/{msgReceive.IdProcessamentoImagem}";

            // Baixa o vídeo do blob para um arquivo temporário
            string tempVideoPath = await DownloadVideo(msgReceive, tempPath);

            // Obtém informações sobre o vídeo (duração em segundos)
            var duration = GetVideoDuration(tempVideoPath);

            //Captura os frames
            CaptureFrames(msgReceive, tempPath, tempVideoPath, duration);

            //Deleta o video baixado
            File.Delete(tempVideoPath);

            // Cria o arquivo zip
            await CreateZipFile(msgReceive, tempPath);
        }

        private async Task<string> DownloadVideo(ProcessamentoImagemSendQueueModel msgReceive, string tempPath)
        {
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);

            Directory.CreateDirectory(tempPath);

            var fileName = $"{msgReceive.IdProcessamentoImagem}{Path.GetExtension(msgReceive.NomeArquivo)}";
            string tempVideoPath = $"{tempPath}/{fileName}";

            await _storageService.DownloadFileAsync(Constants.BLOB_CONTAINER_NAME, fileName, tempVideoPath);
            return tempVideoPath;
        }

        private int GetVideoDuration(string tempVideoPath)
        {
            var durationProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffprobe",
                    Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {tempVideoPath}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            durationProcess.Start();
            var duration = (int)double.Parse(durationProcess.StandardOutput.ReadToEnd());
            durationProcess.WaitForExit();
            
            return duration;
        }

        private void CaptureFrames(ProcessamentoImagemSendQueueModel msgReceive, string tempPath, string tempVideoPath, int duration)
        {
            // Define o intervalo de captura
            var intervalo = duration >= 10 ? duration / 10 : 1;

            // Loop para capturar e processar os frames
            for (int i = 0; i < duration; i += intervalo)
            {
                // Define o tempo de captura do frame
                string currentTime = TimeSpan.FromSeconds(i).ToString(@"hh\:mm\:ss");

                // Comando FFmpeg para capturar o frame e aplicar filtros
                string ffmpegCommand = $"-ss {currentTime} -i {tempVideoPath} -vframes 1 {tempPath}/{msgReceive.IdProcessamentoImagem}_frame_{i}.jpg";

                // Executa o comando FFmpeg
                var ffmpegProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = ffmpegCommand,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                ffmpegProcess.Start();
                ffmpegProcess.WaitForExit();
            }
        }

        private async Task CreateZipFile(ProcessamentoImagemSendQueueModel msgReceive, string tempPath)
        {
            using (FileStream zipFileStream = new FileStream(msgReceive.NomeArquivoZipDownload, FileMode.Create))
            {
                ZipFile.CreateFromDirectory(tempPath, zipFileStream);
                await _storageService.UploadFileAsync(Constants.BLOB_CONTAINER_NAME, msgReceive.NomeArquivoZipDownload, zipFileStream);
            }

            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
        }
    }
}