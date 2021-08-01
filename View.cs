
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Conways_Game_of_Life
{
    public class View
    {
        public Matrix InverseMatrix { get; private set; }

        public float Zoom { get; private set; } = 1;
        public float oldzoom { get; private set; }
        public Matrix Matrix { get; private set; }

        public Matrix TrueMatrix { get; private set; }
        
        public int posX { get; set; }
        public int posY { get; set; }

        public float PositionX { get; set; }
        public float PreviousPositionX { get; set; }
        public float PreviousPositionY { get; set; }
        public float PositionY { get; set; }
        private MouseState oldMouse;
        public Point pos { get; set; }

        public void Update(GameTime time, Viewport viewport, GameWindow window)
        {

            var mouse = Mouse.GetState();

            int scrollValue = mouse.ScrollWheelValue - oldMouse.ScrollWheelValue;

            if (scrollValue < 0)
            {
                Zoom /= 0.5f;
                if (Zoom > 0.25f)
                {
                    Zoom = 0.25f;
                }
            }

            if (scrollValue > 0)
            {
                Zoom *= 2;
                if (Zoom > 16)
                {
                    Zoom = 16;
                }
            }

            if (mouse.MiddleButton == ButtonState.Pressed)
            {
               
                posX = mouse.Position.X - oldMouse.Position.X;
                posY = mouse.Position.Y - oldMouse.Position.Y;

                
                PositionX += posX * 60 / (Zoom * 0.5f) * (float)time.ElapsedGameTime.TotalSeconds;
                PositionY += posY * 60 / (Zoom * 0.5f) * (float)time.ElapsedGameTime.TotalSeconds;
                
            }
           
            
             Matrix = Matrix.CreateTranslation(PositionX, PositionY, 0) * Matrix.CreateScale(Zoom);
            

            InverseMatrix = Matrix.Invert(Matrix);

            oldMouse = mouse;
            TrueMatrix = Matrix;

            oldzoom = Zoom;
            PreviousPositionX = PositionX;
            PreviousPositionY = PositionY;
        }

        


    }
}
