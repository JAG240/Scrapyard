# Scrapyard
After sending out a small demo/play test for another game called "<a href="https://yippika.itch.io/chops-butcher-shop?password=ShiftToRun">Chops Butcher shop</a>," I have embarked on another game idea. The goals of this
game are to implement more better orgaized and testable code. For clarity, I do still intend to revisit the older project and bring it to full closure. 

<h2>The Plan</h1>
The way I plan to achieve this is by implementing a couple rules. 

<h3>Class Diagrams</h3>
The first and most important rule, every feature will have a class daigram that should describe
the attributes and methods along with their releation to other relevant scripts. I prompted left these mind maps on my phone and will upload them later. 

<h3>Singleton Avoidance</h3>
Next, I aim to eliminate singletons from my codebase. While I could look into dependency injection, for a <i>hopefully</i> small project like
this, I opted for a service locator design pattern. 

<h3>Assembly Definitions</h3>
Lastly, I will be using assemly definitions in this project. As assemblies need reference to each other ot work and refuse cyclic dependencies,
it will be nearly impossible to allow the unruly <i>(pun intended)</i> speghetti monster that my previous project has become. 

<h3>Assets</h3>
Post-lastly, my previous project relied very heavily on public asset usage. This often resulted in a large dump of example scene, character controller scripts, animators,
and a bunch of other unused assets. For this project I will strive to use handmade assets as much as possible. Yes this does mean HOURS of modeling, texturing, uv mapping,
and animating but, <i>so what?</i>

<h2>About The Game</h2>
<i>Scrapyard</i> will be a 3D top-down styled bullet-hell shooter. It will implement features like build crafting and looting. 

<h3>The Setting</h3>
The main character will be on a version of Earth that has seen a (very false, yet fun) theory that Earth is moving closer to the sun come to reality. 
As a result all of the oceans have dried up and water is difficult to come by. Not to mention the rogue automation robots of the past that have
turned to violence and resource hording a couple centeries back. The good news, the sun provides plenty of solar energy and the lands are littered with 
scrap metals and resources that a bit of ingenuity can make into nearly anything<i>, just not drinkable water</i>.

<h3>Story</h3>
The main character will find themselves partnered with a great inventor (maybe as a refurbished combat robot). However, in a group of survivalist and families that are running their final well water dry. 
Through-out the adventure it will be the task of the main character to find water sources and return with the water. Additionally, they might do some good to figure out
what all those robots are up to, other than trying to steal resources away from humans, and get some gear to take on the robot overlords.
