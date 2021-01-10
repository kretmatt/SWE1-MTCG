using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class PackageEndpointHandler:IResourceEndpointHandler
    {
        private IPackageRepository _packageRepository;
        private ISessionRepository _sessionRepository;
        private IUserRepository _userRepository;
        private ICardRepository _cardRepository;
        private List<RouteAction> RouteActions;
        private const string urlBase="/packages";
        public PackageEndpointHandler(IPackageRepository packageRepository, ISessionRepository sessionRepository, IUserRepository userRepository, ICardRepository cardRepository)
        {
            _packageRepository = packageRepository;
            _cardRepository = cardRepository;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    CreateHandler,
                    String.Format(@"^\{0}$",urlBase),
                    EHTTPVerbs.POST
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
            Console.WriteLine("Create package!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");

            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Content-Type"&&hp.HeaderValue=="application/json")))
                return ResponseContext.BadRequestResponse().SetContent("Please send data with the following content type: application/json", "text/plain");
            if(String.IsNullOrEmpty(requestContext.Body))
                return ResponseContext.BadRequestResponse().SetContent("No data was sent to the server.", "text/plain");

            List<int> cardIds = JsonNet.Deserialize<List<int>>(requestContext.Body);
            
            if(cardIds.Count!=5)
                return ResponseContext.BadRequestResponse().SetContent("Not enough ids were defined!", "text/plain");
                
            List<ACard> cards = new List<ACard>();

            foreach (int id in cardIds)
            {
                ACard card = _cardRepository.Read(id);
                if(card!=null)
                    cards.Add(card);
            }
            
            if(cards.Count!=5)
                return ResponseContext.BadRequestResponse().SetContent("Some of the ids could not be found in the database!", "text/plain");

            int affectedRows = _packageRepository.CreatePackage(cards);

            if (affectedRows!=6)
                return ResponseContext.BadRequestResponse().SetContent("Corrupt package created", "text/plain");
            return ResponseContext.CreatedResponse().SetContent("Package was created.", "text/plain");
        }
    }
}