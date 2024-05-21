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
    private string m_Title;
    private KeyboardState m_CurrentState, m_PreviousState;
    private Texture2D backgroundTexture;
    private GraphicsDevice _graphicsDevice;
    #endregion

    #region Constructor
    public CreditsScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        m_Title = "CREDITS";

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

        // To menu text render
        spriteBatch.DrawString(mediumFont, "[M] MENU", new Vector2(Game1.CenterText(mediumFont, "[M] MENU").X, 420.0f), Color.Black);
    }
    #endregion
}