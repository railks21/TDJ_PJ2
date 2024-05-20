using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using TDJ_PJ2.src.Entities;

namespace TDJ_PJ2
{
    public class GameScene : IScene
    {
        #region Event related
        public delegate void SceneChange(SceneType sceneType);
        public static event SceneChange SceneChangeEvent;
        #endregion

        #region Fields
        private bool m_IsPaused;
        private string m_PauseText, m_ToMenuText;
        private int nrLinhas = 0;
        private int nrColunas = 0;

        public int tileSize = 64;
        public char[,] level;

        private KeyboardState m_CurrentState, m_PreviousState;
        private Texture2D floor, path, player1;
        private GraphicsDevice _graphicsDevice;
        private GraphicsDeviceManager _graphics;
        private List<Point> boxes;
        private EntityManager Entities;
        private SpawnManager Spawner;

        //player
        private Player player;
        private Vector2 playerPosition;


        // depois apagar caso o player nao recebe um vector2
        // Placeholder position of the player
        // private Vector2 playerStartPosition = new Vector2(64f, 64f);


        //private Texture2D player_sprite;
        // utilizo o 'player4' para a texture2D
        #endregion

        #region Constructor
        public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;

            m_IsPaused = false;
            m_PauseText = "PAUSED";
            m_ToMenuText = "[M] MENU";

            m_CurrentState = Keyboard.GetState();
            m_PreviousState = m_CurrentState;

            // Render texture background
            floor = AssetManager.Instance().GetTile("Grass");
            path = AssetManager.Instance().GetTile("WitheredGrass");
            player1 = AssetManager.Instance().GetSprite("Player1");

            _graphics = graphics;

            LoadLevel("level1.txt");
            Entities = new EntityManager();
            Spawner = new SpawnManager(level, Entities);
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            m_PreviousState = m_CurrentState;
            m_CurrentState = Keyboard.GetState();

            // Pauses the game
            if (m_CurrentState.IsKeyDown(Keys.P) && m_PreviousState.IsKeyUp(Keys.P))
                m_IsPaused = !m_IsPaused;
            // From Game to Menu
            else if (m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
                SceneChangeEvent?.Invoke(SceneType.Menu);

            // Updates the below managers when the game is not paused
            if (m_IsPaused) return;

            // Update the SpawnManager
            Spawner.Update(gameTime);

            // Update the EntityManager
            Entities.Update(gameTime);

            // Player
            player.Update(gameTime);
        }

        public void Render(SpriteBatch spriteBatch)
        {

            _graphics.PreferredBackBufferHeight = tileSize * (1 + level.GetLength(1));
            _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0);

            Rectangle position = new Rectangle(0, 0, tileSize, tileSize); //calculo do retangulo a depender do tileSize
            for (int x = 0; x < level.GetLength(0); x++) //pega a primeira dimensão
            {
                for (int y = 0; y < level.GetLength(1); y++) //pega a segunda dimensão
                {
                    position.X = x * tileSize;
                    position.Y = y * tileSize;

                    switch (level[x, y])
                    {
                        case '#' or 'P':
                            spriteBatch.Draw(floor, position, Color.White);
                        break;
                        case 'C' or 'S' or 'F' :
                            spriteBatch.Draw(path, position, Color.White);
                        break;
                    }
                }
            }

            // Draw text Rounds, Health, Money
            spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Rounds: ", new Vector2(10.0f, 10.0f), Color.White);
            spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Health: ", new Vector2(10.0f, 60.0f), Color.White);
            spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Money: ", new Vector2(10.0f, 110.0f), Color.White);

            // Render the entities
            Entities.Render(spriteBatch);

            // Pause menu
            if (m_IsPaused) PauseMenu(spriteBatch);

            //Player 
            player.Draw(spriteBatch);
        }

        public void LoadLevel(string levelFile)
        {
            boxes = new List<Point>();
            string[] linhas = File.ReadAllLines($"src/Levels/{levelFile}"); // "Content/" + level
            nrLinhas = linhas.Length;
            nrColunas = linhas[0].Length;

            level = new char[nrColunas, nrLinhas];

            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    level[x, y] = linhas[y][x];

                    //player
                    if (linhas[y][x] == 'P')
                    {
                        playerPosition = new Vector2(x, y);
                        player = new Player(player1,playerPosition);
                    }

                }
            }
        }

        private void PauseMenu(SpriteBatch spriteBatch)
        {
            SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
            SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");

            // Pause title render
            spriteBatch.DrawString(largeFont, m_PauseText, Game1.CenterText(largeFont, m_PauseText) - new Vector2(0.0f, 50.0f), Color.CadetBlue);

            // To menu text render
            spriteBatch.DrawString(mediumFont, m_ToMenuText, Game1.CenterText(mediumFont, m_ToMenuText), Color.CadetBlue);
        }
        #endregion
    }
}