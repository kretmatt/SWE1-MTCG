# SWE1-MTCG

Program for trading and battling with and against each other in a magical card-game world.

# Special explanatory notes

Further steps are explained in this section.

## Elemental attributes

Due to future plans involving the decorator pattern, elemental attributes are currently implemented as classes. There is also an elemental attribute enum called "EElementalAttributes". For unit testing I used TestCaseSource to avoid writing lots of lines just for 1 unit test. New elemental attributes can be easily added. A new attribute just needs to inherit from AElementalAttribute and it needs to define it's weaknesses and strengths.

Future plans:
* New elemental attributes
	* Ice
	* Electric
	* Darkness
	* Light
	* Wind
* Immunities (e.g.: Darkness is immune to Normal)

