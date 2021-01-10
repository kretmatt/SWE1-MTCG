using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.WebService
{
    public class TradingEndpointHandler:AEndpointHandler
    {
        private ISessionRepository _sessionRepository;
        private IUserRepository _userRepository;
        private ICardRepository _cardRepository;
        private ITradingDealRepository _tradingDealRepository;
        
        public TradingEndpointHandler(ISessionRepository sessionRepository, IUserRepository userRepository, ICardRepository cardRepository, ITradingDealRepository tradingDealRepository)
        {
            urlBase = "/tradings";
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _cardRepository = cardRepository;
            _tradingDealRepository = tradingDealRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    CreateHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.POST
                ),
                new RouteAction(
                    ReadAllHandler,
                    $@"^\{urlBase}$",
                    EHTTPVerbs.GET
                ),
                new RouteAction(
                    TradeHandler,
                    $@"^\{urlBase}\/[0-9]+$",
                    EHTTPVerbs.POST
                ),
                new RouteAction(
                    DeleteHandler,
                    $@"^\{urlBase}\/[0-9]+$",
                    EHTTPVerbs.DELETE
                ),
            };
        }

        public ResponseContext CreateHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            
            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);

            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User does not exist.", "text/plain");

            Dictionary<string,int> jsonDictionary = JsonNet.Deserialize<Dictionary<string, int>>(requestContext.Body);
            
            
            if(!(jsonDictionary.ContainsKey("CardToTrade")&&jsonDictionary.ContainsKey("Type")&&jsonDictionary.ContainsKey("MinDamage")&&jsonDictionary.ContainsKey("RequiredCoins")))
                return ResponseContext.BadRequestResponse().SetContent("Insufficient data for creation of trading deal was sent to server", "text/plain");

            ACard offeredCard = _cardRepository.Read(jsonDictionary["CardToTrade"]);
            if(offeredCard==null)
                return ResponseContext.NotFoundResponse().SetContent("Card does not exist.", "text/plain");

            int affectedRows = _tradingDealRepository.Create(offeredCard, jsonDictionary["RequiredCoins"],
                jsonDictionary["MinDamage"], (ECardType) jsonDictionary["Type"], user);
            
            if(affectedRows!=1)
                return ResponseContext.BadRequestResponse().SetContent("Trading deal could not be created", "text/plain");
            return ResponseContext.OKResponse().SetContent("Trading deal created", "text/plain");

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

            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(_tradingDealRepository.ReadAll()),"application/json");
        }

        public ResponseContext DeleteHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            
            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);

            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User does not exist.", "text/plain");

            int id = Convert.ToInt32(requestContext.URL.Split('/')[2]);
            int affectedRows = _tradingDealRepository.Delete(id, user);
            
            if(affectedRows!=1)
                return ResponseContext.BadRequestResponse().SetContent("The trading deal could not be deleted!", "text/plain");
            return ResponseContext.OKResponse().SetContent("The trading deal was deleted.","text/plain");
        }

        public ResponseContext TradeHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");

            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");
            
            User user = _userRepository.Read(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue);

            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User does not exist.", "text/plain");

            int id = Convert.ToInt32(requestContext.URL.Split('/')[2]);
            TradingDeal tradingDeal = _tradingDealRepository.Read(id);
            if(tradingDeal==null)
                return ResponseContext.NotFoundResponse().SetContent("Trading deal does not exist.", "text/plain");
            
            if(tradingDeal.OfferingUser.Id==user.Id)
                return ResponseContext.BadRequestResponse().SetContent("You can't trade with yourself.", "text/plain");
            
            
            int affectedrows = 0;
            if (String.IsNullOrEmpty(requestContext.Body))
            {
                affectedrows = _tradingDealRepository.ConductTrade(tradingDeal, user);
            }else
            {
                int cardid = JsonNet.Deserialize<int>(requestContext.Body);
                ACard selectedCard = _cardRepository.Read(cardid);
                if(selectedCard==null)
                    return ResponseContext.NotFoundResponse().SetContent("Card doesn't exist.", "text/plain");

                affectedrows = _tradingDealRepository.ConductTrade(tradingDeal, user, selectedCard);
            }
            
            if(affectedrows<4)
                return ResponseContext.BadRequestResponse().SetContent("Could not conduct trade.", "text/plain");

            return ResponseContext.OKResponse().SetContent("Trade was conducted", "text/plain");
        }
    }
}