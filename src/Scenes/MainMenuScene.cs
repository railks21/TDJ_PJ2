using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2;

public class MainMenuScene : IScene
{
    #region Event related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion

    #region Fields
    private string m_Title, m_PlayText, m_SettingsText, m_ScoresText, m_HelpText, m_CreditsText, m_ExitText;
    private KeyboardState m_CurrentState, m_PreviousState;
    private Rectangle playHitbox, settingsHitbox, scoreHitbox, helpHitbox, creditsHitbox, exitHitbox;
    private Texture2D backgroundTexture;
    private GraphicsDevice _graphicsDevice;
    #endregion

    #region Constructor
    public MainMenuScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        m_Title = "TD Mage Empire";
        m_PlayText = "[ENTER] PLAY";
        m_SettingsText = "[S] SETTINGS";
        m_ScoresText = "[L] SCORE";
        m_HelpText = "[H] Help";
        m_CreditsText = "[C] CREDITS";
        m_ExitText = "[ESC] EXIT";

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

        /* Transitioning from the menu to other scenes */
        // From Menu to Game
        if (m_CurrentState.IsKeyDown(Keys.Enter) && m_PreviousState.IsKeyUp(Keys.Enter))
            SceneChangeEvent?.Invoke(SceneType.Game);
        // From Menu to Help
        else if (m_CurrentState.IsKeyDown(Keys.H) && m_PreviousState.IsKeyUp(Keys.H))
            SceneChangeEvent?.Invoke(SceneType.Help);
        // From Menu to Credits
        else if (m_CurrentState.IsKeyDown(Keys.C) && m_PreviousState.IsKeyUp(Keys.C))
            SceneChangeEvent?.Invoke(SceneType.Credits);


        MouseState mouseState = Mouse.GetState();

        // Use Mouse click
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (playHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Game);
            }

            if (helpHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Help);
            }

            if (creditsHitbox.Contains(mouseState.Position))
            {
                SceneChangeEvent?.Invoke(SceneType.Credits);
            }

            if (exitHitbox.Contains(mouseState.Position))
            {
                // Action here
            }
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
        SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");

        Vector2 playTextPosition = Game1.CenterText(mediumFont, m_PlayText);
        Vector2 settingsTextPosition = Game1.CenterText(mediumFont, m_SettingsText);
        Vector2 scoresTextPosition = Game1.CenterText(mediumFont, m_ScoresText);
        Vector2 helpTextPosition = Game1.CenterText(mediumFont, m_HelpText);
        Vector2 creditTextPosition = Game1.CenterText(mediumFont, m_CreditsText);
        Vector2 exitTextPosition = Game1.CenterText(mediumFont, m_ExitText);

        playTextPosition.Y += 180.0f;
        settingsTextPosition.Y += 230.0f;
        scoresTextPosition.Y += 280.0f;
        helpTextPosition.Y += 330.0f;
        creditTextPosition.Y += 380.0f;
        exitTextPosition.Y += 430.0f;

        // Draw background menu
        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

        // Title render
        spriteBatch.DrawString(largeFont, m_Title, Game1.CenterText(largeFont, m_Title) + new Vector2(0, 10.0f), Color.Black);

        // Play text render
        spriteBatch.DrawString(mediumFont, m_PlayText, playTextPosition, Color.Black);

        // Settings text render
        spriteBatch.DrawString(mediumFont, m_SettingsText, settingsTextPosition, Color.Black);

        // Score text render
        spriteBatch.DrawString(mediumFont, m_ScoresText, scoresTextPosition, Color.Black);

        // Help text render
        spriteBatch.DrawString(mediumFont, m_HelpText, helpTextPosition, Color.Black);

        // Credits text render
        spriteBatch.DrawString(mediumFont, m_CreditsText, creditTextPosition, Color.Black);

        // Exit text render
        spriteBatch.DrawString(mediumFont, m_ExitText, exitTextPosition, Color.Black);

        // Play HitBox render
        playHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_PlayText).X,
            (int)playTextPosition.Y,
            (int)mediumFont.MeasureString(m_PlayText).X,
            (int)mediumFont.MeasureString(m_PlayText).Y);

        // Settings Hitbox render
        settingsHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_SettingsText).X,
            (int)settingsTextPosition.Y,
            (int)mediumFont.MeasureString(m_SettingsText).X,
            (int)mediumFont.MeasureString(m_SettingsText).Y);

        // Score Hitbox render
        scoreHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_ScoresText).X,
            (int)scoresTextPosition.Y,
            (int)mediumFont.MeasureString(m_ScoresText).X,
            (int)mediumFont.MeasureString(m_ScoresText).Y);

        // Help HitBox render
        helpHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_HelpText).X,
            (int)helpTextPosition.Y,
            (int)mediumFont.MeasureString(m_HelpText).X,
            (int)mediumFont.MeasureString(m_HelpText).Y);

        // Credit HitBox render
        creditsHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_CreditsText).X,
            (int)creditTextPosition.Y,
            (int)mediumFont.MeasureString(m_CreditsText).X,
            (int)mediumFont.MeasureString(m_CreditsText).Y);

        // Exit HitBox render
        exitHitbox = new Rectangle(
            (int)Game1.CenterText(mediumFont, m_ExitText).X,
            (int)exitTextPosition.Y,
            (int)mediumFont.MeasureString(m_ExitText).X,
            (int)mediumFont.MeasureString(m_ExitText).Y);

        // Draw HitBoxes
        //DrawHitbox(spriteBatch, playHitbox, Color.Red);
        //DrawHitbox(spriteBatch, settingsHitbox, Color.Orange);
        //DrawHitbox(spriteBatch, scoreHitbox, Color.Pink);
        //DrawHitbox(spriteBatch, helpHitbox, Color.Green);
        //DrawHitbox(spriteBatch, creditsHitbox, Color.Blue);
        //DrawHitbox(spriteBatch, exitHitbox, Color.Yellow);
    }

    //private void DrawHitbox(SpriteBatch spriteBatch, Rectangle hitbox, Color color)
    //{
    //    Texture2D pixel = new Texture2D(_graphicsDevice, 1, 1);
    //    pixel.SetData(new[] { color });

    //    spriteBatch.Draw(pixel, hitbox, color);
    //}
    #endregion
}