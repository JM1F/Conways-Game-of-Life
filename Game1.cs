using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Conways_Game_of_Life
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatchLOGO;
        private Grid _grid;
        public View CurrentView;
        
        public Rectangle LogoRectangle;
        public bool Logohovered { get; set; } = false;
        public bool UIActive { get; set; } = false;
        public SpriteFont font;
        public Texture2D Logo;
        public KeyboardState PreviousKeyboardstate;
        public int updateValue { get; set; } = 0;

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

            if (keyboardState.IsKeyDown(Keys.Up) && PreviousKeyboardstate.IsKeyUp(Keys.Up))
            {
                if (updateValue != 0 || updateValue !< 0)
                {
                    updateValue -= 1;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down) && PreviousKeyboardstate.IsKeyUp(Keys.Down))
            {
                if (updateValue != 10 || updateValue !> 10)
                {
                    updateValue += 1;
                }
            }

            if (keyboardState.IsKeyDown(Keys.T) && PreviousKeyboardstate.IsKeyUp(Keys.T))
            {
                UIActive = !UIActive;
            }


            if (LogoRectangle.Contains(mouseLocation))
            {
                Logohovered = true;
            }
            else 
            {
                Logohovered = false;    
            }

            
            _grid.Update(updateValue);

            PreviousKeyboardstate = keyboardState;

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
                _spriteBatchLOGO.DrawString(font,"Slow: " + updateValue.ToString(), new Vector2(0, 30), Color.Black);
            }






            _spriteBatchLOGO.End();

            base.Draw(gameTime);
        }
    }
}
