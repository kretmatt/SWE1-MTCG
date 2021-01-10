using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class CardsEndpointHandler:IResourceEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private List<RouteAction> RouteActions;
        private const string urlBase="/cards";
        public CardsEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    ReadAllHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.GET
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


        public ResponseContext ReadAllHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");

            User user = _userRepository.Read(requestContext.HeaderPairs
                .SingleOrDefault(hp => hp.HeaderKey == "Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.BadRequestResponse().SetContent("User does not exist.", "text/plain");
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(user.CardStack), "application/json");
        }
    }
}