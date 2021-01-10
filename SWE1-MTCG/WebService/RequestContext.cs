using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace SWE1_MTCG.WebService
{
    public class RequestContext
    {
        public EHTTPVerbs Type { get; set; }
        public String URL { get; set; }
        public String HTTPVersion { get; set; }
        public List<HttpHeaderPair> HeaderPairs { get; private set; }
        public String Body { get; set; }
        
        private RequestContext(EHTTPVerbs type,String url,String httpVersion)
        {
            Type = type;
            URL = url;
            HTTPVersion = httpVersion;
            HeaderPairs = new List<HttpHeaderPair>();
        }

        public void AddHeader(String headerLine)
        {
            String[] headerPairParts = headerLine.Split(' ');
            if(headerPairParts[0].Trim(':')=="Authorization"&&headerPairParts[1]=="Basic")
                HeaderPairs.Add(new HttpHeaderPair(headerPairParts[0].Trim(':'),headerPairParts[2].Split('-')[0]));
            else
                HeaderPairs.Add(new HttpHeaderPair(headerPairParts[0].Trim(':'),headerPairParts[1]));
        }
        
        public static RequestContext GetBaseRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
                return null;

            String[] tokens = request.Split(' ');
            String type = tokens[0];
            String url = tokens[1];
            String httpVersion = tokens[2];

            EHTTPVerbs requestType;
            if (!EHTTPVerbs.TryParse(type, out requestType))
                return null;
            return new RequestContext(requestType,url,httpVersion);
        }

        public override string ToString()
        {
            string requestString = String.Format("{0} {1} {2}\n",Type,URL,HTTPVersion);
            HeaderPairs.ForEach(hp=> requestString+=String.Format("{0}\n",hp.ToString()));
            return String.Format("{0}\n{1}\n",requestString,Body);
        }
    }
}