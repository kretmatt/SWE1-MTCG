using System;
using System.Collections.Generic;
using SWE1_MTCG.DTOs;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.DBFeature
{
    public class CardRepository:ICardRepository
    {
        private MTCGDatabaseConnection _mtcgDatabaseConnection;
        
        public CardRepository()
        {
            _mtcgDatabaseConnection = MTCGDatabaseConnection.ReturnMTCGDatabaseConnection();
        }

        private ACard ConvertToCard(object[] row)
        {
            ACard card = null;
            ECardType cardType = (ECardType) Convert.ToInt32(row[5]);
            int id = Convert.ToInt32(row[0]);
            string name = row[1].ToString();
            double damage = Convert.ToDouble(row[2]);
            string description = row[3].ToString();
            EElementalType elementalType = (EElementalType) Convert.ToInt32(row[4]);
            switch (cardType)
            {
                case ECardType.AREA:
                    
                    INpgsqlCommand readAreaCardDataCommand = new NpsqlCommand("SELECT * FROM areacard WHERE id=@id;");
                    readAreaCardDataCommand.Parameters.AddWithValue("id",id);
                    List<object[]> readAreaCardDataResults =
                        _mtcgDatabaseConnection.QueryDatabase(readAreaCardDataCommand);
                    
                    if(readAreaCardDataResults.Count!=1)
                        break;

                    int areaUses = Convert.ToInt32(readAreaCardDataResults[0][1]);
                    
                    card=new AreaCard
                    {
                        Id=id,
                        Name = name,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        CardType = cardType,
                        Uses = areaUses
                    };
                    break;
                case ECardType.TRAP:
                    INpgsqlCommand readTrapCardDataCommand = new NpsqlCommand("SELECT * FROM trapcard WHERE id=@id;");
                    readTrapCardDataCommand.Parameters.AddWithValue("id",id);
                    List<object[]> readTrapCardDataResults =
                        _mtcgDatabaseConnection.QueryDatabase(readTrapCardDataCommand);
                    
                    if(readTrapCardDataResults.Count!=1)
                        break;

                    int trapUses = Convert.ToInt32(readTrapCardDataResults[0][1]);
                    card=new TrapCard
                    {
                        Id=id,
                        Name = name,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        CardType = cardType,
                        Uses = trapUses
                    };
                    break;
                case ECardType.SPELL:
                    INpgsqlCommand readSpellCardDataCommand = new NpsqlCommand("SELECT * FROM spellcard WHERE id=@id;");
                    readSpellCardDataCommand.Parameters.AddWithValue("id",id);
                    List<object[]> readSpellCardDataResults =
                        _mtcgDatabaseConnection.QueryDatabase(readSpellCardDataCommand);
                    
                    if(readSpellCardDataResults.Count!=1)
                        break;
                    card = new SpellCard
                    {
                        Id=id,
                        Name = name,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        CardType = cardType
                    };
                    break;
                case ECardType.MONSTER:
                    INpgsqlCommand readMonsterCardDataCommand = new NpsqlCommand("SELECT * FROM monstercard WHERE id=@id;");
                    readMonsterCardDataCommand.Parameters.AddWithValue("id",id);
                    List<object[]> readMonsterCardDataResults =
                        _mtcgDatabaseConnection.QueryDatabase(readMonsterCardDataCommand);
                    
                    if(readMonsterCardDataResults.Count!=1)
                        break;

                    EMonsterType monsterType = (EMonsterType) Convert.ToInt32(readMonsterCardDataResults[0][1]);
                    
                    card = new MonsterCard
                    {
                        Id=id,
                        Name = name,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        CardType = cardType,
                        MonsterType = monsterType
                    };
                    break;
                case ECardType.SERVANT:
                    INpgsqlCommand readServantCardDataCommand = new NpsqlCommand("SELECT * FROM servantcard WHERE id=@id;");
                    readServantCardDataCommand.Parameters.AddWithValue("id",id);
                    List<object[]> readServantCardDataResults =
                        _mtcgDatabaseConnection.QueryDatabase(readServantCardDataCommand);
                    
                    if(readServantCardDataResults.Count!=1)
                        break;
                    
                    EServantClass servantClass = (EServantClass) Convert.ToInt32(readServantCardDataResults[0][1]);
                    
                    card = new ServantCard
                    {
                        Id=id,
                        Name = name,
                        Damage = damage,
                        Description = description,
                        ElementalType = elementalType,
                        CardType = cardType,
                        ServantClass = servantClass
                    };
                    
                    break;
            }
            
            return card;
        }
        private int CreateBaseCard(string name, string description, double damage, EElementalType elementalType,
            ECardType cardType)
        {
            INpgsqlCommand checkIfNameExistsCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            checkIfNameExistsCommand.Parameters.AddWithValue("name", name);
            int checkIfNameExistsResultCount = _mtcgDatabaseConnection.QueryDatabase(checkIfNameExistsCommand).Count;

            if (checkIfNameExistsResultCount > 0)
                return 0;

            NpsqlCommand createBaseCardCommand = new NpsqlCommand("INSERT INTO card (name, damage, description, elementaltype, cardtype) VALUES (@name, @damage, @description, @elementaltype, @cardtype);");
            createBaseCardCommand.Parameters.AddWithValue("name",name);
            createBaseCardCommand.Parameters.AddWithValue("damage",damage);
            createBaseCardCommand.Parameters.AddWithValue("description", description);
            createBaseCardCommand.Parameters.AddWithValue("elementaltype",(int)elementalType);
            createBaseCardCommand.Parameters.AddWithValue("cardtype",(int)cardType);
            
            return _mtcgDatabaseConnection.ExecuteStatement(createBaseCardCommand);
        }
        
        public int CreateMonsterCard(string name, string description, double damage, EElementalType elementalType,
            EMonsterType monsterType)
        {
            int createBaseCardAffectedRows = CreateBaseCard(name,description,damage,elementalType,ECardType.MONSTER);

            if (createBaseCardAffectedRows != 1)
                return createBaseCardAffectedRows;
            
            INpgsqlCommand readCreatedBaseCardCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            readCreatedBaseCardCommand.Parameters.AddWithValue("name", name);
            List<object[]> readCreatedBaseCardResults =
                _mtcgDatabaseConnection.QueryDatabase(readCreatedBaseCardCommand);

            if (readCreatedBaseCardResults.Count!=1)
                return 0;
            
            INpgsqlCommand createMonsterCardCommand = new NpsqlCommand("INSERT INTO monstercard (id, monstertype) VALUES (@id,@monstertype);");
            createMonsterCardCommand.Parameters.AddWithValue("id", readCreatedBaseCardResults[0][0]);
            createMonsterCardCommand.Parameters.AddWithValue("monstertype", (int)monsterType);

            return createBaseCardAffectedRows + _mtcgDatabaseConnection.ExecuteStatement(createMonsterCardCommand);
        }
        // should return 2
        public int CreateSpellCard(string name, string description, double damage, EElementalType elementalType)
        {
            int createBaseCardAffectedRows = CreateBaseCard(name,description,damage,elementalType,ECardType.SPELL);

            if (createBaseCardAffectedRows != 1)
                return createBaseCardAffectedRows;
            
            INpgsqlCommand readCreatedBaseCardCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            readCreatedBaseCardCommand.Parameters.AddWithValue("name", name);
            List<object[]> readCreatedBaseCardResults =
                _mtcgDatabaseConnection.QueryDatabase(readCreatedBaseCardCommand);

            if (readCreatedBaseCardResults.Count!=1)
                return 0;
            
            INpgsqlCommand createSpellCardCommand = new NpsqlCommand("INSERT INTO spellcard (id) VALUES (@id);");
            createSpellCardCommand.Parameters.AddWithValue("id", readCreatedBaseCardResults[0][0]);

            return createBaseCardAffectedRows + _mtcgDatabaseConnection.ExecuteStatement(createSpellCardCommand);
        }

        public int CreateAreaCard(string name, string description, double damage, EElementalType elementalType, int uses)
        {
            int createBaseCardAffectedRows = CreateBaseCard(name,description,damage,elementalType,ECardType.AREA);

            if (createBaseCardAffectedRows != 1)
                return createBaseCardAffectedRows;
            
            INpgsqlCommand readCreatedBaseCardCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            readCreatedBaseCardCommand.Parameters.AddWithValue("name", name);
            List<object[]> readCreatedBaseCardResults =
                _mtcgDatabaseConnection.QueryDatabase(readCreatedBaseCardCommand);

            if (readCreatedBaseCardResults.Count!=1)
                return 0;
            
            INpgsqlCommand createAreaCardCommand = new NpsqlCommand("INSERT INTO areacard (id, uses) VALUES (@id,@uses);");
            createAreaCardCommand.Parameters.AddWithValue("id", readCreatedBaseCardResults[0][0]);
            createAreaCardCommand.Parameters.AddWithValue("uses", uses);

            return createBaseCardAffectedRows + _mtcgDatabaseConnection.ExecuteStatement(createAreaCardCommand);
        }

        public int CreateServantCard(string name, string description, double damage, EElementalType elementalType,
            EServantClass servantClass)
        {
            int createBaseCardAffectedRows = CreateBaseCard(name,description,damage,elementalType,ECardType.SERVANT);

            if (createBaseCardAffectedRows != 1)
                return createBaseCardAffectedRows;
            
            INpgsqlCommand readCreatedBaseCardCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            readCreatedBaseCardCommand.Parameters.AddWithValue("name", name);
            List<object[]> readCreatedBaseCardResults =
                _mtcgDatabaseConnection.QueryDatabase(readCreatedBaseCardCommand);

            if (readCreatedBaseCardResults.Count!=1)
                return 0;
            
            INpgsqlCommand createServantCardCommand = new NpsqlCommand("INSERT INTO servantcard (id, servanttype) VALUES (@id,@servanttype);");
            createServantCardCommand.Parameters.AddWithValue("id", readCreatedBaseCardResults[0][0]);
            createServantCardCommand.Parameters.AddWithValue("servanttype", (int)servantClass);

            return createBaseCardAffectedRows + _mtcgDatabaseConnection.ExecuteStatement(createServantCardCommand);
        }

        public int CreateTrapCard(string name, string description, double damage, EElementalType elementalType, int uses)
        {
            int createBaseCardAffectedRows = CreateBaseCard(name,description,damage,elementalType,ECardType.TRAP);

            if (createBaseCardAffectedRows != 1)
                return createBaseCardAffectedRows;
            
            INpgsqlCommand readCreatedBaseCardCommand = new NpsqlCommand("SELECT * FROM card WHERE name=@name;");
            readCreatedBaseCardCommand.Parameters.AddWithValue("name", name);
            List<object[]> readCreatedBaseCardResults =
                _mtcgDatabaseConnection.QueryDatabase(readCreatedBaseCardCommand);

            if (readCreatedBaseCardResults.Count!=1)
                return 0;
            
            INpgsqlCommand createTrapCardCommand = new NpsqlCommand("INSERT INTO trapcard (id, uses) VALUES (@id,@uses);");
            createTrapCardCommand.Parameters.AddWithValue("id", readCreatedBaseCardResults[0][0]);
            createTrapCardCommand.Parameters.AddWithValue("uses", uses);

            return createBaseCardAffectedRows + _mtcgDatabaseConnection.ExecuteStatement(createTrapCardCommand);
        }

        public int Delete(int id)
        {    
            INpgsqlCommand deleteCardCommand = new NpsqlCommand("DELETE FROM card WHERE id=@id;");
            deleteCardCommand.Parameters.AddWithValue("id", id);
            return _mtcgDatabaseConnection.ExecuteStatement(deleteCardCommand);
        }

        public ACard Read(int id)
        {
            INpgsqlCommand readCardCommand = new NpsqlCommand("SELECT * FROM card WHERE id=@id;");
            readCardCommand.Parameters.AddWithValue("id", id);

            List<object[]> readCardResults = _mtcgDatabaseConnection.QueryDatabase(readCardCommand);

            if (readCardResults.Count != 1)
                return null;
            
            return ConvertToCard(readCardResults[0]);
        }

        public List<ACard> ReadAll()
        {
            INpgsqlCommand readAllCardsCommand = new NpsqlCommand("SELECT * FROM card;");
            List<object[]> readAllCardsResults = _mtcgDatabaseConnection.QueryDatabase(readAllCardsCommand);
            List<ACard> cards = new List<ACard>();

            foreach (object[] row in readAllCardsResults)
            {
                ACard card = ConvertToCard(row);
                if(card!=null)
                    cards.Add(card);
            }

            return cards;
        }
        public List<ACard> LoadCardDeck(User user)
        {
            List<ACard> cardDeck = new List<ACard>();
            
            INpgsqlCommand loadCardDeckOfUserCommand = new NpsqlCommand("SELECT * FROM carddeck WHERE userid=@userid;");
            loadCardDeckOfUserCommand.Parameters.AddWithValue("userid", user.Id);
            List<object[]> loadCardDeckOfUserResults = _mtcgDatabaseConnection.QueryDatabase(loadCardDeckOfUserCommand);

            foreach (object[] row in loadCardDeckOfUserResults)
            {
                ACard card = Read(Convert.ToInt32(row[2]));
                if(card!=null)
                    cardDeck.Add(card);
            }

            return cardDeck;
        }

        public List<ACard> LoadCardStack(User user)
        {
            List<ACard> cardStack = new List<ACard>();
            
            INpgsqlCommand loadCardStackOfUserCommand = new NpsqlCommand("SELECT * FROM cardstack WHERE userid=@userid;");
            loadCardStackOfUserCommand.Parameters.AddWithValue("userid", user.Id);
            List<object[]> loadCardStackOfUserResults =
                _mtcgDatabaseConnection.QueryDatabase(loadCardStackOfUserCommand);

            foreach (object[] row in loadCardStackOfUserResults)
            {
                ACard card = Read(Convert.ToInt32(row[2]));
                if(card!=null)
                    cardStack.Add(card);
            }

            return cardStack;
        }

        public List<ACard> LoadPackage(int packageid)
        {
            List<ACard> packageCards = new List<ACard>();
            
            INpgsqlCommand loadPackageCardsCommand = new NpsqlCommand("SELECT * FROM packagecards WHERE packageid=@packageid;");
            loadPackageCardsCommand.Parameters.AddWithValue("packageid", packageid);

            List<object[]> loadPackageCardsResults = _mtcgDatabaseConnection.QueryDatabase(loadPackageCardsCommand);
            
            foreach(object[] row in loadPackageCardsResults)
            {
                ACard card = Read(Convert.ToInt32(row[2]));
                if(card!=null)
                    packageCards.Add(card);
            }

            return packageCards;
        }
    }
}