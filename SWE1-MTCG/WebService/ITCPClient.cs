using System;
using System.IO;
using System.Net.Sockets;

namespace SWE1_MTCG.WebService
{
    public interface ITCPClient:IDisposable
    {
        Stream GetStream();
        void Close();
    }
}