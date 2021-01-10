using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SWE1_MTCG.WebService
{
    public abstract class AEndpointHandler:IResourceEndpointHandler
    {
        protected List<RouteAction> RouteActions=new List<RouteAction>();
        protected string urlBase="";
        
        
        public bool CheckResponsibility(RequestContext requestContext)
        {
            return requestContext.URL.StartsWith($"{urlBase}/")||requestContext.URL==urlBase||requestContext.URL==
                $"{urlBase}?format=plain";
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
    }
}