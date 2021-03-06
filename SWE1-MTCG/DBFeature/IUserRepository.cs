using System.Collections.Generic;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.DBFeature
{
    public interface IUserRepository
    {
        int Create(string username, string password, string bio);
        int Update(User user,string username, string bio);
        int Delete(string username);
        int Delete(int id);
        User Read(int id);
        User Read(string username);

        int SetCardDeck(List<ACard> cards, User user);

        int DeductCoins(int coinAmount, User user);

        bool CoinsDeductible(int coinAmount, User user);
    }
}