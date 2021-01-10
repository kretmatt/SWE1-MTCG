using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;

namespace SWE1_MTCG.WebService
{
    public class SessionEndpointHandler:AEndpointHandler
    {
        private ISessionRepository _sessionRepository;
        public SessionEndpointHandler(ISessionRepository sessionRepository)
        {
            urlBase = "/sessions";
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    LoginHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.POST
                )
            };
        }

        public ResponseContext LoginHandler(RequestContext requestContext)
        {
            Console.WriteLine("Logging in!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");

            Dictionary<string,string> jsonDictionary = JsonNet.Deserialize<Dictionary<string, string>>(requestContext.Body);
            
            if(!(jsonDictionary.ContainsKey("Password")&&jsonDictionary.ContainsKey("Username")))
                return ResponseContext.BadRequestResponse().SetContent("Provided data was not sufficient for login.", "text/plain");

            if(_sessionRepository.Login(jsonDictionary["Username"],jsonDictionary["Password"]))
                return ResponseContext.OKResponse().SetContent("Login successful.", "text/plain");
            
            return ResponseContext.UnauthorizedResponse().SetContent("Login was unsuccessful","text/plain");
        }
    }
}