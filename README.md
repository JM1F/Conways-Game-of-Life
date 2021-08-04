<p align="center">
  <img src="https://user-images.githubusercontent.com/71614127/126913258-daa4f4af-40fd-4642-b88a-c8370789386a.png">
</p>
<p align="center">
  This game is a rendition of John Conway's Game of Life.
</p>

## Background
The Game of Life, also known simply as Life, is a [cellular automaton](https://en.wikipedia.org/wiki/Cellular_automaton) devised by the British mathematician [John Horton Conway](https://en.wikipedia.org/wiki/John_Horton_Conway) in 1970. It is a [zero-player game](https://en.wikipedia.org/wiki/Zero-player_game), meaning that its evolution is determined by its initial state, requiring no further input. One interacts with the Game of Life by creating an initial configuration and observing how it evolves. It is [Turing complete](https://en.wikipedia.org/wiki/Turing_completeness) and can simulate a [universal constructor](https://en.wikipedia.org/wiki/Von_Neumann_universal_constructor) or any other [Turing machine](https://en.wikipedia.org/wiki/Turing_machine). 

Source: [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

## Rules

<b>For Alive (Active) Cells:</b>

- Each cell with one or zero neighbours dies, as if by underpopulation.
- Each cell with four or more neighbours dies, as if by overpopulation.
- Each cell with two or three neighbours will survive.

<b>For Dead (Unactive) Cells:</b>

- Each cell with exactly three neighbours becomes populated.

## Project Details

![GOL](https://user-images.githubusercontent.com/71614127/128258060-947d887b-1c8c-4ad8-8f39-24ade2b5ba2d.gif)


This rendition provides an infinite grid space in which you are able to draw and erase cells within. All cells within the grid follow the rules stated above.

- Made using [Monogame.](https://www.monogame.net/)
- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

<b>The game also comes infomation about:</b>

- Whether the game is "Running" or "Not Running" <img align="right" src="https://user-images.githubusercontent.com/71614127/128254027-2aba6c48-1ce9-455a-8761-f1be2fe5d8a4.png">
- Mouse position 
- Speed of program
- Selected file
- Generation number
- Population amount
- Frames Per Second Counter


## How the file input works

To activate the file input process you will need to press the <b>`</b> or <b>¬</b> key. Once active, you will not be able to run the next generations until the process is turned off with the same button(s). You can input one of the files from the LifeWikiPatterns.zip file or make your own text file which details are below.

<b>Step-by-Step:</b>

- Press the key ` or ¬ to activate
- Type the directory of the file. e.g. 1beacon.cells OR 1beacon.rle OR FileName\1beacon.rle
- Press the key ` or ¬ to deactivate
- Press `A` to output pattern to mouse location

## How to add your own patterns with a .txt file

The file input works the same as above but with the .txt extension.

When creating your own pattern you will have to use the `X` character to represent an alive (active) cell while everything else is classed as a dead (unactive) cell.


*Glider Gun Example*
```
GLIDERGUN###############X#######
######################X#X#######
############XX######XX########XX
###########X###X####XX########XX
XX########X#####x###XX##########
XX########X###X#XX####X#X#######
##########X#####X#######X#######
###########X###X################
############XX##################
```

## Controls

**` or ¬ : To activate/deactivate the file input process. Once active you will be able to type with the keybaord**

`SPACE` **: Run the game at the set speed**

`DOWN ARROW` **: Slow down the game**

`UP ARROW` **: Speed up the game**

`RIGHT ARROW` **: Procced to next generation**

`A` **: Output current file at mouse location**

`R` **: Reset the grid of cells**

`MOUSE1 (LEFT CLICK)` **: Draw cells**

`MOUSE2 (RIGHT CLICK)` **: Erase cells**

`MOUSE3 (MIDDLE CLICK)` **: Move Camera**

`SCROLL` **: Zoom in and out**

`T` **: Toggle the UI on/off**

`ESCAPE` **: Exits the program**

## Download

***Download [Here](https://github.com/JM1F/Conways-Game-of-Life/releases/tag/v1.0)***

Versions:

- Windows - win-x64
- Windows - win-x86
- Linux - linux-x64
- Mac - osx-x64

## More information about John Conway's Game of Life

- [LifeWiki](https://conwaylife.com/wiki/Main_Page)

- [Conway's Game of Life Wikipedia](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

- [Pattern Libary](https://conwaylife.appspot.com/library)
