namespace SWE1_MTCG
{
    public interface IUserRepository
    {
        int Create(string username, string password, string bio);
        int Update(User user,string username, string bio);
        int Delete(string username);
        int Delete(int id);
        User Read(int id);
        User Read(string username);
    }
}