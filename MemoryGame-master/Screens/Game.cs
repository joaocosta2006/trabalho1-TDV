using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameScreen : IScreen
{
  private SpriteFont font;
  private Texture2D cardBackTexture;
  private List<Texture2D> cardFrontTextures;
  private List<Vector2> cardPositions;
  private List<int> cardIndices;

  private int? firstSelectedCardIndex, secondSelectedCardIndex = null;
  private bool[] cardsFound;

  private bool isCheckingCards, isEnded = false;
  private double timer = 0;
  private const double intervalInSeconds = 1;

  private int score = 0;

  public void Initialize()
  {
  }

  public void LoadContent(ContentManager content)
  {
    cardBackTexture = content.Load<Texture2D>("interrogation");

    font = content.Load<SpriteFont>("Arial");

    cardFrontTextures =
    [
        content.Load<Texture2D>("banana"),
                content.Load<Texture2D>("cake"),
                content.Load<Texture2D>("fries"),
                content.Load<Texture2D>("pizza"),
            ];

    cardPositions = [];
    cardIndices = [];

    int rows = 4;
    int columns = 4;
    for (int row = 0; row < rows; row++)
    {
      for (int col = 0; col < columns; col++)
      {
        int cardSize = 150;
        int spacingOffset = 50;

        int cardX = col * cardSize + spacingOffset;
        int cardY = row * cardSize + spacingOffset;

        cardPositions.Add(new Vector2(cardX, cardY));

        // Calcula o índice único para cada carta no jogo
        int cardIndex = row * columns + col;

        cardIndices.Add(cardIndex);
      }
    }

    cardsFound = new bool[cardPositions.Count];
    for (int i = 0; i < cardsFound.Length; i++)
    {
      cardsFound[i] = false;
    }

    ShuffleCards();
  }

  public void Update(float deltaTime)
  {

    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
      Globals.GAME_INSTANCE.ChangeScreen(EScreen.MENU);
      ResetGame();
      return;
    }

    if (!isEnded && !isCheckingCards && Mouse.GetState().LeftButton == ButtonState.Pressed)
    {
      int mouseX = Mouse.GetState().X;
      int mouseY = Mouse.GetState().Y;

      for (int index = 0; index < cardPositions.Count; index++)
      {
        float cardLeft = cardPositions[index].X;
        float cardRight = cardPositions[index].X + cardBackTexture.Width;
        float cardTop = cardPositions[index].Y;
        float cardBottom = cardPositions[index].Y + cardBackTexture.Height;

        bool isMouseOverCard = mouseX > cardLeft && mouseX < cardRight &&
                                mouseY > cardTop && mouseY < cardBottom;

        if (isMouseOverCard)
        {
          if (firstSelectedCardIndex == null)
          {
            firstSelectedCardIndex = index;
          }
          else if (secondSelectedCardIndex == null && firstSelectedCardIndex != index)
          {
            secondSelectedCardIndex = index;
            isCheckingCards = true;

            int firstCardTextureIndex = cardIndices[(int)firstSelectedCardIndex] % cardFrontTextures.Count;
            int secondCardTextureIndex = cardIndices[(int)secondSelectedCardIndex] % cardFrontTextures.Count;
            if (firstCardTextureIndex == secondCardTextureIndex)
            {
              score++;

              if (score == cardIndices.Count / 2)
              {
                timer = 0;
                isEnded = true;
                Console.WriteLine("Parabéns! Você completou o jogo!");
              }

              cardsFound[(int)firstSelectedCardIndex] = true;
              cardsFound[(int)secondSelectedCardIndex] = true;
            }

            break;
          }
        }
      }
    }

    if (isEnded)
    {
      timer += deltaTime;

      if (timer >= 3)
      {
        Globals.GAME_INSTANCE.ChangeScreen(EScreen.MENU);
        ResetGame();
      }
    }

    // Se estiver verificando e o intervalo passou, vira as cartas de volta
    if (isCheckingCards)
    {
      timer += deltaTime;

      if (timer >= intervalInSeconds)
      {
        firstSelectedCardIndex = null;
        secondSelectedCardIndex = null;
        isCheckingCards = false;
        timer = 0;
      }
    }
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    int progress = (int)Math.Round(score / (cardIndices.Count / 2.0) * 100);

    spriteBatch.DrawString(font, $"Progresso: {progress}%", new Vector2(10, 0), Color.White);

    for (int index = 0; index < cardPositions.Count; index++)
    {
      if (cardsFound[index] || index == firstSelectedCardIndex || index == secondSelectedCardIndex)
      {
        int selectedCardFrontTextureIndex = cardIndices[index] % cardFrontTextures.Count;

        spriteBatch.Draw(cardFrontTextures[selectedCardFrontTextureIndex], cardPositions[index], Color.White);
      }
      else
      {
        spriteBatch.Draw(cardBackTexture, cardPositions[index], Color.White);
      }
    }

    if (isEnded)
    {
      spriteBatch.Draw(cardBackTexture, new Rectangle(0, 0, Globals.SCREEN_WIDTH, Globals.SCREEN_HEIGHT), Color.Black * 0.5f);

      int exitSeconds = (int)Math.Round(3 - timer);

      DrawMessageOnCenter(spriteBatch, "Parabéns! Você completou o jogo!", 0);

      if (exitSeconds == 0) DrawMessageOnCenter(spriteBatch, "Saindo...", 50);
      else DrawMessageOnCenter(spriteBatch, $"Saindo em {exitSeconds} segundos...", 50);
    }
  }

  private void ResetGame()
  {
    for (int i = 0; i < cardsFound.Length; i++)
    {
      cardsFound[i] = false;
    }

    ShuffleCards();

    firstSelectedCardIndex = null;
    secondSelectedCardIndex = null;
    isCheckingCards = false;
    isEnded = false;
    timer = 0;
    score = 0;
  }

  private void ShuffleCards()
  {
    Random rng = new();
    int n = cardIndices.Count;
    while (n > 1)
    {
      n--;
      int k = rng.Next(n + 1);
      (cardIndices[n], cardIndices[k]) = (cardIndices[k], cardIndices[n]);
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