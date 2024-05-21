using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2;

public class HelpScene : IScene
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
    public HelpScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        m_Title = "HELP";
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

        // From Help to Menu
        if(m_CurrentState.IsKeyDown(Keys.M) && m_PreviousState.IsKeyUp(Keys.M))
            SceneChangeEvent?.Invoke(SceneType.Menu);

        MouseState mouseState = Mouse.GetState();

        // Use Mouse click
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
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

        // Title
        spriteBatch.DrawString(largeFont, m_Title, new Vector2(Game1.CenterText(largeFont, m_Title).X, 10.0f), Color.Black);

        // Controls
        spriteBatch.DrawString(mediumFont, "CONTROLS:", new Vector2(20.0f, 150.0f), Color.Blue);

        // CONTROLS: Movement
        spriteBatch.DrawString(smallFont, "Use WASD or ARROWS to move the player.", new Vector2(20.0f, 200.0f), Color.Black);
        
        // CONTROLS: Mouse DragAndDrop Turret
        spriteBatch.DrawString(smallFont, "Use MOUSE to move turrets on map.", new Vector2(20.0f, 230.0f), Color.Black);

        // How to play
        spriteBatch.DrawString(mediumFont, "How to play:", new Vector2(20.0f, 290.0f), Color.Blue);

        // Kill enemies
        spriteBatch.DrawString(smallFont, "Kill Zombies to defend yourself.", new Vector2(20.0f, 350.0f), Color.Black);

        // Use Player or Turret
        spriteBatch.DrawString(smallFont, "Use Player or Turrets to kill them", new Vector2(20.0f, 380.0f), Color.Black);

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