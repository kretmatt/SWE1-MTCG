using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Json.Net;
using SWE1_MTCG.Battle;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;
using SWE1_MTCG.WebService;

namespace SWE1_MTCG
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BaseHTTPServer baseHttpServer = new BaseHTTPServer(10001);
            baseHttpServer.Start();
        }
    }
}