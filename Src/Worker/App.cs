using System.Net;
using System.Net.Sockets;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Api
{
    public class App
    {
        private static string _name = string.Empty;
        public static string Name { get { return _name; } }

        public static string Title { get { return $"{Name} ({GetLocalIPAddress()})"; } }

        private static string _version = string.Empty;
        public static string Version { get { return _version; } }

        public static void SetAtributesAppFromDll()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            _name = assembly.GetName().Name ?? "";

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            _version = lastModified.ToString("vyyyy.MM.dd.HHmm");
        }

        /// <summary>
        /// Retorna o Ip local
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return host.AddressList[0].ToString() ?? "";
        }

        /// <summary>
        /// Retorno a instancia de log.
        /// Para ser utilizado em classe staticas.
        /// </summary>
        internal static ILogger CreateLogger()
        {
            return new Logger<IHost>(LoggerFactory.Create(x => x.AddConsole()));
        }
    }
}
