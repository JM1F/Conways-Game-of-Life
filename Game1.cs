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
        
        private Texture2D rect;

        public List<Cell> GridList;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            _grid = new Grid();

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            _grid.Main(this, _graphics);
            GridList = _grid.CreateGrid();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Orange});
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _grid.Update();
            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            

            foreach(Cell cells in GridList)
            {
                _spriteBatch.Draw(cells.CellTexture, new Rectangle(cells.XPosition, cells.YPosition, 9, 9), cells.CellColour);
            }



            _spriteBatch.End();
            // TODO: Add your drawing code here
           
            base.Draw(gameTime);
        }
    }
}
