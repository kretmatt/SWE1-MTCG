using System;

namespace SWE1_MTCG.WebService
{
    public class RouteAction
    {
        public RouteAction(Func<RequestContext,ResponseContext> pathAction, String pathRegex,EHTTPVerbs requestType)
        {
            PathAction = pathAction;
            PathRegex = pathRegex;
            RequestType = requestType;
        }
        
        public String PathRegex { get; set; }
        public Func<RequestContext,ResponseContext> PathAction{ get; set; }
        public EHTTPVerbs RequestType { get; set; }
    }
}