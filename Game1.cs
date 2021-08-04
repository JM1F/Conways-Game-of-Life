using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;


namespace Conways_Game_of_Life
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatchUI;
        private Grid _grid;
        public View CurrentView;
        public string fileString { get; set; } = "";
        public Rectangle LogoRectangle;
        public bool Logohovered { get; set; } = false;
        public bool UIActive { get; set; } = false;
        public bool FileInputInProgress { get; set; } = false;
        public bool CapsLockEnabled { get; set; } = false;
        public bool ShiftEnabled { get; set; } = false;
        public string FramesPerSecondCounter { get; set; }
        public SpriteFont font;
        public Texture2D Logo;
        public KeyboardState PreviousKeyboardstate;
        public Keys[] PreviousKeysPressed = new Keys[10];
        public int updateValue { get; set; } = 1;
        /// <summary>
        /// Main method for the game class.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            IsFixedTimeStep = true;
            
        }
        /// <summary>
        /// Initialize.
        /// </summary>
        protected override void Initialize()
        {
            // Generate new Grid.
            _grid = new Grid();
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            //Generate a view for the game
            CurrentView = new View();
            _grid.Main(this, _graphics, CurrentView);
            _grid.CreateGrid();
            base.Initialize();
        }
        /// <summary>
        /// Load content of the game.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatchUI = new SpriteBatch(GraphicsDevice);
            Logo = this.Content.Load<Texture2D>("GOLLogo");
            font = this.Content.Load<SpriteFont>("ArialFont");
        }
        /// <summary>
        /// Update loop section for main game class.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            // Gets Location of mouse
            var mouseLocation = new Point(mouseState.X, mouseState.Y);
            var keyboardState = Keyboard.GetState();
            // Update the View
            CurrentView.Update(gameTime);
            // Have the rectangle dimensions follow the width of the window, even on resize
            LogoRectangle = new Rectangle(_graphics.GraphicsDevice.Viewport.Width / 2 - 150, 0, 300, 125);
            // If the Escape key is pressed the program will exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Checks to see if the update value is decreased
            if (keyboardState.IsKeyDown(Keys.Up) && PreviousKeyboardstate.IsKeyUp(Keys.Up) && FileInputInProgress == false)
            {
                // 1 is the min update value
                if (updateValue != 1 || updateValue !< 1)
                {
                    updateValue -= 1;
                }
            }
            // Checks to see if the update value is increased
            if (keyboardState.IsKeyDown(Keys.Down) && PreviousKeyboardstate.IsKeyUp(Keys.Down) && FileInputInProgress == false)
            {
                // 20 is the max update value
                if (updateValue != 20 || updateValue !> 20)
                {
                    updateValue += 1;
                }
            }
            // If "T" is pressed the UI is either toggled on/off
            if (keyboardState.IsKeyDown(Keys.T) && PreviousKeyboardstate.IsKeyUp(Keys.T) && FileInputInProgress == false)
            {
                UIActive = !UIActive;
            }
            // If "`" is pressed the file input state of the game is either on/off
            if (keyboardState.IsKeyDown(Keys.OemTilde) && PreviousKeyboardstate.IsKeyUp(Keys.OemTilde) && _grid.GameInProgress == false )
            {
                FileInputInProgress = !FileInputInProgress;
            }
            // General Caps Lock checker for the file input
            if (keyboardState.IsKeyDown(Keys.CapsLock) && PreviousKeyboardstate.IsKeyUp(Keys.CapsLock))
            {
                CapsLockEnabled = !CapsLockEnabled;
            }
            // Checks to see if either left or right shift is held down
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) )
            {
                ShiftEnabled = true;
            }
            else
            {
                ShiftEnabled = false;
            }
            // Checks to see if the file input state is enabled
            if (FileInputInProgress == true )
            {
                Keys[] CurrentPressedKeys = keyboardState.GetPressedKeys();
                // Array of exempt keys
                Keys[] ExemptKeys = { Keys.Back, Keys.LeftShift, Keys.RightShift, Keys.LeftControl, Keys.RightControl, Keys.CapsLock, Keys.OemTilde };
                Keys[] Numbers = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9};
                // Loops through each key pressed in the current pressed keys array
                foreach (Keys key in CurrentPressedKeys)
                {
                    // Makes sure one key stroke input is not mistaken for several key storkes.
                    if ( !PreviousKeysPressed.Contains(key) || PreviousKeysPressed.Contains(Keys.Back))
                    {
                        // When the back key is preseed remove one character off the file string
                        if (key == Keys.Back && fileString.Length > 0)
                        {
                            fileString = fileString.Remove(fileString.Length - 1);
                        }
                        // Checks to see if the keys are not in the exempt array and proceeds to add to the file string respective of the key pressed
                        if (!ExemptKeys.Contains(key))
                        {
                            if (key == Keys.OemPeriod)
                            {
                                fileString += ".";
                            }
                            else if (key == Keys.Space)
                            {
                                fileString += " ";
                            }
                            else if (Numbers.Contains(key))
                            {
                                // Takes the number input e.g "D0-9" and turns the second character into a string to represent the actual number
                                fileString += key.ToString()[^1];
                            }
                            else if (key == Keys.OemMinus)
                            {
                                if (CapsLockEnabled == true || ShiftEnabled == true)
                                {
                                    fileString += "_";
                                }
                                else
                                {
                                    fileString += "-";
                                }
                            }
                            else if (key == Keys.OemQuestion)
                            {
                                if (ShiftEnabled == true)
                                {
                                    fileString += "?";
                                }
                                else
                                {
                                    fileString += "/";
                                }
                            }
                            else if (key == Keys.OemPipe)
                            {
                                if (ShiftEnabled == true)
                                {
                                    fileString += "|";
                                }
                                else
                                {
                                    fileString += '\\';
                                }
                            }
                            // Basic uppercase and lowercase to check if caps or shift is enabled
                            else if (CapsLockEnabled == true || ShiftEnabled == true)
                            {
                                fileString += key.ToString().ToUpper();
                            }
                            else
                            {
                                fileString += key.ToString().ToLower();
                            }
                            
                        }
                        
                    }
                }
                // Sets the current array of keys to the previous keys for the next update loop
                PreviousKeysPressed = CurrentPressedKeys;
            }
            // Checks to see if the mouse is in the rectangle of the logo
            if (LogoRectangle.Contains(mouseLocation))
            {
                // Sets Logohovered
                Logohovered = true;
            }
            else 
            {
                Logohovered = false;    
            }
            // Sets the previous keyboard state to the current keyboard state
            PreviousKeyboardstate = keyboardState;
            // Calls the grid update function
            _grid.Update(updateValue, FileInputInProgress, fileString);

            base.Update(gameTime);
        }
        /// <summary>
        /// Draw loop for the main game class.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            // Sets the background to White
            GraphicsDevice.Clear(Color.White);
           // Calls the grid draw function
            _grid.Draw(_spriteBatch);

            _spriteBatchUI.Begin();
           // Only draws the Items and follow logic if the UI is active
            if (UIActive == false)
            {
                // Set to Running or Not Running if the game is active
                if (_grid.GameInProgress == true)
                {
                    _spriteBatchUI.DrawString(font,"Running", new Vector2(0,0), Color.Green);
                }
                else
                {
                    _spriteBatchUI.DrawString(font, "Not Running", new Vector2(0, 0), Color.Red);
                }
                // Checks to see if the logo is hovered and sets the opacity depending if the logo is being hovered over
                if (Logohovered == true)
                {
                    _spriteBatchUI.Draw(Logo, LogoRectangle, new Color(Color.White, 0.9f));
                }
                else
                {
                    _spriteBatchUI.Draw(Logo, LogoRectangle, new Color(Color.White, 0.7f));
                }
                // Draws the different parameters for the game
                _spriteBatchUI.DrawString(font, "X:" + _grid.mouseLocationSimplified.X.ToString() + "Y:" + _grid.mouseLocationSimplified.Y.ToString(), new Vector2(0, 15), Color.Black);
                _spriteBatchUI.DrawString(font,"Slow: x" + updateValue.ToString(), new Vector2(0, 30), Color.Black);
                _spriteBatchUI.DrawString(font, "File: " + fileString, new Vector2(0, 45), Color.Black);
                _spriteBatchUI.DrawString(font,"Gen: " + _grid.GenerationNumber.ToString(), new Vector2(0, 60), Color.Black);
                _spriteBatchUI.DrawString(font, "Pop: " + _grid.CellPopulation.ToString(), new Vector2(0, 75), Color.Black);
                // Draws the FPS
                _spriteBatchUI.DrawString(font, "FPS: " + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(0, 90), Color.Black);
            }
            _spriteBatchUI.End();

            base.Draw(gameTime);
        }
    }
}
