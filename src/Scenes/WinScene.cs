using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2;

public class WinScene : IScene
{
    #region Event related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion

    #region Fields
    private string m_Title, m_RetryText, m_ToMenuText;
    private KeyboardState m_CurrentState, m_PreviousState;
    private Rectangle pauseHitbox, retryHitbox;
    private Texture2D backgroundTexture;
    private GraphicsDevice _graphicsDevice;
    #endregion

    #region Constructor
    public WinScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        m_Title = "CONGRATULATIONS";
        m_RetryText = "[R] RETRY";
        m_ToMenuText = "[M] MENU";

        m_CurrentState = Keyboard.GetState();
        m_PreviousState = m_CurrentState;

        // Render texture background
        backgroundTexture = AssetManager.Instance().GetBackground("Background");
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = Keyboard.GetState();

        // Over to Game
        if (m_CurrentState.IsKeyDown(Keys.R) && m_PreviousState.IsKeyUp(Keys.R))
            SceneChangeEvent?.Invoke(SceneType.Game);
        // Over to Menu
        else if (m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
            SceneChangeEvent?.Invoke(SceneType.Menu);

        MouseState mouseState = Mouse.GetState();

        // Use Mouse click
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (pauseHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Menu);
            }

            if (retryHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Game);
            }
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
        SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");

        // Draw background menu
        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

        // Title render
        spriteBatch.DrawString(largeFont, m_Title, new Vector2(Game1.CenterText(largeFont, m_Title).X, 10.0f), Color.Black);

        Vector2 pauseTextPosition = Game1.CenterText(mediumFont, m_ToMenuText);
        Vector2 retryTextPosition = Game1.CenterText(mediumFont, m_RetryText);
        pauseTextPosition.Y += 500.0f;
        retryTextPosition.Y += 400.0f;

        // To menu text render
        spriteBatch.DrawString(mediumFont, m_ToMenuText, new Vector2(Game1.CenterText(mediumFont, m_ToMenuText).X, pauseTextPosition.Y), Color.Black);

        // To game text retry
        spriteBatch.DrawString(mediumFont, m_RetryText, new Vector2(Game1.CenterText(mediumFont, m_RetryText).X, retryTextPosition.Y), Color.Black);

        // Pause HitBox render
        pauseHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_ToMenuText).X,
            (int)pauseTextPosition.Y,
            (int)mediumFont.MeasureString(m_ToMenuText).X,
            (int)mediumFont.MeasureString(m_ToMenuText).Y);

        // Retry Hitbox render
        retryHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_RetryText).X,
            (int)retryTextPosition.Y,
            (int)mediumFont.MeasureString(m_RetryText).X,
            (int)mediumFont.MeasureString(m_RetryText).Y);
    }
    #endregion
}