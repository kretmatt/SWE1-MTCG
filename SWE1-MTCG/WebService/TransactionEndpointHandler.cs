using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Json.Net;
using SWE1_MTCG.DBFeature;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.WebService
{
    public class TransactionEndpointHandler:AEndpointHandler
    {
        private IPackageRepository _packageRepository;
        private ISessionRepository _sessionRepository;
        private IUserRepository _userRepository;

        public TransactionEndpointHandler(IPackageRepository packageRepository, ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            urlBase = "/transactions";
            _packageRepository = packageRepository;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            RouteActions = new List<RouteAction>
            {
                new RouteAction(
                    OpenPackageHandler,
                    $@"^\{urlBase}\/packages$",
                    EHTTPVerbs.POST
                    )
            };
        }

        public ResponseContext OpenPackageHandler(RequestContext requestContext)
        {
            Console.WriteLine("Opening package!");
            
            if(!(requestContext.HeaderPairs.Exists(hp=>hp.HeaderKey=="Authorization")))
                return ResponseContext.UnauthorizedResponse().SetContent("Appropriate credentials are missing.", "text/plain");
            if(!_sessionRepository.CheckIfInValidSession(requestContext.HeaderPairs.SingleOrDefault(hp=>hp.HeaderKey=="Authorization").HeaderValue))
                return ResponseContext.UnauthorizedResponse().SetContent("There is no valid session for the token used.", "text/plain");

            User user = _userRepository.Read(requestContext.HeaderPairs
                .SingleOrDefault(hp => hp.HeaderKey == "Authorization").HeaderValue);
            
            if(user==null)
                return ResponseContext.NotFoundResponse().SetContent("User could not be loaded!", "text/plain");
            
            int affectedRows = _packageRepository.OpenPackage(user);

            if (affectedRows!=7)
                return ResponseContext.BadRequestResponse().SetContent("Package could not be opened!", "text/plain");
            return ResponseContext.CreatedResponse().SetContent("Package was successfully opened.", "text/plain");
        }
    }
}