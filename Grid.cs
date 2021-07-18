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

        public List<Cell> GridOfCells { get; set; }

        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;

            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;

        }

        public void Update()
        {
            
           

        }
        
        public Grid ReturnGrid()
        {
            return this;
        }

        
        public List<Cell> CreateGrid()
        {
            GridOfCells = new List<Cell>();
            
            int indexofcell = 0;
            Texture2D CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);


            for (var i = 0; i <=ViewportWidth; i+=TileSize)
            {
                for (var j = 0; j <= ViewportHeight; j += TileSize)
                {
                    indexofcell += 1;

                    GridOfCells.Add(new Cell(Game, Graphics, i, j, CellTexture , Color.Purple, false, indexofcell));

                }
            }

            return GridOfCells;
        }

    }



}
