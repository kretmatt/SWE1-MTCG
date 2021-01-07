namespace SWE1_MTCG
{
    public interface ISessionRepository
    {
        bool Login(string username, string password);
        bool CheckIfInValidSession(string username);
        bool Logout(string username);
    }
}