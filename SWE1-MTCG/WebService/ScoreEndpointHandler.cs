using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class ScoreEndpointHandler:AEndpointHandler
    {
         private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private IUserStatsRepository _userStatsRepository;

        public ScoreEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IUserStatsRepository userStatsRepository)
        {
            urlBase = "/score";
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            _userStatsRepository = userStatsRepository;
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
            
            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User does not exist.", "text/plain");

            List<UserStats> userStatsList = _userStatsRepository.ReadAll();
            
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(userStatsList),"application/json");
        }

    }
}