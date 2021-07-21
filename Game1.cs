using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conways_Game_of_Life
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Grid _grid;
        public View CurrentView;
        public Cell[,] GridList;
        

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

            GridList = _grid.CreateGrid();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentView.Update(gameTime, GraphicsDevice.Viewport, Window);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _grid.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _grid.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
