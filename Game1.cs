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
        public Cell[,] GridList;
        public Rectangle LogoRectangle;
        public bool Logohovered { get; set; } = false;
        public bool UIActive { get; set; } = false;
        public SpriteFont font;
        public Texture2D Logo;
        public KeyboardState PreviousKeyboardstate;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

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
            LogoRectangle = new Rectangle(490, 0, 300, 125);

            font = this.Content.Load<SpriteFont>("ArialFont");
        }

        protected override void Update(GameTime gameTime)
        {

            var mouseState = Mouse.GetState();
            var mouseLocation = new Point(mouseState.X, mouseState.Y);

            var keyboardState = Keyboard.GetState();

            CurrentView.Update(gameTime, GraphicsDevice.Viewport, Window);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            

            _grid.Update();
            
            base.Update(gameTime);

            PreviousKeyboardstate = keyboardState;
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
            }
            

            
            
            

            _spriteBatchLOGO.End();

            base.Draw(gameTime);
        }
    }
}
