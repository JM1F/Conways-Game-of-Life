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

        public Dictionary<Point, bool> GridOfCells { get; set; }
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

        
       
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: CurrentView.Matrix, samplerState: SamplerState.PointClamp);

            

            //spriteBatch.Draw(CellTexture, new Vector2((GridOfCells[i, j].XPosition), GridOfCells[i, j].YPosition), null, Color.Gray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
                    
                    
                    
            //spriteBatch.Draw(CellTexture, new Vector2((GridOfCells[i, j].XPosition), GridOfCells[i, j].YPosition), null, Color.DarkGray, 0, Vector2.Zero, TileSize - 2, SpriteEffects.None, 0);
                    
                
            
                
            spriteBatch.End();
        }
        
        public Grid ReturnGrid()
        {
            return this;
        }

        public void CreateGrid()
        {
            GridOfCells = new Dictionary<Point, bool>();
            
            
            CellTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            CellTexture.SetData(new Color[] { Color.DarkGray});

         
        }

    }



}
