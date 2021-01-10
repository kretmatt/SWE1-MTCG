namespace SWE1_MTCG.WebService
{
    /*
        ITcpListener - Used for mocking and used in the server instead of the "normal" TcpListener.
     */
    public interface ITCPListener
    {
        void Start();
        void Stop();
        ITCPClient AcceptTcpClient();
    }
}