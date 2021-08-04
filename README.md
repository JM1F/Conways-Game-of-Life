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

![image](https://user-images.githubusercontent.com/71614127/128253341-5086e9b7-f5d5-49f6-8b71-64e5319798df.png)

This rendition provides an infinite grid space in which you are able to draw and erase cells within. All cells within the grid follow the rules stated above.

<b>The game also comes infomation about:</b>

- Whether the game is "Running" or "Not Running" 
- Mouse position
- Speed of program
- Selected file
- Generation number
- Population amount
- Frames Per Second Counter

![image](https://user-images.githubusercontent.com/71614127/128254027-2aba6c48-1ce9-455a-8761-f1be2fe5d8a4.png)
