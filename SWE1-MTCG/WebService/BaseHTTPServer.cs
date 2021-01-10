using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SWE1_MTCG.DBFeature;

namespace SWE1_MTCG.WebService
{
    public class BaseHTTPServer : IHTTPServer

    {
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "FHTW SWE HTTP Server v1.0";

        private int port;
        private bool running = false;
        private ITCPListener listener;

        public List<IResourceEndpointHandler> ResourceEndpointHandlers { get; set; }

        public BaseHTTPServer(int port)
        {
            this.port = port;
            listener = new TCPListener(IPAddress.Any, this.port);
            ResourceEndpointHandlers = new List<IResourceEndpointHandler>();
            ResourceEndpointHandlers.Add(new UserEndpointHandler(new UserRepository(), new SessionRepository()));
            ResourceEndpointHandlers.Add(new SessionEndpointHandler(new SessionRepository()));
            ResourceEndpointHandlers.Add(new PackageEndpointHandler(new PackageRepository(), new SessionRepository(), new UserRepository(), new CardRepository()));
            ResourceEndpointHandlers.Add(new TransactionEndpointHandler(new PackageRepository(),new SessionRepository(), new UserRepository()));
            ResourceEndpointHandlers.Add(new CardsEndpointHandler(new UserRepository(), new SessionRepository()));
            ResourceEndpointHandlers.Add(new DeckEndpointHandler(new UserRepository(), new SessionRepository(),new CardRepository()));
            ResourceEndpointHandlers.Add(new StatsEndpointHandler(new UserRepository(), new SessionRepository(), new BattleHistoryRepository()));
            ResourceEndpointHandlers.Add(new ScoreEndpointHandler(new UserRepository(), new SessionRepository(), new UserStatsRepository()));
        }

        public void Start()
        {
            Console.WriteLine("Starting server on port {0}", port);
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();
            while (running)
            {
                Console.WriteLine("Waiting for connection ...");
                ITCPClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                Thread clientThread = new Thread(new ThreadStart(()=>HandleClient(client)));
                clientThread.Start();
            }

            running = false;
            listener.Stop();
        }


        public void HandleClient(ITCPClient client)
        {
            String msg = "";
            RequestContext requestContext;
            NetworkStream networkStream = (NetworkStream) client.GetStream();
            if (networkStream != null)
            {
                
                bool requestHandled = false;
                ResponseContext responseContext=ResponseContext.BadRequestResponse();
                responseContext.SetContent("The requested resource was not found!","text/plain");
                using (StreamReader streamReader = new StreamReader(networkStream))
                {
                    //Get first request line
                    requestContext = RequestContext.GetBaseRequest(streamReader.ReadLine());
                    //Get headers
                    while ((msg = streamReader.ReadLine()) != "")
                    {
                        requestContext.AddHeader(msg);
                    }

                    msg = "";
                    //Get body
                    while (streamReader.Peek() != -1)
                    {
                        msg += (char) streamReader.Read();
                    }

                    requestContext.Body = msg;
                    
                    Console.WriteLine("\n<-- REQUEST -->\n");
                    
                    Console.WriteLine(requestContext.ToString());

                    ResourceEndpointHandlers.ForEach(reh =>
                    {
                        if (reh.CheckResponsibility(requestContext))
                        {
                            responseContext=reh.HandleRequest(requestContext);
                        }
                    });
                    /* Due to "using" the streamreader and the underlying stream (in this case networkstream of tcpclient) are closed. Afterwards you can't access the stream anymore. That's why there is a using-Statement in another using-Statement */

                    using (StreamWriter streamWriter = new StreamWriter(client.GetStream()))
                        streamWriter.Write(responseContext.ToString());
                    
                    Console.WriteLine("\n<-- RESPONSE -->\n");
                    Console.WriteLine(responseContext.ToString());
                }
            }
            client.Close();
        }
    }
}