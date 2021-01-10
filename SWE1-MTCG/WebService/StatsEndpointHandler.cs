using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class StatsEndpointHandler:AEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private IBattleHistoryRepository _battleHistoryRepository;

        public StatsEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IBattleHistoryRepository battleHistoryRepository)
        {
            urlBase = "/stats";
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            _battleHistoryRepository = battleHistoryRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    ReadAllForUserHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.GET
                )
            };
        }

        public ResponseContext ReadAllForUserHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            
            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.BadRequestResponse().SetContent("User does not exist.", "text/plain");
            
            List<BattleHistory> battleHistories = _battleHistoryRepository.ReadAll(user);
            
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(battleHistories), "application/json");
        }
    }
}