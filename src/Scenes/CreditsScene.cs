using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2;

public class CreditsScene : IScene
{
    #region Event related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion
    
    #region Fields
    private string m_Title, m_ToMenuText;
    private KeyboardState m_CurrentState, m_PreviousState;
    private Rectangle pauseHitbox;
    private Texture2D backgroundTexture;
    private GraphicsDevice _graphicsDevice;
    #endregion

    #region Constructor
    public CreditsScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        m_Title = "CREDITS";
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

        // From Credits to Menu
        if(m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
            SceneChangeEvent?.Invoke(SceneType.Menu);

        MouseState mouseState = Mouse.GetState();

        // Verifica se o bot�o esquerdo do mouse foi clicado
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            // Verifica se o clique do mouse ocorreu dentro da �rea do texto "Play"
            if (pauseHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Menu);
            }
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
        SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");
        SpriteFont smallFont = AssetManager.Instance().GetFont("Small");

        // Draw background menu
        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

        // Title render
        spriteBatch.DrawString(largeFont, m_Title, new Vector2(Game1.CenterText(largeFont, m_Title).X, 10.0f), Color.Black);

        // CREDITS: Sprites 
        spriteBatch.DrawString(smallFont, "Sprites: ", new Vector2(Game1.CenterText(smallFont, "Sprites: ").X, 100.0f), Color.Black);
        spriteBatch.DrawString(smallFont, "Cornerlord", new Vector2(Game1.CenterText(smallFont, "Cornerlord").X, 130.0f), Color.Black);
        spriteBatch.DrawString(smallFont, "Hypotis", new Vector2(Game1.CenterText(smallFont, "Hypotis").X, 160.0f), Color.Black);
        spriteBatch.DrawString(smallFont, "Ocal", new Vector2(Game1.CenterText(smallFont, "Ocal").X, 190.0f), Color.Black);

        // CREDITS: Sounds
        spriteBatch.DrawString(smallFont, "Audio: ", new Vector2(Game1.CenterText(smallFont, "Audio: ").X, 250.0f), Color.Blue);
        spriteBatch.DrawString(smallFont, "Michel Baradari", new Vector2(Game1.CenterText(smallFont, "Michel Baradari").X, 280.0f), Color.Black);
        spriteBatch.DrawString(smallFont, "artisticdude", new Vector2(Game1.CenterText(smallFont, "artisticdude").X, 310.0f), Color.Black);
        spriteBatch.DrawString(smallFont, "Independent.nu", new Vector2(Game1.CenterText(smallFont, "Independent.nu").X, 340.0f), Color.Black);

        Vector2 pauseTextPosition = Game1.CenterText(mediumFont, m_ToMenuText);
        pauseTextPosition.Y += 500.0f;

        // To menu text render
        spriteBatch.DrawString(mediumFont, m_ToMenuText, new Vector2(Game1.CenterText(mediumFont, m_ToMenuText).X, pauseTextPosition.Y), Color.Black);

        // Pause HitBox render
        pauseHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_ToMenuText).X,
            (int)pauseTextPosition.Y,
            (int)mediumFont.MeasureString(m_ToMenuText).X,
            (int)mediumFont.MeasureString(m_ToMenuText).Y);
    }
    #endregion
}