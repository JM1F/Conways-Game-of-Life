
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Conways_Game_of_Life
{
    /// <summary>
    /// Class for the view of the game which is moveable and has zoom functions.
    /// </summary>
    public class View
    {
        public Matrix InverseMatrix { get; private set; }
        public float Zoom { get; private set; } = 1;
        public Matrix Matrix { get; private set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        private MouseState oldMouse;
        /// <summary>
        /// Main update loop for the View class.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            var mouse = Mouse.GetState();
            // Gets the value of which direction the mouse is scrolling
            int scrollValue = mouse.ScrollWheelValue - oldMouse.ScrollWheelValue;

            // Checks the scroll value and either zooms in or zooms out depending on the input.
            if (scrollValue < 0)
            {
                Zoom /= 2;
                if (Zoom < 0.2f)
                {
                    Zoom = 0.2f;
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
                // Finds the difference in position from the old mouse location and the new
                posX = mouse.Position.X - oldMouse.Position.X;
                posY = mouse.Position.Y - oldMouse.Position.Y;
                // Moves the Matrix position X,Y values
                PositionX += posX * 60 / (Zoom * 0.5f) * (float)time.ElapsedGameTime.TotalSeconds;
                PositionY += posY * 60 / (Zoom * 0.5f) * (float)time.ElapsedGameTime.TotalSeconds;
            }
             // Makes the Matrix movement with a zoom scale
             Matrix = Matrix.CreateTranslation(PositionX, PositionY, 0) * Matrix.CreateScale(Zoom);
            
            InverseMatrix = Matrix.Invert(Matrix);

            oldMouse = mouse;

        }

        


    }
}
