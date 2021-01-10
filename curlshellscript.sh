#!/bin/bash



# Create User
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"

# should fail:
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"



# Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"

# create packages (done by "admin")
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,15,10,6,23]"
 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[2,18,11,4,19]"
																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[16,8,9,14,21]"
																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[20,22,3,5,17]"
																																																																																	 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[7,13,12,1,19]"
																																																																																	 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,18,9,5,19]"



# 4) acquire packages kienboec
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""

curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""

curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""

curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""

# should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""


#5) acquire packages altenhof
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""

curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""

#should fail (no package):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""



#add new packages
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[1,5,10,15,20]"

curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[2,8,12,19,22]"

curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[3,6,9,14,17]"



# 7) acquire newly created packages altenhof
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""

curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""

# should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""


# 8) show all acquired cards kienboec
curl -X GET http://localhost:10001/cards --header "Authorization: Basic kienboec-mtcgToken"
# should fail (no token)
curl -X GET http://localhost:10001/cards 


#9) show all acquired cards altenhof
curl -X GET http://localhost:10001/cards --header "Authorization: Basic altenhof-mtcgToken"



# 10) show unconfigured deck
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"


#11) configure deck
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "[17,5,3,23]"

curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"

curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[8,19,1,10]"

curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"

# should fail and show original from before:
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[3,9,1,10]"

curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"

# should fail ... only 3 cards set
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[8,19,1]"




#12) show configured deck 
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"


# 13) show configured deck different representation
# kienboec
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic kienboec-mtcgToken"

# altenhof
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic altenhof-mtcgToken"



# 15) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"



#16) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"



# 17) battle
curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-mtcgToken" &
curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-mtcgToken" &


# 18) Stats 
# kienboec
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"

# altenhof
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"



#19) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"



#20) trade
#check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"

# create trading deal
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"RequiredCoins\":3, \"CardToTrade\":22, \"Type\":0, \"MinDamage\": 18}"

# check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"

# delete trading deals
curl -X DELETE http://localhost:10001/tradings/1 --header "Authorization: Basic kienboec-mtcgToken"


# 21) check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"

curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"RequiredCoins\":3, \"CardToTrade\":22, \"Type\":0, \"MinDamage\": 18}"
# check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic altenhof-mtcgToken"

# try to trade with yourself (should fail)
curl -X POST http://localhost:10001/tradings/2 --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "20"

#try to trade 

curl -X POST http://localhost:10001/tradings/2 --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "20"

curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"

# 14) edit user data

curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-mtcgToken"

curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-mtcgToken"

curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic Kienboeck-mtcgToken" -d "{\"Username\": \"Kienboeck\",  \"Bio\": \"me playin...\"}"

curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "{\"Username\": \"Altenhofer\", \"Bio\": \"me codin...\"}"

curl -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Basic Kienboeck-mtcgToken"

curl -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Basic Altenhofer-mtcgToken"
# should fail:
curl -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Basic Kienboeck-mtcgToken"

curl -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Basic Altenhofer-mtcgToken"

curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic Altenhofer-mtcgToken" -d "{\"Username\": \"Hoax\",  \"Bio\": \"me playin...\"}"

curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic Kienboeck-mtcgToken" -d "{\"Username\": \"Hoax\", \"Bio\": \"me codin...\"}"

curl -X GET http://localhost:10001/users/someGuy  --header "Authorization: Basic Kienboeck-mtcgToken"

# end...

