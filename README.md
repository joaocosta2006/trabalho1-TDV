# trabalho1-TDV


João Filipe Da Silva Costa nº31489
Mário Rafael Azevedo Costa nº31467


A pasta config contém os tools do dotnet
A pasta .vscode contém o launcher do jogo
A pasta content contém os .png das sprites das cartas do jogo
A pasta Screens contém as diferentes screens do jogo e o código base do jogo em si
A pasta obj contém arquivos de build temporários gerados automaticamente pelo compilador C#. Eles servem para armazenar dados intermediários durante a construção do jogo.

Game.cs
Declaração de variáveis e campos privados:
csharp
Copiar
Editar
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

	Aqui são declaradas as variáveis necessárias para o jogo

	font: Fonte usada para renderizar o texto.

	cardBackTexture: Textura do verso das cartas.

	cardFrontTextures: Lista de texturas dos faces das cartas (imagens que mostram quando a carta é virada).

	cardPositions: Lista de posições das cartas na tela.

	cardIndices: Lista de índices que determinam qual imagem deve ser exibida em cada carta.

	firstSelectedCardIndex / secondSelectedCardIndex: Índices das duas cartas selecionadas para comparação.

	cardsFound: Lista que guarda se uma carta foi encontrada ou não.

	isCheckingCards: Flag que indica se o jogo está verificando se as duas cartas selecionadas são iguais.

	isEnded: Flag que indica se o jogo terminou.

	timer: Usado para temporizar eventos como virar cartas e o final do jogo.

	intervalInSeconds: Intervalo de tempo entre as verificações das cartas.

	score: Pontuação do jogador.


public void Initialize()
{
}
	Este método está vazio. Pode ser usado para inicializar variáveis, mas atualmente não faz nada.



public void LoadContent(ContentManager content)
{
  cardBackTexture = content.Load<Texture2D>("interrogation");
  font = content.Load<SpriteFont>("Arial");

  cardFrontTextures = [
      content.Load<Texture2D>("banana"),
      content.Load<Texture2D>("cake"),
      content.Load<Texture2D>("fries"),
      content.Load<Texture2D>("pizza"),
  ];

  cardPositions = [];
  cardIndices = [];
  // Código para calcular posições e índices das cartas
  ShuffleCards();
}
	Este método carrega todos os recursos do jogo (texturas e fontes). As texturas das cartas (frente e verso) são carregadas e as posições das cartas são calculadas. A função ShuffleCards() é chamada para baralhar as cartas.



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
    // Lógica de seleção de cartas
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


	Este método é chamado a cada quadro do jogo. Ele realiza as seguintes tarefas:

	Verifica se a tecla "Escape" é pressionada, e se for, muda para a tela do menu e reinicia o jogo.

	Processa as interações do jogador: Quando o jogador clica com o mouse, ele seleciona as cartas. A lógica verifica se as cartas selecionadas são iguais. Se forem, aumenta a pontuação e marca as cartas como encontradas.

	Gerencia o fim do jogo: Se o jogo terminar, o tempo é contado e, após 3 segundos, o jogo é reiniciado.

	Gerencia o tempo entre as verificações de cartas: Se o jogador selecionar duas cartas, um intervalo de tempo é aguardado antes de virar as cartas de volta.	


public void Draw(SpriteBatch spriteBatch)
{
  int progress = (int)Math.Round(score / (cardIndices.Count / 2.0) * 100);
  spriteBatch.DrawString(font, $"Progresso: {progress}%", new Vector2(10, 0), Color.White);

  for (int index = 0; index < cardPositions.Count; index++)
  {
    // Lógica para desenhar as cartas
  }

  if (isEnded)
  {
    // Exibe mensagem final de vitória
  }
}


	Este método desenha o conteúdo na tela:

	Exibe o progresso do jogador com base na pontuação.

	Desenha todas as cartas: Se a carta foi encontrada ou se está selecionada, a face da carta é mostrada; caso contrário, mostra o verso.

	Se o jogo terminou, exibe uma mensagem de vitória e uma contagem regressiva antes de voltar ao menu.


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
	Este método reinicia o estado do jogo:

	Marca todas as cartas como não encontradas.

	Embaralha as cartas novamente.

	Zera a pontuação, o tempo e as variáveis de estado.

	
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
	Este método embaralha as cartas, trocando aleatoriamente os índices das cartas.


private void DrawMessageOnCenter(SpriteBatch spriteBatch, string message, int yOffset = 0)
{
  int centerWidth = Globals.SCREEN_WIDTH / 2;
  int centerHeight = Globals.SCREEN_HEIGHT / 2;

  Vector2 messageSize = font.MeasureString(message);
  int messageX = centerWidth - (int)messageSize.X / 2;
  int messageY = centerHeight - (int)messageSize.Y / 2 + yOffset;

  spriteBatch.DrawString(font, message, new Vector2(messageX, messageY), Color.White);
}

	Este método desenha uma mensagem centralizada na tela.	

	

Resumo do que o código faz
Este arquivo representa uma tela de jogo (provavelmente um jogo de memória) onde o jogador precisa encontrar pares de cartas correspondentes. As cartas são exibidas com o verso virado para cima e, ao clicar, elas são viradas para mostrar suas faces. Se o jogador escolher dois pares de cartas que correspondem, ele marca essas cartas como "encontradas" e aumenta sua pontuação.

O jogo termina quando todos os pares são encontrados, e o jogador recebe uma mensagem de vitória. Após 3 segundos, o jogo retorna para o menu principal.

O código usa eventos de teclado (esc) e mouse para interagir com o jogador, temporizadores para controlar as verificações das cartas e animações de fim de jogo. As cartas são embaralhadas e distribuídas na tela de forma aleatória.

Em resumo, o código cria um jogo de memória simples onde o objetivo é combinar pares de cartas dentro de um tempo limitado e com a pontuação sendo exibida ao longo do jogo.



IScreen.cs
O ficheiro IScreen.cs define uma interface chamada IScreen, que obriga qualquer classe que a implemente a ter três métodos:
Load()

Update(GameTime gameTime)

Draw(SpriteBatch spriteBatch)

Essa interface padroniza como cada tela (por exemplo, menu, jogo ou fim de jogo) deve ser carregada, atualizada e desenhada no projeto.



Global.cs
O ficheiro Globals.cs define variáveis globais usadas em todo o projeto, principalmente:

Content: para carregar recursos (como imagens e sons).

SpriteBatch: para desenhar elementos gráficos na tela.

Essas variáveis são partilhadas entre diferentes partes do jogo para facilitar o acesso e o desenho dos conteúdos.
É uma forma de centralizar recursos comuns.



Program.cs
O ficheiro Program.cs serve apenas para iniciar o jogo. Ele cria uma instância da classe Game1 e chama Run() para começar a execução do jogo.
É um ponto de entrada padrão para aplicações feitas com MonoGame.



Menu.cs
O ficheiro Menu.cs define a tela de menu principal do jogo. Ele implementa a interface IScreen e possui:

Um botão ("Jogar") que inicia o jogo ao ser clicado.

Métodos Load(), Update(), e Draw() para carregar o botão, verificar cliques e desenhar o menu na tela.

Basicamente, ele controla a entrada inicial do jogador antes do jogo começar.



game1.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

	Importa as bibliotecas do MonoGame necessárias para o jogo funcionar

namespace MemoryGame
{
	
	define o namespace do projeto

public class Game1 : Game

	Começa a classe Game1, que é a classe principal do MonoGame.

	Esta classe é o coração que gere todo o ciclo do jogo (Initialize, LoadContent, Update, Draw).

private GraphicsDeviceManager _graphics;
private SpriteBatch _spriteBatch;

	GraphicsDeviceManager _graphics gere a janela e as definições de ecrã.

	SpriteBatch _spriteBatch é usado para desenhar imagens/texto no ecrã.

private Menu menuScreen;
private GameScreen gameScreen;
private IScreen currentScreen;

	Criam-se variáveis para os ecrãs:

	menuScreen: o menu inicial.

	gameScreen: o jogo em si.

	currentScreen: qual ecrã está ativo no momento.

public Game1()

	O construtor da classe. Aqui define-se:

	Que vai ser usado _graphics.

	A pasta dos conteúdos é Content.

	Que o rato deve ficar visível (IsMouseVisible = true).

	Define-se a resolução da janela: 640×600 pixels.


public void ChangeScreen(EScreen screen)

	Método para mudar de ecrã:

	Se o screen for MENU, muda para menuScreen.

	Se o screen for GAME, muda para gameScreen.

	Depois de mudar de ecrã, chama Initialize() para preparar o novo ecrã.


protected override void Initialize()

	Método do MonoGame chamado no início.

	Inicializa a referência global Globals.GAME_INSTANCE.

	Define as dimensões do ecrã (SCREEN_WIDTH e SCREEN_HEIGHT) nas Globals.

	Inicializa o currentScreen.

protected override void LoadContent()

	Carrega todos os conteúdos (imagens, fontes, etc).

	Cria o _spriteBatch.

	Carrega o menuScreen e o gameScreen (com LoadContent para cada um).

	Define que o primeiro ecrã ativo será o menuScreen.

protected override void Update(GameTime gameTime)

	Método que é chamado a cada frame.

	Atualiza o currentScreen (o menu ou o jogo), passando o tempo que passou (deltaTime).

	Depois chama base.Update(gameTime) para continuar o fluxo normal do MonoGame.


protected override void Draw(GameTime gameTime)

	Método que é chamado para desenhar o ecrã.

	Limpa o ecrã com uma cor azul.

Começa o desenho (_spriteBatch.Begin()).

	Manda desenhar o currentScreen (menu ou jogo).

Termina o desenho (_spriteBatch.End()).


Resumo do que Game1.cs faz:
O ficheiro Game1.cs é a classe principal do projeto.
É responsável por:

Configurar a janela do jogo.

Inicializar o jogo e os ecrãs.

Carregar conteúdos.

Atualizar o jogo a cada frame.

Desenhar no ecrã o que está a acontecer.

Mudar entre ecrãs.

Resumidamente, Game1 é o motor que faz tudo andar: trata da janela, atualiza a lógica e desenha o que o jogador vê.



Link do jogo: https://github.com/Wollace-Buarque/MemoryGame.git
