#!/bin/bash



# Create User
echo Create Users
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo
sleep .5
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo
sleep .5
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo
sleep .5
# should fail:
echo Should fail
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo
sleep .5
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
sleep .5
echo
sleep .5
# Login Users
echo Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo
sleep .5
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo
sleep .5
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo Should fail
sleep .5
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
sleep .5

# create packages (done by "admin")
echo create packages
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,15,10,6,23]"
echo	
sleep .5		    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[2,18,11,4,19]"
echo																																																																														
sleep .5				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[16,8,9,14,21]"
echo																																																																																 
sleep .5				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[20,22,3,5,17]"
echo																																																																															 
sleep .5				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[7,13,12,1,19]"
echo																																																																															 
sleep .5				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,18,9,5,19]"
echo
sleep .5
# 4) acquire packages kienboec
echo Kienboec open packages
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
echo
sleep .5
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
echo
sleep .5
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
echo
sleep .5
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
echo Should fail
sleep .5
# should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
sleep .5

#5) acquire packages altenhof
echo Altenhof open packages
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
echo
sleep .5
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
echo Should Fail
#should fail (no package):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
sleep .5


#add new packages
echo Add new packages
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,5,10,15,20]"
echo
sleep .5
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[2,8,12,19,22]"
echo
sleep .5
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[3,6,9,14,17]"
echo
sleep .5

# 7) acquire newly created packages altenhof
echo Altenhof open package
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
echo
sleep .5
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
echo Should fail
sleep .5
# should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
sleep .5

# 8) show all acquired cards kienboec
echo Kienboec card stack
curl -X GET http://localhost:10001/cards --header "Authorization: Basic kienboec-mtcgToken"
# should fail (no token)
sleep .5
echo should fail
curl -X GET http://localhost:10001/cards 
sleep .5

#9) show all acquired cards altenhof
echo Altenhof card stack
curl -X GET http://localhost:10001/cards --header "Authorization: Basic altenhof-mtcgToken"
sleep .5
# 10) show unconfigured deck
echo Unconfigured decks
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
sleep .5

#11) configure deck
echo Configure decks
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "[17,5,3,23]"
echo
sleep .5
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[8,19,1,10]"
echo
sleep .5
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
echo Should fail
sleep .5
# should fail and show original from before:
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[3,9,1,10]"
echo
sleep .5
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
echo
sleep .5
# should fail ... only 3 cards set
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[8,19,1]"
echo
sleep .5


#12) show configured deck 
echo show configured deck
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
sleep .5

# 13) show configured deck different representation
# kienboec
echo Show configured deck plain
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
# altenhof
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic altenhof-mtcgToken"
echo
sleep .5

# 15) stats
echo Show battle histories
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"
echo
sleep .5

#16) scoreboard
echo Show stats of all players
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"
sleep .5


# 17) battle
echo battle
curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-mtcgToken" &
sleep .5
curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-mtcgToken" &
wait
echo
sleep .5

# 18) Stats 
# kienboec
echo Battle history kienboec
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"
sleep .5
# altenhof
echo Battle history altenhof
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"
sleep .5


#19) scoreboard
echo User stats
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"
sleep .5


#20) trade
#check trading deals
echo all trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
sleep .5
# create trading deal
echo create
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"RequiredCoins\":3, \"CardToTrade\":22, \"Type\":0, \"MinDamage\": 18}"
sleep .5
# check trading deals
echo check deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"
echo
sleep .5
# delete trading deals
echo delete
curl -X DELETE http://localhost:10001/tradings/1 --header "Authorization: Basic kienboec-mtcgToken"
sleep .5

# 21) check trading deals
echo check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"
echo create trading deal
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"RequiredCoins\":3, \"CardToTrade\":22, \"Type\":0, \"MinDamage\": 18}"
echo
sleep .5
# check trading deals
echo check trading deal
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic altenhof-mtcgToken"
echo Should fail
sleep .5
# try to trade with yourself (should fail)
curl -X POST http://localhost:10001/tradings/2 --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "20"
echo
sleep .5
#try to trade 
echo Trade
curl -X POST http://localhost:10001/tradings/2 --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "20"
echo
sleep .5
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"
sleep .5
# 14) edit user data
echo Edit user data - Show data
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-mtcgToken"
sleep .5
echo Update user
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Username\": \"Kienboeck\",  \"Bio\": \"me playin...\"}"
echo
sleep .5
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "{\"Username\": \"Altenhofer\", \"Bio\": \"me codin...\"}"
sleep .5
echo Show
curl -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Basic Kienboeck-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Basic Altenhofer-mtcgToken"
sleep .5
echo Should fail
# should fail:
curl -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Basic Kienboeck-mtcgToken"
echo
sleep .5
curl -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Basic Altenhofer-mtcgToken"
echo
sleep .5
curl -X PUT http://localhost:10001/users/Kienboeck --header "Content-Type: application/json" --header "Authorization: Basic Altenhofer-mtcgToken" -d "{\"Username\": \"Hoax\",  \"Bio\": \"me playin...\"}"
echo 
sleep .5
curl -X PUT http://localhost:10001/users/Altenhofer --header "Content-Type: application/json" --header "Authorization: Basic Kienboeck-mtcgToken" -d "{\"Username\": \"Hoax\", \"Bio\": \"me codin...\"}"
echo 
sleep .5
curl -X GET http://localhost:10001/users/someGuy  --header "Authorization: Basic Kienboeck-mtcgToken"
sleep .5
# end...

