using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public interface IScreen 
{
  void Initialize();
  void Update(float deltaTime);
  void Draw(SpriteBatch spriteBatch);
  void LoadContent(ContentManager content);
}