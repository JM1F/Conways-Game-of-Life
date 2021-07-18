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
        
        public List<Cell> GridList;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

        }

        protected override void Initialize()
        {
            

            // TODO: Add your initialization logic here
            base.Initialize();

            _grid = new Grid();
            _grid.Main(this, _graphics);

            GridList = _grid.CreateGrid();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                foreach (Cell cells in GridList)
                {
                    cells.IsActtive = false;
                }

            
            // TODO: Add your update logic here
            foreach (Cell cells in GridList)
            {
                cells.Update(_grid.ReturnGrid());
            }
            

            
        }

        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            

            foreach(Cell cells in GridList)
            {
                if (cells.IsActtive == true)
                {
                    _spriteBatch.Draw(cells.CellTexture, cells.CellRectangle, cells.CellColour);
                }
                
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here
           
            
        }
    }
}
