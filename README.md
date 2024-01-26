### Introduction

This is a grid-based pathfinding simulation that uses a self made implementation of the A* star search algorithm.




### How to use

Press LMB to place tiles or press RMB to remove them. You can also switch between tile types by using the button on the bottom-left side of the screen. Place walls to block the path or glue, which takes twice as long to traverse as empty tiles. 

After placing down some tiles press the start button to start the simulation. While a simulation is running  you can't edit the grid, but you can pause the simulation and edit the grid again using the pause button and after unpausing again, a new path gets calcult. 

Press the reset button to reset the actor's position or press the clear button to clear the board of any tiles you placed down. 

By enabling or disabling the checkboxes on the top-left of the screen, you can also change the amount of information shown to you.



### Controls


| Input | Action |
|------------|-----------------|
| LMB | Place Object |
| RMB | Remove Object |
| SPACE | Start/Stop the simulation |
| W, A, S, D | Rotate the camera |
| Scroll wheel | Zoom in/out | 
| Q	| Switch between tile types |
| E	| Clear the board of any placed down tiles |
| R	| Reset the actor's position |
| H | Hide GUI |



### Distances Explanation
![image](https://github.com/SEB010105/a-star-pathfinder/assets/132783910/c4e13c15-a4ca-41fa-8c11-04bc14399968)

f ... sum of g and h

g ... distance to start

h ... distance to target

The font color of the tiles ,that have been visited while calculating the path , is set to black and the font color of the tiles, that only have been looked at by the algorithm but not visited, is set to grey. Tiles, without any text, haven't been looked at by the algorithm. 



### Extra Information

The cost to go from one empty tile to another is 5, the extra cost to go to a diagonal tile is 2 and the cost to traverse glue is 10.

[link to itch.io page](https://seb010105.itch.io/pathfinder-a-star-algorithm)
