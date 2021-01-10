using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Json.Net;
using SWE1_MTCG.Battle;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.WebService
{
    public class BattleEndpointHandler:AEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private IBattleHistoryRepository _battleHistoryRepository;
        private IUserStatsRepository _userStatsRepository;
        private BattleSystem _battleSystem;
        
        
        public BattleEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IBattleHistoryRepository battleHistoryRepository, IUserStatsRepository userStatsRepository)
        {
            _battleSystem=new BattleSystem();
            urlBase = "/battles";
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            _userStatsRepository = userStatsRepository;
            _battleHistoryRepository = battleHistoryRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    BattleHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.POST
                )
            };
        }

        public ResponseContext BattleHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");

            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User does not exist.", "text/plain");
            
            if(user.CardDeck.Count!=4)
                return ResponseContext.BadRequestResponse().SetContent("Carddeck is not properly set.", "text/plain");
            
            user.CardStack=new List<ACard>();
            BattleSummary battleSummary = _battleSystem.DuelEnqueue(user).Result;
            if(battleSummary==null)
                return ResponseContext.BadRequestResponse().SetContent("Don't battle against yourself!", "text/plain");
            int affectedRows = 0;
            if (battleSummary.Loser == null && battleSummary.Victor == null)
            {
                affectedRows += _battleHistoryRepository.Create(user, EBattleResult.DRAW, 0);
                affectedRows += _userStatsRepository.Update(user);
            }else if (battleSummary.Loser.Id == user.Id)
            {
                affectedRows += _battleHistoryRepository.Create(user, EBattleResult.LOSS, -5);
                affectedRows += _userStatsRepository.Update(user);
            }else if (battleSummary.Victor.Id == user.Id)
            {
                affectedRows += _battleHistoryRepository.Create(user, EBattleResult.WIN, 2);
                affectedRows += _userStatsRepository.Update(user);
            }
            
            if(affectedRows!=2)
                return ResponseContext.BadRequestResponse().SetContent("Battlehistories and userstats could not be updated!", "text/plain");
            
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(battleSummary),"application/json");
        }
    }
}