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

        public bool IsActtive { get; set; }

        public Rectangle CellRectangle { get; set; }

        public int CellIndex { get; set; }

        public Cell(Game CurrentGame,GraphicsDeviceManager CurrentGraphics, int xpos, int ypos, Texture2D celltexture, Color cellcolour, bool iscellactive, int cellindex)
        {
            Game = CurrentGame;
            Graphics = CurrentGraphics;
            XPosition = xpos;
            YPosition = ypos;
            CellTexture = celltexture;
            CellColour = cellcolour;
            IsActtive = iscellactive;
            CellIndex = cellindex;

            CellTexture.SetData(new[] { CellColour });
            CellRectangle = new Rectangle(XPosition, YPosition, 9,9);

        }

        public void DrawInput()
        {
            
        }
        public void Update(Grid grid)
        {
            var mouseState = Mouse.GetState();
            var mouseLocation = new  Point(mouseState.X,mouseState.Y);

            if (CellRectangle.Contains(mouseLocation))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    IsActtive = true;
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    IsActtive = false;
                }
                
                
            }

            




        }
    }
}
