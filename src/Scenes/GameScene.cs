using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    private bool m_IsPaused;
    private string m_PauseText, m_ToMenuText;
    private int nrLinhas = 0;
    private int nrColunas = 0;

    public int tileSize = 64;
    public char[,] level;

    private KeyboardState m_CurrentState, m_PreviousState;
    private Texture2D floor, path, sideImage, turret, projectileTexture, circleTexture, player1;
    private GraphicsDevice _graphicsDevice;
    private GraphicsDeviceManager _graphics;
    private List<Point> boxes;
    private List<Tower> towers;
    private Tower tower;
    private EntityManager Entities;
    private SpawnManager Spawner;

    private bool isDraggingTower = false;
    private Point towerPosition; // A posição atual da torre enquanto está sendo arrastada
    private Rectangle towerSourceRectangle; // A área onde a torre está desenhada no painel lateral

    //player
    private Player player;
    private Vector2 playerPosition;
    public int LevelWidth => level.GetLength(0);
    public int LevelHeight => level.GetLength(1);
    public int TileSize => tileSize;
    #endregion

    #region Constructor
    public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;
        _graphics = graphics;
        towers = new List<Tower>();

        m_IsPaused = false;
        m_PauseText = "PAUSED";
        m_ToMenuText = "[M] MENU";

        m_CurrentState = Keyboard.GetState();
        m_PreviousState = m_CurrentState;

        // Render texture background
        floor = AssetManager.Instance().GetTile("Grass");
        path = AssetManager.Instance().GetTile("WitheredGrass");
        sideImage = AssetManager.Instance().GetTile("SideImage");
        turret = AssetManager.Instance().GetSprite("Turret");
        projectileTexture = AssetManager.Instance().GetSprite("TurretBullet");
        player1 = AssetManager.Instance().GetSprite("Player1");
        towerSourceRectangle = new Rectangle(0, 0, turret.Width, turret.Height);

        LoadLevel("level1.txt");
        Entities = new EntityManager();
        Spawner = new SpawnManager(level, Entities);
        Tower tower = new Tower(turret, new Vector2(100, 100), projectileTexture);

        circleTexture = CreateCircleTexture(graphicsDevice, (int)tower.Range);
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        // Pauses the game
        if (m_CurrentState.IsKeyDown(Keys.P) && m_PreviousState.IsKeyUp(Keys.P))
            m_IsPaused = !m_IsPaused;
        // From Game to Menu
        else if (m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
            SceneChangeEvent?.Invoke(SceneType.Menu);

        // Updates the below managers when the game is not paused
        if (m_IsPaused) return;

        // Verify click on mouse Tower
        int levelWidthInPixels = tileSize * level.GetLength(0);
        Rectangle relativeTowerRectangle = new Rectangle(levelWidthInPixels + 135, 250, turret.Width, turret.Height);
        if (mouseState.LeftButton == ButtonState.Pressed && !isDraggingTower)
        {
            if (relativeTowerRectangle.Contains(mouseState.Position))
            {
                isDraggingTower = true;
            }
        }

        // Update position tower
        if (isDraggingTower)
        {
            // Update position from mouse
            towerPosition = mouseState.Position;

            if (mouseState.LeftButton == ButtonState.Released)
            {
                // Drop tower on map
                PlaceTowerOnMap(towerPosition);
                isDraggingTower = false;
            }
        }

        // Update towers
        foreach (var tower in towers)
        {
            tower.Update(gameTime, Entities);
        }

        // Player
        player.Update(gameTime, Entities);

        // Update the SpawnManager
        Spawner.Update(gameTime);

        // Update the EntityManager
        Entities.Update(gameTime);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        _graphics.PreferredBackBufferHeight = tileSize * (1 + level.GetLength(1));
        _graphics.PreferredBackBufferWidth = tileSize * (level.GetLength(0));

        Rectangle position = new Rectangle(0, 0, tileSize, tileSize);
        for (int x = 0; x < level.GetLength(0); x++)
        {
            for (int y = 0; y < level.GetLength(1); y++)
            {
                position.X = x * tileSize;
                position.Y = y * tileSize;

                switch (level[x, y])
                {
                    case '#' or 'P':
                        spriteBatch.Draw(floor, position, Color.White);
                        break;
                    case 'C' or 'S' or 'F':
                        spriteBatch.Draw(path, position, Color.White);
                        break;
                }
            }
        }

        // Draw text Rounds, Health
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Rounds: " + Spawner.Rounds + "/" + Spawner.MaxRounds, new Vector2(10.0f, 10.0f), Color.White);
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Health: " + Entities.Health, new Vector2(10.0f, 60.0f), Color.White);

        // Draw the side image
        int levelWidthInPixels = tileSize * level.GetLength(0);
        int sideImageWidthInPixels = 6 * tileSize;
        int sideImageHeightInTiles = 20;
        Rectangle sideImagePosition = new Rectangle(levelWidthInPixels, 0, sideImageWidthInPixels, tileSize * sideImageHeightInTiles);
        spriteBatch.Draw(sideImage, sideImagePosition, Color.White);

        // Draw text Money
        Vector2 textPosition = new Vector2(levelWidthInPixels + 10, 10);
        spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Money: " + Tower.Money, textPosition, Color.White);

        // Draw text Turrets
        if (!isDraggingTower)
        {
            Vector2 turretPosition = new Vector2(levelWidthInPixels + 135, 250);
            Vector2 costPosition = new Vector2(levelWidthInPixels + 100, 320);

            spriteBatch.DrawString(AssetManager.Instance().GetFont("Medium"), "Cost: " + "350", costPosition, Color.White);
            spriteBatch.Draw(turret, turretPosition, Color.White);
        }

        // Draw Tower on map
        if (isDraggingTower)
        {
            // Calculate coordinates mouse
            int tileX = towerPosition.X / tileSize;
            int tileY = towerPosition.Y / tileSize;

            // Calculate position
            Vector2 towerDrawPosition = new Vector2(tileX * tileSize, tileY * tileSize);

            // Draw Tower 
            spriteBatch.Draw(turret, towerDrawPosition, Color.White);

            // Draw Range Circle
            Vector2 circleDrawPosition = new Vector2(towerDrawPosition.X + (turret.Width / 2) - (circleTexture.Width / 2),
                                                     towerDrawPosition.Y + (turret.Height / 2) - (circleTexture.Height / 2));
            spriteBatch.Draw(circleTexture, circleDrawPosition, Color.White * 0.5f); // Adjust transparency as needed
        }

        Vector2 averageZombiePosition = Vector2.Zero;
        int activeZombiesCount = 0;

        // Calcula a posição média dos zumbis ativos
        foreach (var entity in Entities.Entities)
        {
            if (entity is Zombie zombie && zombie.IsActive)
            {
                averageZombiePosition += zombie.Position;
                activeZombiesCount++;
            }
        }

        if (activeZombiesCount > 0)
        {
            averageZombiePosition /= activeZombiesCount;
        }

        // Desenha as torres e seus projéteis
        foreach (var tower in towers)
        {
            tower.Draw(spriteBatch, averageZombiePosition);
        }

        // Player 
        player.Draw(spriteBatch);

        // Render the entities
        Entities.Render(spriteBatch);

        // Pause menu
        if (m_IsPaused) PauseMenu(spriteBatch);
    }

    private Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int radius)
    {
        int diameter = radius * 2;
        Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
        Color[] colorData = new Color[diameter * diameter];

        float radiussq = radius * radius;

        for (int x = 0; x < diameter; x++)
        {
            for (int y = 0; y < diameter; y++)
            {
                int index = x * diameter + y;
                Vector2 pos = new Vector2(x - radius, y - radius);
                if (pos.LengthSquared() <= radiussq)
                {
                    colorData[index] = Color.White;
                }
                else
                {
                    colorData[index] = Color.Transparent;
                }
            }
        }

        texture.SetData(colorData);
        return texture;
    }

    // Verify position is valid
    private bool IsValidPosition(Point position)
    {
        position.X /= tileSize;
        position.Y /= tileSize;

        if (position.X < 0 || position.Y < 0 || position.X >= level.GetLength(0) || position.Y >= level.GetLength(1))
            return false;

        if (level[position.X, position.Y] != '#')
            return false;

        return true;
    }

    // Place Tower on map
    private void PlaceTowerOnMap(Point position)
    {
        if (IsValidPosition(position) && (Tower.Money >= 350))
        {
            int tileX = position.X / tileSize;
            int tileY = position.Y / tileSize;

            Tower newTower = new Tower(turret, new Vector2(tileX * tileSize, tileY * tileSize), projectileTexture);
            Tower.Money -= 350;
            towers.Add(newTower);
        }
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
                    playerPosition = new Vector2(x * tileSize, y * tileSize); // Adjusted position for player
                    player = new Player(player1, playerPosition);
                }
            }
        }
    }

    private void PauseMenu(SpriteBatch spriteBatch)
    {
        SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
        SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");

        // Pause title render
        spriteBatch.DrawString(largeFont, m_PauseText, Game1.CenterText(largeFont, m_PauseText) - new Vector2(0.0f, 50.0f), Color.Black);

        // To menu text render
        spriteBatch.DrawString(mediumFont, m_ToMenuText, Game1.CenterText(mediumFont, m_ToMenuText), Color.Black);
    }

    public Vector2 GetPlayerPosition()
    {
        return player.position;
    }
    #endregion
}
