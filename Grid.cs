using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace Conways_Game_of_Life
{
    public class Grid
    {
        public Game Game;
        public GraphicsDeviceManager Graphics;
        public int ViewportWidth;
        public int ViewportHeight;
        public int TileSize = 10;
        public Texture2D CellTexture;
        public View CurrentView;
        public KeyboardState previousKeyboardstate;
        public Dictionary<Point, bool> GridOfCells { get; set; }
        public Dictionary<Point, bool> GridOfCellsTest { get; set; }
        public Dictionary<Point, int> GridOfNeighbourCells { get; set; }
        public KeyboardState keyboardState { get; set; }
        public Vector2 mouseLocationSimplified { get; set; }
        public bool GameInProgress { get; set; } = false;
        public int updateValGrid { get; set; } = 0;
        public int updateValueGame { get; set; }
        public int GenerationNumber { get; set; } = 0;
        public int CellPopulation { get; set; } = 0;
        public string FileString { get; set; }
        public bool FileInputInProgress { get; set; }
        /// <summary>
        /// The Main method of the grid class.
        /// </summary>
        /// <param name="CurrentGame"></param>
        /// <param name="CurentGraphics"></param>
        /// <param name="view"></param>
        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics, View view)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;
            CurrentView = view;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;
            GameInProgress = false; 
        }
        /// <summary>
        /// The main update loop for the grid class.
        /// </summary>
        /// <param name="gameUpdateValue"></param>
        /// <param name="fileinputinprogress"></param>
        /// <param name="fileString"></param>
        public void Update(int gameUpdateValue, bool fileinputinprogress, string fileString)
        {
            // Setting variables
            updateValueGame = gameUpdateValue;
            FileInputInProgress = fileinputinprogress;
            FileString = fileString;

            keyboardState = Keyboard.GetState();
            // Checks to see if space has been pressed and that the file input state is not in progress.
            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardstate.IsKeyUp(Keys.Space) && FileInputInProgress == false)
            {
                // GameInProgress set to the opposite of the game state
                GameInProgress = !GameInProgress;
                // Generation number is reset
                GenerationNumber = 0;
            }

            var mouseState = Mouse.GetState();
            // Transforms mouse locations into Vector2 with respective inverse matrix of the view
            var mouseLocation = Vector2.Transform(mouseState.Position.ToVector2(), CurrentView.InverseMatrix);
            // Sets the mouse location to a simplified grid state by dividing the location by the tilesize. e.g (0,10) to (0,1)
            mouseLocationSimplified = new Vector2(MathF.Floor(mouseLocation.X / TileSize), MathF.Floor(mouseLocation.Y / TileSize));
            // Checks to see if the "A" button is pressed. When "A" is pressed then it will try to read the file string
            if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardstate.IsKeyUp(Keys.A) && FileInputInProgress == false)
            {
                // Tries the call ReadPattern();
                try
                {
                    ReadPattern();
                }
                // If unsuccessful
                catch (IOException)
                {
                    // Continue with the loop if file not found or error.
                }  
            }
            // Checks the see if "R" is pressed and that the file input is not active. Resets the current cells
            if (keyboardState.IsKeyDown(Keys.R) && FileInputInProgress == false)
            {
                // Sets new empty GridOfCells
                GridOfCells = new Dictionary<Point, bool>();
                GenerationNumber = 0;
            }
            // Allows user to step to next generation with right arrow if the file input is not active and the game is not currently running
            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardstate.IsKeyUp(Keys.Right) && GameInProgress == false && FileInputInProgress == false)
            {
                ProceedToNextGeneration();
            }
            // Checks if the left mouse button is pressed
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Checks to see if the cell is already alive (active) where the mouse clicks
                if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() ) == true )
                {
                    // Leaves it set to true in the dictionary
                    GridOfCells[mouseLocationSimplified.ToPoint()] = true;
                }
                else
                {
                    // If not, the cell is added to the grid of cells structure
                    GridOfCells.Add(mouseLocationSimplified.ToPoint(), true);
                }
            }
            // Checks if the right mouse button is pressed. Removes/erases cells off the grid
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                // Checks to see if the cell is already alive (active) where the mouse clicks
                if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint()) == true)
                {
                    // Removes the cell from the grid of cells structure 
                    GridOfCells.Remove(mouseLocationSimplified.ToPoint()); 
                }

            }
            // Checks if the game is running
            if (GameInProgress == true)
            {
                // Resets the update value for the grid update value when it is greater than 20.
                if (updateValGrid > 20)
                {
                    updateValGrid = 0;
                }
                // Runs ProceedToNextGeneration(); each update loop
                ProceedToNextGeneration();
                // update value of the grid increases by 1 each time
                updateValGrid += 1;
                
            }
            // Gathers the amount of cells in the GridOfCells dictionary
            CellPopulation = GridOfCells.Count(); 
            // Sets previous keyboard state to the current keyboard state
            previousKeyboardstate = keyboardState;
            
        }

       /// <summary>
       /// The method that runs each time the game updates to get the next generation of cells
       /// </summary>
       public void ProceedToNextGeneration()
        {
            // If the set speed equals the addition of the updated grid value and the game is not active, then only proceed to the next generation
            if (updateValGrid == updateValueGame || GameInProgress == false)
            {
                // Increase generation number each iteration
                GenerationNumber += 1;
                // Set and generate new dictionaries for the set of next generation cells and all the neighbouring cells
                Dictionary<Point, bool> NextGridOfCells = new Dictionary<Point, bool>();
                GridOfNeighbourCells = new Dictionary<Point, int>();
                // Check each cell in the the grid of all the cells
                foreach (var cell in GridOfCells)
                {
                    // init cellNeighbours
                    int cellNeighbours = 0;
                    // Check neighbours for each of the surrounding cells on the grid. e.g. (1,1) = Bottom Right Cell. The return variable form the function is the added to the cellNeighbour amount
                    cellNeighbours += CheckNeighbours(cell, -1, 0);
                    cellNeighbours += CheckNeighbours(cell, 0, -1);
                    cellNeighbours += CheckNeighbours(cell, 1, 0);
                    cellNeighbours += CheckNeighbours(cell, 0, 1);

                    cellNeighbours += CheckNeighbours(cell, 1, 1);
                    cellNeighbours += CheckNeighbours(cell, -1, -1);
                    cellNeighbours += CheckNeighbours(cell, -1, 1);
                    cellNeighbours += CheckNeighbours(cell, 1, -1);
                    // Rule of Conway's Game of Life. Checks to see if the cell neighbours are greater or equal to 2 but also less than 4
                    if (cellNeighbours >= 2 && cellNeighbours < 4)
                    {
                        // Set the Next Generation if the rule is fulfilled 
                        NextGridOfCells.Add(cell.Key, cell.Value);
                    }
                }
                // Loops through each cell in the Neighbour cells dictionary
                foreach (var neighbourCell in GridOfNeighbourCells)
                {
                    // Rule of Conway's Game of Life. Checks to see if the dead (unactive) cell has 3 neighbours
                    if (neighbourCell.Value == 3)
                    {
                        // If the cell is not already alive (active)
                        if (NextGridOfCells.ContainsKey(neighbourCell.Key) == false)
                        {
                            // Add the cell to the next generation
                            NextGridOfCells.Add(neighbourCell.Key, true);
                        }
                    }

                }
                // Sets the current grid of cells to the next generation
                GridOfCells = NextGridOfCells;
                // The update grid value is reset to 0
                updateValGrid = 0;
            }
        }
        /// <summary>
        /// Checks the neighbours of the cell inputted with the directional vectors. 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="directionX"></param>
        /// <param name="directionY"></param>
        /// <returns></returns>
        public int CheckNeighbours(KeyValuePair<Point, bool> cell, int directionX, int directionY)
        {
            // init cellNeighbours and set to 0
            int cellNeighbours = 0;
            // Creates a cell vector that takes in the directional vectors
            Vector2 cellVector = new Vector2(cell.Key.X + directionX, cell.Key.Y + directionY);
            // Checks to see if the neigbour cell is active which will then count as an active neighbour
            if (GridOfCells.ContainsKey( cellVector.ToPoint() ) == true)     
            {
                cellNeighbours += 1;
            }
            // Check the neighbour cell's neighbours and add the amount of cells respectively
            if (GridOfNeighbourCells.TryAdd(cellVector.ToPoint(), 1) == false)
            {
                GridOfNeighbourCells[cellVector.ToPoint()] += 1;
            }
            // Return the cell neighbour amount 
            return cellNeighbours;
        }
        /// <summary>
        /// The main draw function for the grid class.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: CurrentView.Matrix, samplerState: SamplerState.PointClamp);
            // Loop over each cell in the grid of cells dictionary
            foreach (var cell in GridOfCells)
            {
                // Draw only the cells that are active
                spriteBatch.Draw(CellTexture, new Vector2((cell.Key.X * TileSize), cell.Key.Y * TileSize), null, Color.Gray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
            }
            // Draws the hover square when the mouse enters the region
            spriteBatch.Draw(CellTexture, new Vector2((mouseLocationSimplified.X * TileSize), mouseLocationSimplified.Y * TileSize), null, Color.DarkGray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);

            spriteBatch.End();
        }
        /// <summary>
        /// Generates the grid structure.
        /// </summary>
        public void CreateGrid()
        {
            GridOfCells = new Dictionary<Point, bool>();
            CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            CellTexture.SetData(new Color[] { Color.DarkGray});
        }
        /// <summary>
        /// Takes the file input and reads the files for their data to be outputted on the screen for different types of patterns in the Conway's Game of Life archive.
        /// </summary>
        public void ReadPattern()
        {
            // Gets the path of the current directory and concatenate the file string to the path
            string textFilePath = Directory.GetCurrentDirectory() + "\\" + FileString ;
            // Reads the file
            string[] text = File.ReadAllLines(textFilePath);
            // Gets the file extension beginning index
            int fileextensionStartIndex = textFilePath.LastIndexOf(".");
            // Gets the length needed for the file extension to make a substring
            int lengthOfSubstring = textFilePath.Length - fileextensionStartIndex;
            // Variable for the file extension 
            string fileextension = textFilePath.Substring(fileextensionStartIndex, lengthOfSubstring);

            // Checks what type of file extension and proceeds accordinly
            if (fileextension == ".rle")
            {
                // Run Length Encoding File - More efficient hardly human readable.

                // Calls the RLE Decoder and returns the decoded text into a list
                List<string> DecodedText = RLEDecode(text);
                int lineCount = 0;
                // Loops through each line in the decoded text
                foreach (var line in DecodedText)
                {
                    // Loops through each character in the decoded text
                    for (int i = 0; i < line.Length; i++)
                    {
                        // Checks to see if the character is "o" because this is what the official Game of Life wiki uses to identify an alive (active) cell
                        if (line[i] == 'o')
                        {
                            // Generate a location within the file X,Y coordinates
                            Point coordinates = new Point(i, lineCount);
                            // Check to see if the cells are already active when added to the location of the mouse.
                            if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() + coordinates) == false)
                            {
                                // If not, Add the cell to the grid of cells structure 
                                GridOfCells.Add(mouseLocationSimplified.ToPoint() + coordinates, true);
                            }
                        }
                    }
                    // Increment the line count on each pass of the loop 
                    lineCount += 1;
                }
            }
            else if (fileextension == ".cells")
            {
                // .Cells Files - Less efficient but easier to understand.
                int lineCount = 0;
                // The official patterns contain other data that is not the pattern itself, so I made and identifier that allows you to only read the needed line in the file
                bool ProcessNextLine = false;

                // Loops through each line the in the file
                foreach (var line in text)
                {
                    // Checks to see if the line is populated due to unpopulated lines causing a bug
                    if (line.Length != 0)
                    {
                        // Indicator for the official pattern misc. Checks if the first character of each line is "!" if not then the program can procced and calculate the pattern
                        if (line[0] != '!')
                        {
                            ProcessNextLine = true;
                        }
                    }
                    // Once the file misc has stopped
                    if (ProcessNextLine == true)
                    {
                        // Loop through each character in the text
                        for (int i = 0; i < line.Length; i++)
                        {
                            // Check to see if the character is "O", which is the identifier used in the .cells structure for an alive (active) cell
                            if (line[i] == 'O')
                            {
                                // Generate a location within the file X,Y coordinates
                                Point coordinates = new Point(i, lineCount);
                                // Check to see if the cells are already active when added to the location of the mouse.
                                if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() + coordinates) == false)
                                {
                                    // If not, Add the cell to the grid of cells structure 
                                    GridOfCells.Add(mouseLocationSimplified.ToPoint() + coordinates, true);
                                }
                            }
                        }
                        // Increment the line count on each pass of the loop 
                        lineCount += 1;
                    }
                    
                }
            }
            else if (fileextension == ".txt")
            {
                // .txt Files - Make your own patterns and save them to use. 
                int lineCount = 0;
                // Loop through each line in the text
                foreach (var line in text)
                {
                    // Loop through each character in the text
                    for (int i = 0; i < line.Length; i++)
                    {
                        // Check to see if the character is "X", which is the identifier that im using to indentify and alive (active) cell
                        if (line[i] == 'X')
                        {
                            // Generate a location within the file X,Y coordinates
                            Point coordinates = new Point(i, lineCount);
                            // Check to see if the cells are already active when added to the location of the mouse.
                            if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() + coordinates) == false)
                            {
                                // If not, Add the cell to the grid of cells structure 
                                GridOfCells.Add(mouseLocationSimplified.ToPoint() + coordinates, true);
                            }
                        }
                    }
                    // Increment the line count on each pass of the loop 
                    lineCount += 1;
                }
            }
        }
        /// <summary>
        /// A Run Length Decoder that allows .rle files to be decoded in the Game of Life's wiki format of the files.
        /// </summary>
        /// <param name="FileTextRLE"></param>
        /// <returns></returns>
        public List<string> RLEDecode(string[] FileTextRLE)
        {
            // Makes a new List for the decoded string
            List<string> DecodedStringFormat = new List<string>();
            string DecodedString = "";
            string NumberInString = "";
            // Set the bool value to see if the next line needs to deocded
            bool RLENextLine = false;
            // Loops through each line the in the file
            foreach (string line in FileTextRLE)
            {
                // Checks to see if the current line needs to decoded
                if (RLENextLine == true)
                {
                    // Loop through each character in the text
                    for (int i = 0; i < line.Length; i++)
                    {
                        // Check if the character is not a number and does not equal the dollar sign which indicates a new line
                        if (char.IsLetter(line[i]) && line[i] != '$')
                        {
                            // Checks to see if the NumberInString is populated if its not, it marks the end of the string to be increased. e.g. 5o = ooooo
                            if (NumberInString.Length > 0)
                            {
                                // This concatenates the character by the number infront of it
                                DecodedString += String.Concat(Enumerable.Repeat(line[i], int.Parse(NumberInString)));
                            }
                            else
                            {
                                DecodedString += line[i];
                            }
                            // Reset the variable
                            NumberInString = "";
                        }
                        // Checks to see if there is a new line (?) or end of decoding (!) character
                        else if (line[i] == '$' || line[i] == '!')
                        {
                            if (NumberInString.Length > 0)
                            {
                                // Loops the length of the number infront of the character
                                for (var j = 0; j < int.Parse(NumberInString); j++)
                                {
                                    // Adds empty lines due to a new line being added through the "$" code
                                    DecodedStringFormat.Add(DecodedString);
                                    DecodedString = "";
                                }
                            }
                            // If there is just one "$" or "!", just add a new line once
                            else
                            {
                                DecodedStringFormat.Add(DecodedString);
                                DecodedString = "";
                            }

                            NumberInString = "";

                        }
                        // If none of them are the case then concatenate the number to a string 
                        else
                        {
                            NumberInString += line[i];

                        }
                    }
                }
                // Checks to see if the line contains the parameters below becuase this is the line on all of the official files which marks the start of the code that needs to decoded
                if (line.Contains("x = ") && line.Contains("y = ") && line.Contains("rule = "))
                {
                    RLENextLine = true;
                }
            }
            // Returns the deocded list
            return DecodedStringFormat;
        }   

    }



}
