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
    private string m_Title, m_PlayText, m_HelpText, m_CreditsText, m_ExitText;
    private KeyboardState m_CurrentState, m_PreviousState;
    #endregion

    #region Constructor
    public MainMenuScene()
    {
        m_Title = "The Horde";
        m_PlayText = "[ENTER] PLAY";
        m_HelpText = "[H] Help";
        m_CreditsText = "[C] CREDITS";
        m_ExitText = "[ESC] EXIT";

        m_CurrentState = Keyboard.GetState();
        m_PreviousState = m_CurrentState;
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = Keyboard.GetState();

        /* Transitioning from the menu to other scenes */
        // From Menu to Game
        if(m_CurrentState.IsKeyDown(Keys.Enter) && m_PreviousState.IsKeyUp(Keys.Enter))
            SceneChangeEvent?.Invoke(SceneType.Game);
        // From Menu to Help
        else if(m_CurrentState.IsKeyDown(Keys.H) && m_PreviousState.IsKeyUp(Keys.H))
            SceneChangeEvent?.Invoke(SceneType.Help);
        // From Menu to Credits
        else if(m_CurrentState.IsKeyDown(Keys.C) && m_PreviousState.IsKeyUp(Keys.C))
            SceneChangeEvent?.Invoke(SceneType.Credits);
    }
    
    public void Render(SpriteBatch spriteBatch)
    {
        SpriteFont largeFont = AssetManager.Instance().GetFont("Large");
        SpriteFont mediumFont = AssetManager.Instance().GetFont("Medium");

        // Title render
        spriteBatch.DrawString(largeFont, m_Title, Game1.CenterText(largeFont, m_Title) + new Vector2(0, 10.0f), Color.CadetBlue);

        // Play text render
        spriteBatch.DrawString(mediumFont, m_PlayText, Game1.CenterText(mediumFont, m_PlayText) + new Vector2(0, 180.0f), Color.CadetBlue);

        // Help text render
        spriteBatch.DrawString(mediumFont, m_HelpText, Game1.CenterText(mediumFont, m_HelpText) + new Vector2(0, 230.0f), Color.CadetBlue);

        // Credits text render
        spriteBatch.DrawString(mediumFont, m_CreditsText, Game1.CenterText(mediumFont, m_CreditsText) + new Vector2(0, 280.0f), Color.CadetBlue);

        // Exit text render
        spriteBatch.DrawString(mediumFont, m_ExitText, Game1.CenterText(mediumFont, m_ExitText) + new Vector2(0, 330.0f), Color.CadetBlue);
    }
    #endregion
}