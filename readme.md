# SWE1-MTCG project

The aim of this project is to create the possible backend of a monster trading card game. The Webservice aspect is taken from another [repository](https://github.com/kretmatt/SWE1-REST-HTTP-Webservice) of mine.

This project has a public [Github Repository](https://github.com/kretmatt/SWE1-MTCG), where all commits and branches can be seen.

# Special explanatory notes

Further steps are explained in this section.

## Comment on the amount of branches

Although I programmed the game logic, etc. for this project during the semester, I basically began from scratch a week ago, because I wasn't satisfied with the code and game logic. The game logic was scattered in different classes, which led to more and more problems. That's why I began from scratch. The old branches are still visible in the repository. The "new" branches end with the suffix "-revised". The first time I started randomly with the project, but this time I had a database first approach.

## Class Diagram

The class diagram can be broken up into serveral "pieces" of logic. 

* DBConnection + Repositories
* GameLogic
* WebService-Aspect

### DBConnection, Repsoitories, Data transfer objects

Like I said before, this "version" of the project used a database-first approach. In the beginning, I defined the DTOs.

* Cards: ACard, AreaCard, ...
* Package
* User
* UserStats
* BattleHistory
* TradingDeal

For all those DTOs I created tables and defined their relationships between each other. Additionally, I created a session table, in which the current sessions are saved.

I wanted the access to the database to be regulated and consistent, which is why I defined interfaces for the repositories. In addition I encapsulated NPGSQL to some extent (NPGSQLCommand and NPGSQLDatareader) in new classes, which I used to read, update, insert and delete entities. Every repository has several NPGSQL-Commands which are sent to MTCGDatabaseConnection, where they are executed. Inserts, Updates and deletes return the amount of rows affected by them, whereas select statements return lists of object arrays.

### Game logic - Arena, BattleSystem and rule books

The main elements of the battle logic are arenas, battle systems and rule books. A battle system has an arena, in which two users repeatedly let their cards fight against each other. Some cards have special effects. One example are area cards, which change the area in the arena and boost the damage of cards, who have the same elemental attribute as the area itself. Those areas influence the battle until a new area overrides it. One-time influences are trap cards. The round after the trap card is used, a trap will be triggered, which adds to the attack dealt by the currently used card.

Although there are some influences, the majority of the battles will be decided by the Rulebook-class. This class is pretty much the battle logic itself. It consists of several objects, which define strengths and weaknesses between certain cards. These are the classes, which define those specialties.

* Affinity Charts - Define elemental strengths and weaknesses
* Monster Hierarchies - Define strengths and weaknesses of monsters.
* Servant Hierarchies - Define strengths and weaknesses of servants. 

### WebService-Aspect

The web-service aspect of this project is described in the other repository called SWE1-HTTP-Webservice. Based on this earlier project, I created several different endpoint handlers, which handle the different http-requests that are supported by the BaseHTTPServer.

## Additional features

### ServantCards
Inspired by the Fate-Series I implemented servant cards. Servant cards can have one of the following classes

* Saber
* Archer
* Lancer
* Rider
* Caster
* Assassin
* Berserker

Weaknesses and strenghts are inspired by a mobile game of the franchise. The same multipliers from the elements apply here.

### AreaCards
Area cards influence the battle over several different rounds. Areas do not disappear, they can only be changed.

### TrapCards
Trap cards have limited uses and can turn the tides of the battle. They deal pure damage, which is not affected by anything else. Thus they could save a knight in a fight against water based spells.

## Unit-Testing and Mocking

The majority of the tests consists of mocks. Before I implemented the repositories themselves, I mocked the different functionalities provided by them. After mocking, I implemented the repositories. That's why the mocks are only the rough course of action of the methods.

Big parts of the battle logic are covered by unit tests. The WebService-Aspect of the project is not tested at all, because the basic functionality was already tested in the other project. 


## Time spent on the project
Approximately 50-60 hours were spent on this version of the project. In addition, I was probably programming for 20-30 hours on the original version. 

## Lessons learnt
* I should spend more time on the design of projects
* Better understanding of threads (especially locks with monitors)
* (Hopefully) better understanding of the repository pattern

