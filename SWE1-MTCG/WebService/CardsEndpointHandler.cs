using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class CardsEndpointHandler:AEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;

        public CardsEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            urlBase = "/cards";
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    ReadAllHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.GET
                    )
            };
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