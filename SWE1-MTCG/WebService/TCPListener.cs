using System.Net;

namespace SWE1_MTCG.WebService
{
    public class TCPListener:ITCPListener
    {
        private readonly System.Net.Sockets.TcpListener _tcpListener;
        public void Start() => _tcpListener.Start();
        public void Stop() => _tcpListener.Stop();
        public ITCPClient AcceptTcpClient()=>new TCPClient(_tcpListener.AcceptTcpClient());
        public TCPListener(IPAddress ipAddress, int port)
        {
            _tcpListener=new System.Net.Sockets.TcpListener(ipAddress,port);
        }
    }
}