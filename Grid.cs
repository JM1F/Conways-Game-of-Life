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
        public string FileString { get; set; }
        public bool FileInputInProgress { get; set; }

        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics, View view)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;
            CurrentView = view;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;
            GameInProgress = false;
            
        }

        public void Update(int gameUpdateValue, bool fileinputinprogress, string fileString)
        {
            updateValueGame = gameUpdateValue;
            FileInputInProgress = fileinputinprogress;
            FileString = fileString;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardstate.IsKeyUp(Keys.Space) && FileInputInProgress == false)
            {
                GameInProgress = !GameInProgress;
            }

            var mouseState = Mouse.GetState();

            var mouseLocation = Vector2.Transform(mouseState.Position.ToVector2(), CurrentView.InverseMatrix);

            mouseLocationSimplified = new Vector2(MathF.Floor(mouseLocation.X / TileSize), MathF.Floor(mouseLocation.Y / TileSize));


            if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardstate.IsKeyUp(Keys.A) && FileInputInProgress == false)
            {
                try
                {
                    ReadPattern();
                }
                catch(IOException)
                {
                    // continue with the loop if file not found or error.
                }  
            }

            if (keyboardState.IsKeyDown(Keys.R) && FileInputInProgress == false)
            {
                GridOfCells = new Dictionary<Point, bool>();
            }
            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardstate.IsKeyUp(Keys.Right) && GameInProgress == false && FileInputInProgress == false)
            {
                ProceedToNextGeneration();
            }


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() ) == true )
                {
                    GridOfCells[mouseLocationSimplified.ToPoint()] = true;
                }
                else
                {
                    GridOfCells.Add(mouseLocationSimplified.ToPoint(), true);
                }
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint()) == true)
                {
                    GridOfCells.Remove(mouseLocationSimplified.ToPoint()); 
                }

            }
            if (GameInProgress == true)
            {
                if (updateValGrid > 20)
                {
                    updateValGrid = 0;
                }
                
                ProceedToNextGeneration();
                updateValGrid += 1;
            }

            previousKeyboardstate = keyboardState;
            
        }

        
       public void ProceedToNextGeneration()
        {
            if (updateValGrid == updateValueGame || GameInProgress == false)
            {
                
                Dictionary<Point, bool> NextGridOfCells = new Dictionary<Point, bool>();

                GridOfNeighbourCells = new Dictionary<Point, int>();

                foreach (var cell in GridOfCells)
                {
                    int cellNeighbours = 0;

                    cellNeighbours += CheckNeighbours(cell, -1, 0);
                    cellNeighbours += CheckNeighbours(cell, 0, -1);
                    cellNeighbours += CheckNeighbours(cell, 1, 0);
                    cellNeighbours += CheckNeighbours(cell, 0, 1);

                    cellNeighbours += CheckNeighbours(cell, 1, 1);
                    cellNeighbours += CheckNeighbours(cell, -1, -1);
                    cellNeighbours += CheckNeighbours(cell, -1, 1);
                    cellNeighbours += CheckNeighbours(cell, 1, -1);

                    if (cellNeighbours >= 2 && cellNeighbours < 4)
                    {
                        NextGridOfCells.Add(cell.Key, cell.Value);
                    }

                }
                foreach (var neighbourCell in GridOfNeighbourCells)
                {
                    if (neighbourCell.Value == 3)
                    {
                        if (NextGridOfCells.ContainsKey(neighbourCell.Key) == false)
                        {
                            NextGridOfCells.Add(neighbourCell.Key, true);
                        }
                    }

                }

                GridOfCells = NextGridOfCells;
                updateValGrid = 0;
            }
            
            


        }
        
        public int CheckNeighbours(KeyValuePair<Point, bool> cell, int directionX, int directionY)
        {
            int cellNeighbours = 0;
            Vector2 cellVector = new Vector2(cell.Key.X + directionX, cell.Key.Y + directionY);
            if (GridOfCells.ContainsKey( cellVector.ToPoint() ) == true)     
            {
                cellNeighbours += 1;
            }
            if (GridOfNeighbourCells.TryAdd(cellVector.ToPoint(), 1) == false)
            {
                GridOfNeighbourCells[cellVector.ToPoint()] += 1;
            }


                return cellNeighbours;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: CurrentView.Matrix, samplerState: SamplerState.PointClamp);


            foreach (var cell in GridOfCells)
            {
                spriteBatch.Draw(CellTexture, new Vector2((cell.Key.X * TileSize), cell.Key.Y * TileSize), null, Color.Gray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);

            }
            
            spriteBatch.Draw(CellTexture, new Vector2((mouseLocationSimplified.X * TileSize), mouseLocationSimplified.Y * TileSize), null, Color.DarkGray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);

            spriteBatch.End();
        }
        
        public void CreateGrid()
        {
            GridOfCells = new Dictionary<Point, bool>();

            CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            CellTexture.SetData(new Color[] { Color.DarkGray});
        }

        public void ReadPattern()
        {
            var textFilePath = Directory.GetCurrentDirectory() + "\\" + FileString;

            string[] text = File.ReadAllLines(textFilePath);

            List<string> DecodedText = RLEDecode(text);
             

            int lineCount = 0;

            foreach(var line in DecodedText)
            {
                
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'o')
                    {
                        Point coordinates = new Point(i, lineCount);

                        if (GridOfCells.ContainsKey(mouseLocationSimplified.ToPoint() + coordinates) == false)
                        {
                            GridOfCells.Add(mouseLocationSimplified.ToPoint() + coordinates, true);
                        }
                    }
                }
                lineCount += 1;

            }
           
            
           
            
        }
        public List<string> RLEDecode(string[] FileTextRLE)
        {
            List<string> DecodedStringFormat = new List<string>();
            string DecodedString = "";
            string NumberInString = "";

            foreach (string line in FileTextRLE)
            {
                
                for (int i = 0; i < line.Length ; i++)
                {
                    
                    if (char.IsLetter(line[i]) && line[i] != '$')
                    {
                        if (NumberInString.Length > 0)
                        {
                            DecodedString += String.Concat(Enumerable.Repeat(line[i], int.Parse(NumberInString)));
                        }
                        else
                        {
                            DecodedString += line[i];
                        }
                        NumberInString = "";
                    }
                    else if (line[i] == '$')
                    {

                        DecodedStringFormat.Add(DecodedString);
                        DecodedString = "";
                    }
                    else
                    {
                        NumberInString += line[i];
                    }
                }
            }
            return DecodedStringFormat;
        }   

    }



}
