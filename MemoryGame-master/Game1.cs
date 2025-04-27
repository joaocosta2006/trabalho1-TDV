using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MemoryGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Menu menuScreen;
        private GameScreen gameScreen;
        private IScreen currentScreen;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 600;
        }

        public void ChangeScreen(EScreen screen)
        {
            switch (screen)
            {
                case EScreen.MENU:
                    currentScreen = menuScreen;
                    break;
                case EScreen.GAME:
                    currentScreen = gameScreen;
                    break;
            }

            currentScreen.Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            Globals.GAME_INSTANCE = this;
            Globals.SCREEN_WIDTH = _graphics.PreferredBackBufferWidth;
            Globals.SCREEN_HEIGHT = _graphics.PreferredBackBufferHeight;

            currentScreen.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            menuScreen = new Menu();
            menuScreen.LoadContent(Content);

            gameScreen = new GameScreen();
            gameScreen.LoadContent(Content);

            currentScreen = menuScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            currentScreen.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            currentScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
