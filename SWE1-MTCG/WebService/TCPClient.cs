using System.IO;

namespace SWE1_MTCG.WebService
{
    public class TCPClient : ITCPClient
    {
        private readonly System.Net.Sockets.TcpClient _client;
        public Stream GetStream() => _client.GetStream();

        public void Dispose() => _client.Dispose();
        public void Close() => _client.Close();

        public TCPClient(System.Net.Sockets.TcpClient tcpClient)
        {
            _client = tcpClient;
        }

        public TCPClient()
        {
            _client = new System.Net.Sockets.TcpClient();
        }
    }
}