using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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

        public Cell[,] GridOfCells { get; set; }
        public bool GameInProgress { get; set; }

        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics, View view)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;
            CurrentView = view;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;
            GameInProgress = false;
        }

        public void Update()
        {

        }

        public void ProceedToNextGeneration()
        {
           
            for (int i = 0; i < GridOfCells.GetLength(0); i += TileSize)
            {
                for (int j = 0; j < GridOfCells.GetLength(1); j += TileSize)
                {
                    int cellNeighbours = 0;

                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, -TileSize, 0);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, 0, -TileSize);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, TileSize, 0);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, 0, TileSize);

                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, TileSize, TileSize);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, -TileSize, -TileSize);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, -TileSize, TileSize);
                    cellNeighbours += CheckNeighbours(GridOfCells, i, j, TileSize, -TileSize);

                    GridOfCells[i, j].NeighbourCount = cellNeighbours;
                   
                }
            }


            for (int i = 0; i < GridOfCells.GetLength(0); i += TileSize)
            {
                for (int j = 0; j < GridOfCells.GetLength(1); j += TileSize)
                {
                    if (GridOfCells[i,j].IsActtive == true)
                    {
                        if (GridOfCells[i, j].NeighbourCount < 2 || GridOfCells[i, j].NeighbourCount > 3)
                        {
                            GridOfCells[i, j].IsActtive = false;
                        }
                        if (GridOfCells[i,j].NeighbourCount >= 2 && GridOfCells[i,j].NeighbourCount < 4)
                        {
                            GridOfCells[i, j].IsActtive = true;
                        }

                    }
                    else
                    {
                        if (GridOfCells[i,j].NeighbourCount == 3)
                        {
                            GridOfCells[i, j].IsActtive = true;
                        }
                    }

                }
            }

        }
        public int CheckNeighbours(Cell[,] cellGrid, int cellPostionX, int cellPositionY  ,int conditionalTileSizeX, int conditionalTileSizeY )
        {
            int cellNeighbours = 0;
            
            if ( cellPostionX + conditionalTileSizeX < cellGrid.GetLength(0) && cellPostionX + conditionalTileSizeX  >= 0 && cellPositionY + conditionalTileSizeY < cellGrid.GetLength(1) && cellPositionY + conditionalTileSizeY >= 0)
            {
                
                if (cellGrid[cellPostionX + conditionalTileSizeX, cellPositionY + conditionalTileSizeY].IsActtive)
                {
                    cellNeighbours += 1;

                }

            }
            


            return cellNeighbours;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: CurrentView.Matrix, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < GridOfCells.GetLength(0); i+= TileSize)
            {
                for (int j = 0; j < GridOfCells.GetLength(1); j+= TileSize)
                {
                    if (GridOfCells[i, j].IsActtive == true)
                    {

                        spriteBatch.Draw(CellTexture, new Vector2((GridOfCells[i, j].XPosition), GridOfCells[i, j].YPosition), null, Color.Gray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
                    }
                    if (GridOfCells[i, j].IsHovering == true)
                    {
                        spriteBatch.Draw(CellTexture, new Vector2((GridOfCells[i, j].XPosition), GridOfCells[i, j].YPosition), null, Color.DarkGray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
                    }
                }
            }
                
            spriteBatch.End();
        }
        
        public Grid ReturnGrid()
        {
            return this;
        }

        public Cell[,] CreateGrid()
        {
            
           
            int indexofcell = 0;
            CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            CellTexture.SetData(new Color[] { Color.DarkGray});

            for (var i = 0; i <=ViewportWidth - 1; i+=TileSize)
            {
                for (var j = 0; j <= ViewportHeight - 1; j += TileSize)
                {

                    
                    GridOfCells[i,j] = new Cell(Game, Graphics, new Vector2(i , j), i, j, false, indexofcell, false);
                    indexofcell += 1;
                    
                }
            }
            
            
            return GridOfCells;
        }

    }



}
