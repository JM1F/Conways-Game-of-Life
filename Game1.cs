using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace Conways_Game_of_Life
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatchLOGO;
        private Grid _grid;
        public View CurrentView;
        public string fileString { get; set; } = "";

        public Rectangle LogoRectangle;
        public bool Logohovered { get; set; } = false;
        public bool UIActive { get; set; } = false;
        public bool FileInputInProgress { get; set; } = false;
        public bool CapsLockEnabled { get; set; } = false;
        public bool ShiftEnabled { get; set; } = false;

        public SpriteFont font;
        public Texture2D Logo;
        public KeyboardState PreviousKeyboardstate;
        public Keys[] PreviousKeysPressed = new Keys[10];
 
        public int updateValue { get; set; } = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            IsFixedTimeStep = true;
            
        }
        protected override void Initialize()
        {

            _grid = new Grid();

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            CurrentView = new View();
            _grid.Main(this, _graphics, CurrentView);

            _grid.CreateGrid();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatchLOGO = new SpriteBatch(GraphicsDevice);
            Logo = this.Content.Load<Texture2D>("GOLLogo");
            

            font = this.Content.Load<SpriteFont>("ArialFont");
        }

        protected override void Update(GameTime gameTime)
        {
            
            var mouseState = Mouse.GetState();
            var mouseLocation = new Point(mouseState.X, mouseState.Y);

            var keyboardState = Keyboard.GetState();

            CurrentView.Update(gameTime, GraphicsDevice.Viewport, Window);

            LogoRectangle = new Rectangle(_graphics.GraphicsDevice.Viewport.Width / 2 - 150, 0, 300, 125);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Up) && PreviousKeyboardstate.IsKeyUp(Keys.Up) && FileInputInProgress == false)
            {
                if (updateValue != 1 || updateValue !< 1)
                {
                    updateValue -= 1;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down) && PreviousKeyboardstate.IsKeyUp(Keys.Down) && FileInputInProgress == false)
            {
                if (updateValue != 20 || updateValue !> 20)
                {
                    updateValue += 1;
                }
            }

            if (keyboardState.IsKeyDown(Keys.T) && PreviousKeyboardstate.IsKeyUp(Keys.T) && FileInputInProgress == false)
            {
                UIActive = !UIActive;
            }
            if (keyboardState.IsKeyDown(Keys.OemTilde) && PreviousKeyboardstate.IsKeyUp(Keys.OemTilde) && _grid.GameInProgress == false )
            {
                FileInputInProgress = !FileInputInProgress;
            }

            if (keyboardState.IsKeyDown(Keys.CapsLock) && PreviousKeyboardstate.IsKeyUp(Keys.CapsLock))
            {
                CapsLockEnabled = !CapsLockEnabled;
            }
            
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) )
            {
                ShiftEnabled = true;
            }
            else
            {
                ShiftEnabled = false;
            }

            if (FileInputInProgress == true )
            {
                Keys[] CurrentPressedKeys = keyboardState.GetPressedKeys();
                Keys[] ExemptKeys = { Keys.Back, Keys.LeftShift, Keys.RightShift, Keys.LeftControl, Keys.RightControl, Keys.CapsLock, Keys.OemTilde };
                Keys[] Numbers = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9};
                
                foreach (Keys key in CurrentPressedKeys)
                {
                    if ( !PreviousKeysPressed.Contains(key) || PreviousKeysPressed.Contains(Keys.Back))
                    {
                        if (key == Keys.Back && fileString.Length > 0)
                        {
                            fileString = fileString.Remove(fileString.Length - 1);
                        }
                        if (!ExemptKeys.Contains(key) && fileString.Length <= 25)
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
                PreviousKeysPressed = CurrentPressedKeys;

            }

            if (LogoRectangle.Contains(mouseLocation))
            {
                Logohovered = true;
            }
            else 
            {
                Logohovered = false;    
            }

            PreviousKeyboardstate = keyboardState;
            _grid.Update(updateValue, FileInputInProgress, fileString);

            

            base.Update(gameTime);

            
        }
        

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.White);

           
            _grid.Draw(_spriteBatch);
                
            

            _spriteBatchLOGO.Begin();
           
            if (UIActive == false)
            {
                if (_grid.GameInProgress == true)
                {
                    _spriteBatchLOGO.DrawString(font,"Running", new Vector2(0,0), Color.Green);
                }
                else
                {
                    _spriteBatchLOGO.DrawString(font, "Not Running", new Vector2(0, 0), Color.Red);
                }

                if (Logohovered == true)
                {
                    _spriteBatchLOGO.Draw(Logo, LogoRectangle, new Color(Color.White, 0.9f));
                }
                else
                {
                    _spriteBatchLOGO.Draw(Logo, LogoRectangle, new Color(Color.White, 0.7f));
                }

                _spriteBatchLOGO.DrawString(font, "X:" + _grid.mouseLocationSimplified.X.ToString() + "Y:" + _grid.mouseLocationSimplified.Y.ToString(), new Vector2(0, 15), Color.Black);
                _spriteBatchLOGO.DrawString(font,"Slow: x" + updateValue.ToString(), new Vector2(0, 30), Color.Black);
                _spriteBatchLOGO.DrawString(font, "File: " + fileString, new Vector2(0, 45), Color.Black);

            }






            _spriteBatchLOGO.End();

            base.Draw(gameTime);
        }
    }
}
