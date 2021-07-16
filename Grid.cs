using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Conways_Game_of_Life
{
    
    class Grid
    {
        public Game Game;
        public GraphicsDeviceManager Graphics;

        public int ViewportWidth;
        public int ViewportHeight;
        

        public void Main(Game CurrentGame, GraphicsDeviceManager CurentGraphics)
        {
            Game = CurrentGame;
            Graphics = CurentGraphics;

        }

        public void Update()
        {
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;

            Debug.WriteLine(ViewportHeight);
            Debug.WriteLine(ViewportWidth);

            
            
            
        }
        
        public void Draw()
        {
            


        }
        
    }



}
