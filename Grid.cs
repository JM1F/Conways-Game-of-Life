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

         
        public List<Cell> GridOfCells { get; set; }

        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics, View view)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;
            CurrentView = view;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;

        }

        public void Update()
        {
            var mouseState = Mouse.GetState();


            var mouseLocation = Vector2.Transform(mouseState.Position.ToVector2(), CurrentView.InverseMatrix);

            mouseLocation = new Vector2(MathF.Floor(mouseLocation.X / TileSize), MathF.Floor(mouseLocation.Y / TileSize));


            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                foreach (var cell in GridOfCells)
                {
                    cell.IsActtive = false;
                }
            }
            foreach (var cell in GridOfCells)
            {
                
                if (cell.CellVector == mouseLocation )
                {
                    if (cell.IsActtive == false)
                    {
                        cell.IsHovering = true;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        cell.IsActtive = true;
                        
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        cell.IsActtive = false;
                    }
                    
                }
                else
                {
                    cell.IsHovering = false;
                }


            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: CurrentView.Matrix, samplerState: SamplerState.PointClamp);
            foreach (var cell in GridOfCells)
            {
                if (cell.IsActtive == true)
                {
                    
                    spriteBatch.Draw(CellTexture, new Vector2((cell.XPosition), cell.YPosition), null, Color.Purple, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0 );
                }
                if (cell.IsHovering == true)
                {
                    spriteBatch.Draw(CellTexture, new Vector2((cell.XPosition), cell.YPosition), null, Color.Orange, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
                }

            }
            spriteBatch.End();
        }
        
        public Grid ReturnGrid()
        {
            return this;
        }

        
        public List<Cell> CreateGrid()
        {
            GridOfCells = new List<Cell>();
            
            int indexofcell = 0;
            CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            CellTexture.SetData(new Color[] { Color.Purple});

            for (var i = 0; i <=ViewportWidth; i+=TileSize)
            {
                for (var j = 0; j <= ViewportHeight; j += TileSize)
                {

                    
                    GridOfCells.Add(new Cell(Game, Graphics, new Vector2(i , j), i, j, false, indexofcell, false));
                    indexofcell += 1;
                }
            }

            return GridOfCells;
        }

    }



}
