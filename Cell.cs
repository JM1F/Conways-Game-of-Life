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
        

        public bool IsActtive { get; set; }

        public Rectangle CellRectangle { get; set; }

        public int CellIndex { get; set; }

        public Vector2 CellVector { get; set; }

        public int TileSize = 10;

        public bool IsHovering { get; set; }


        public Cell(Game CurrentGame,GraphicsDeviceManager CurrentGraphics, Vector2 vector ,int xpos, int ypos,  bool iscellactive, int cellindex, bool iscellhovered)
        {
            Game = CurrentGame;
            Graphics = CurrentGraphics;
            XPosition = xpos;
            YPosition = ypos;
            CellVector = vector;
            IsActtive = iscellactive;
            CellIndex = cellindex;
            IsHovering = iscellhovered;

            CellVector = new Vector2(MathF.Floor(CellVector.X / TileSize), MathF.Floor(CellVector.Y / TileSize));
            
        }

        public void DrawInput()
        {
            
        }
        public void Update(Grid grid)
        {
            

            




        }
    }
}
