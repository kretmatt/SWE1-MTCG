using System.Collections.Generic;

namespace SWE1_MTCG
{
    public interface ICardRepository
    {
        List<ACard> LoadCardDeck(User user);
        List<ACard> LoadCardStack(User user);
        List<ACard> LoadPackage(int packageid);
        int CreateMonsterCard(string name, string description,double damage,EElementalType elementalType,EMonsterType monsterType);
        int CreateSpellCard(string name, string description,double damage,EElementalType elementalType);
        int CreateAreaCard(string name, string description,double damage,EElementalType elementalType, int uses);
        int CreateServantCard(string name, string description,double damage,EElementalType elementalType, EServantClass servantClass);
        int CreateTrapCard(string name, string description,double damage,EElementalType elementalType, int uses);
        int Delete(int id);

        ACard Read(int id);

        List<ACard> ReadAll();

        //ACARD = name, description, damage, elementaltype, cardtype,
        //spellcard = - 
        //monster card = monstertype
        //area & trap card = uses
    }
}