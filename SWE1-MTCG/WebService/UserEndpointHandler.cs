using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class UserEndpointHandler:IResourceEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private List<RouteAction> RouteActions;
        private const string urlBase="/users";
        public UserEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    CreateHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.POST
                    ),
                new RouteAction(
                    ReadHandler,
                    String.Format(@"^\{0}\/[a-zA-Z0-9]+$",urlBase),
                    EHTTPVerbs.GET
                    ),
                new RouteAction(
                    UpdateHandler,
                    String.Format(@"^\{0}\/[a-zA-Z0-9]+$",urlBase),
                    EHTTPVerbs.PUT
                    
                    )
            };
        }
        public bool CheckResponsibility(RequestContext requestContext)
        {
            return requestContext.URL.StartsWith(String.Format("{0}/",urlBase))||requestContext.URL==urlBase;
        }

        private RouteAction DetermineRouteAction(RequestContext requestContext)
        {
            RouteAction endpointAction=null;
            RouteActions.ForEach(ra =>
            {
                Regex re = new Regex(ra.PathRegex);
                
                if (re.IsMatch(requestContext.URL)&&ra.RequestType==requestContext.Type)
                    endpointAction = ra;
            });
            return endpointAction;
        }

        public ResponseContext HandleRequest(RequestContext requestContext)
        {
            RouteAction routeAction = DetermineRouteAction(requestContext);
            ResponseContext responseContext;
            if (routeAction != null)
                responseContext=routeAction.PathAction(requestContext);
            else
            {
                responseContext = ResponseContext.BadRequestResponse().SetContent("No fitting endpoint could be found!", "text/plain");
            }

            return responseContext;
        }
        public ResponseContext CreateHandler(RequestContext requestContext)
        {
            Console.WriteLine("Create user!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");

            Dictionary<string,string> jsonDictionary = JsonNet.Deserialize<Dictionary<string, string>>(requestContext.Body);
            
            if(!(jsonDictionary.ContainsKey("Password")&&jsonDictionary.ContainsKey("Username")))
                return ResponseContext.BadRequestResponse().SetContent("Provided data was not sufficient to create user.", "text/plain");

            int affectedRows = 0;

            if (jsonDictionary.ContainsKey("Bio"))
                affectedRows= _userRepository.Create(jsonDictionary["Username"], jsonDictionary["Password"], jsonDictionary["Bio"]);
            else
               affectedRows= _userRepository.Create(jsonDictionary["Username"], jsonDictionary["Password"], "");
            if(affectedRows!=2)
                return ResponseContext.BadRequestResponse().SetContent("Try again with other data!", "text/plain");
            return ResponseContext.CreatedResponse().SetContent("User was created.", "text/plain");;
        }

        public ResponseContext ReadHandler(RequestContext requestContext)
        {
            Console.WriteLine("Reading user data!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            string username = requestContext.URL.Split('/')[2];
            
            if(username!=requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue)
                return ResponseContext.BadRequestResponse().SetContent("You can only see your own profilepage. If you want to know more about other users, battle them or look at some trades.", "text/plain");

            User user = _userRepository.Read(username);
            
            if(user==null)
                return ResponseContext.BadRequestResponse().SetContent("User does not exist.", "text/plain");
            
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(user), "application/json");
        }

        public ResponseContext UpdateHandler(RequestContext requestContext)
        {
            Console.WriteLine("Updating user!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            string username = requestContext.URL.Split('/')[2];
            
            if(username!=requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue)
                return ResponseContext.BadRequestResponse().SetContent("You can only see your own profilepage. If you want to know more about other users, battle them or look at some trades.", "text/plain");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");

            Dictionary<string,string> jsonDictionary = JsonNet.Deserialize<Dictionary<string, string>>(requestContext.Body);
            
            if(!(jsonDictionary.ContainsKey("Username")&&jsonDictionary.ContainsKey("Bio")))
                return ResponseContext.BadRequestResponse().SetContent("Provided data was not sufficient to update user.", "text/plain");

            User user = _userRepository.Read(username);
            
            if(user==null)
                return ResponseContext.BadRequestResponse().SetContent("User does not exist.", "text/plain");



            int affectedRows = _userRepository.Update(user, jsonDictionary["Username"], jsonDictionary["Bio"]);
            
            if(affectedRows!=1)
                return ResponseContext.BadRequestResponse().SetContent("User could not be updated!", "text/plain");
                
            user.Bio = jsonDictionary["Bio"];
            user.Username = jsonDictionary["Username"];
            
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(user), "application/json");
        }
        
    }
}