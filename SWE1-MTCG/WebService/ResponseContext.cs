using System;
using System.Collections.Generic;

namespace SWE1_MTCG.WebService
{
    public class ResponseContext
    {
        public String HTTPVersion { get; set; }
        public String StatusMessage { get; set; }
        public int StatusCode { get; set; }
        public List<HttpHeaderPair> HeaderPairs { get; private set; }
        public String Content { get; private set; }

        private ResponseContext(String httpVersion, String statusMessage, int statusCode)
        {
            HTTPVersion = httpVersion;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            HeaderPairs = new List<HttpHeaderPair>();
            AddHeader(new HttpHeaderPair("Date", DateTime.Now.ToString("r")));
            AddHeader(new HttpHeaderPair("Server", BaseHTTPServer.NAME));
        }

        public static ResponseContext OKResponse()
        {
            return new ResponseContext(BaseHTTPServer.VERSION, "OK", 200);
        }
        public static ResponseContext CreatedResponse()
        {
            return new ResponseContext(BaseHTTPServer.VERSION, "Created", 201);
        }
        public static ResponseContext BadRequestResponse()
        {
            return new ResponseContext(BaseHTTPServer.VERSION, "Bad Request", 400);
        }
        public static ResponseContext NotFoundResponse()
        {
            return new ResponseContext(BaseHTTPServer.VERSION, "Not Found", 404);
        }

        public static ResponseContext UnauthorizedResponse()
        {
            return new ResponseContext(BaseHTTPServer.VERSION, "Unauthorized", 401);
        }
        
        public ResponseContext AddHeader(HttpHeaderPair httpHeaderPair)
        {
            HeaderPairs.Add(httpHeaderPair);
            return this;
        }

        // SetContent sets the content and the corresponding headers for the response. Is an extension method.
        public ResponseContext SetContent(String content, String contentType)
        {
            if (!String.IsNullOrEmpty(content))
            {
                Content = content;
                HeaderPairs.RemoveAll(hp=>hp.HeaderKey=="Content-Length"||hp.HeaderKey=="Content-Type");
                return AddHeader(new HttpHeaderPair(
                    "Content-Length",
                    System.Text.Encoding.ASCII.GetByteCount(Content).ToString()
                )).AddHeader(new HttpHeaderPair(
                    "Content-Type",
                    contentType
                ));
            }

            return this;
        }

        public override string ToString()
        {
            string responseString = String.Format("{0} {1} {2}\n", HTTPVersion, StatusCode, StatusMessage);
            HeaderPairs.ForEach(hp => responseString += String.Format("{0}\n", hp.ToString()));
            if (!String.IsNullOrEmpty(Content))
                responseString = String.Format("{0}\n{1}\n\n", responseString, Content);
            return responseString;
        }
    }
}