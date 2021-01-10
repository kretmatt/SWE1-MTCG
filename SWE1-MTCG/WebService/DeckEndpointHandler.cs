using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class DeckEndpointHandler:IResourceEndpointHandler
    {
        private IUserRepository _userRepository;
        private ISessionRepository _sessionRepository;
        private ICardRepository _cardRepository;
        private List<RouteAction> RouteActions;
        private const string urlBase="/deck";
        public DeckEndpointHandler(IUserRepository userRepository, ISessionRepository sessionRepository, ICardRepository cardRepository)
        {
            _userRepository=userRepository;
            _sessionRepository = sessionRepository;
            _cardRepository = cardRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    ReadAllHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.GET
                ),
                new RouteAction(
                    ReadAllHandler,
                    String.Format(@"^\{0}\?format=plain",urlBase),
                    EHTTPVerbs.GET
                ),
                new RouteAction(
                    UpdateDeckHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.PUT
                )
            };
        }
        public bool CheckResponsibility(RequestContext requestContext)
        {
            return requestContext.URL.StartsWith(String.Format("{0}/",urlBase))||requestContext.URL==urlBase||requestContext.URL==String.Format("{0}?format=plain",urlBase);
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
            if(requestContext.URL.Contains("?format=plain"))
                return ResponseContext.OKResponse().SetContent(String.Concat(user.CardDeck), "text/plain");
            return ResponseContext.OKResponse().SetContent(JsonNet.Serialize(user.CardDeck), "application/json");
        }

        public ResponseContext UpdateDeckHandler(RequestContext requestContext)
        {
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");

            User user = _userRepository.Read(requestContext.HeaderPairs
                .SingleOrDefault(hp => hp.HeaderKey == "Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.BadRequestResponse().SetContent("User does not exist.", "text/plain");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");

            List<int> cardIds = JsonNet.Deserialize<List<int>>(requestContext.Body);
            
            if(cardIds.Count!=4)
                return ResponseContext.BadRequestResponse().SetContent("4 Ids need to be passed", "text/plain");
                
            List<ACard> cards = new List<ACard>();

            foreach (int id in cardIds)
            {
                ACard card = _cardRepository.Read(id);
                if(card!=null)
                    cards.Add(card);
            }
            
            if(cards.Count!=4)
                return ResponseContext.BadRequestResponse().SetContent("Some of the ids could not be found in the database!", "text/plain");


            int affectedRows = _userRepository.SetCardDeck(cards, user);
            
            if(affectedRows>=4)
                return ResponseContext.OKResponse().SetContent("Card deck was set.", "text/plain");

            return ResponseContext.BadRequestResponse().SetContent("Card deck could not be set!", "text/plain");
        }
        
        
        
        
    }
}