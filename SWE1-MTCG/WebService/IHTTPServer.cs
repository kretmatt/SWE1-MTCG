namespace SWE1_MTCG.WebService
{
    public interface IHTTPServer
    {
        void Start();

        void HandleClient(ITCPClient tcpClient);
    }
}