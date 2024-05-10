using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using System.IO;

namespace TDJ_PJ2;

public class GameScene : IScene
{
    #region Event related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion

    #region Fields
    //public EntityManager Entities;
    //public SpawnManager Spawner;
    //public CollisionManager Collision;
    
    //private ScoreManager m_Score;
    private bool m_IsPaused;
    private string m_PauseText, m_ToMenuText;
    private int nrLinhas = 0;
    private int nrColunas = 0;

    public int tileSize = 64;
    public char[,] level;

    private KeyboardState m_CurrentState, m_PreviousState;
    private Texture2D floor, path;
    private GraphicsDevice _graphicsDevice;
    private GraphicsDeviceManager _graphics;
    public List<Point> boxes;
    #endregion

    #region Constructor
    public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
    {

        _graphicsDevice = graphicsDevice;
        _graphics = graphics;

        //Entities = new EntityManager();
        //Spawner = new SpawnManager(Entities, new Vector2(64.0f, 0.0f));
        //Collision = new CollisionManager();

        //m_Score = score;

        m_IsPaused = false;
        m_PauseText = "PAUSED";
        m_ToMenuText = "[M] MENU";

        m_CurrentState = Keyboard.GetState();
        m_PreviousState = m_CurrentState;

        // Render texture background
        floor = AssetManager.Instance().GetTile("Grass");
        path = AssetManager.Instance().GetTile("WitheredGrass");

        _graphics = graphics;

    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = Keyboard.GetState();

        // Pauses the game
        if(m_CurrentState.IsKeyDown(Keys.P) && m_PreviousState.IsKeyUp(Keys.P))
            m_IsPaused = !m_IsPaused;
        // From Game to Menu
        else if(m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
            SceneChangeEvent?.Invoke(SceneType.Menu);

        // Updates the below managers when the game is not paused
        if(m_IsPaused) return;

        //Entities.Update(gameTime);
        //Spawner.Update();
        //m_Score.Update();
    }
    
    public void Render(SpriteBatch spriteBatch)
    {
        // Entities render
        //Entities.Render(spriteBatch);

        LoadLevel("level1.txt");

        _graphics.PreferredBackBufferHeight = tileSize * (1 + level.GetLength(1));
        _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0);

        Rectangle position = new Rectangle(0, 0, tileSize, tileSize); //calculo do retangulo a depender do tileSize
        for (int x = 0; x < level.GetLength(0); x++) //pega a primeira dimens�o
        {
            for (int y = 0; y < level.GetLength(1); y++) //pega a segunda dimens�o
            {
                position.X = x * tileSize;
                position.Y = y * tileSize;

                switch (level[x, y])
                {
                    case '#':
                        spriteBatch.Draw(floor, position, Color.White);
                    break;
                    case 'C':
                        spriteBatch.Draw(path, position, Color.White);
                    break;
                }
            }
        }

        // Draw text Rounds, Health, Money
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Rounds: ", new Vector2(10.0f, 10.0f), Color.White);
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Health: ", new Vector2(10.0f, 60.0f), Color.White);
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Money: ", new Vector2(10.0f, 110.0f), Color.White);

        // Pause menu
        if (m_IsPaused) PauseMenu(spriteBatch);
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