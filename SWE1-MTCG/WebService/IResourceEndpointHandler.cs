namespace SWE1_MTCG.WebService
{
    public interface IResourceEndpointHandler
    {
        bool CheckResponsibility(RequestContext requestContext);
        ResponseContext HandleRequest(RequestContext requestContext);
    }
}