using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Conways_Game_of_Life
{
    public class Cell
    {
        public Game Game;
        public GraphicsDeviceManager Graphics;

        public int XPosition;
        public int YPosition;
        public Texture2D CellTexture { get; set; }
        public Color CellColour { get; set; }

        public Cell(Game CurrentGame,GraphicsDeviceManager CurrentGraphics, int xpos, int ypos, Texture2D celltexture, Color cellcolour)
        {
            Game = CurrentGame;
            Graphics = CurrentGraphics;
            XPosition = xpos;
            YPosition = ypos;
            CellTexture = celltexture;
            CellColour = cellcolour;

            CellTexture.SetData(new[] { CellColour });
        }

        public void DrawInput()
        {
            
        }
    }
}
