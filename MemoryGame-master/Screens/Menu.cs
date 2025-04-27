using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Menu : IScreen
{
  private SpriteFont font;

  public void LoadContent(ContentManager content)
  {
    font = content.Load<SpriteFont>("Arial");
  }

  public void Initialize()
  {
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    DrawMessageOnCenter(spriteBatch, "Pressione ENTER para iniciar o jogo!");
    DrawMessageOnCenter(spriteBatch, "Pressione ESC para sair do jogo!", 20);
  }

  public void Update(float deltaTime)
  {
    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
    {
      Globals.GAME_INSTANCE.ChangeScreen(EScreen.GAME);
    }

    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
      Globals.GAME_INSTANCE.Exit();
    }
  }

  private void DrawMessageOnCenter(SpriteBatch spriteBatch, string message, int yOffset = 0)
  {
    int centerWidth = Globals.SCREEN_WIDTH / 2;
    int centerHeight = Globals.SCREEN_HEIGHT / 2;

    Vector2 messageSize = font.MeasureString(message);

    int messageX = centerWidth - (int)messageSize.X / 2;
    int messageY = centerHeight - (int)messageSize.Y / 2 + yOffset;

    spriteBatch.DrawString(font, message, new Vector2(messageX, messageY), Color.White);
  }
}