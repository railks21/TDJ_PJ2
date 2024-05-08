using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;

namespace TDJ_PJ2;

public class SceneManager
{
    #region Delegate
    public delegate void SceneChange(SceneType sceneType);
    #endregion

    #region Fields
    //public ScoreManager Score;
    public SceneType Type {get; set;}
    public object Score { get; private set; }

    public IScene CurrentScene;
    private bool m_IsSceneChanged;
    private GraphicsDevice _graphicsDevice;
    #endregion

    #region Constructor
    public SceneManager(GraphicsDevice graphicsDevice)
    {
        //Score = new ScoreManager();

        Type = SceneType.Menu;
        CurrentScene = new MainMenuScene(graphicsDevice);

        m_IsSceneChanged = false;

        // Subscribing to events
        MainMenuScene.SceneChangeEvent += OnSceneChange;
        //GameScene.SceneChangeEvent += OnSceneChange;
        HelpScene.SceneChangeEvent += OnSceneChange;
        CreditsScene.SceneChangeEvent += OnSceneChange;
        //OverScene.SceneChangeEvent += OnSceneChange;
        //EntityManager.SceneChangeEvent += OnSceneChange;
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        // Only updating the current scene
        CurrentScene.Update(gameTime);

        // Only loading the scene when there is a change in the state
        if(!m_IsSceneChanged) return;

        switch(Type)
        {
            case SceneType.Menu:
                CurrentScene = new MainMenuScene(_graphicsDevice);
            break;
            //case SceneType.Game:
            //    CurrentScene = new GameScene(Score);
            //    break;
            case SceneType.Help:
                CurrentScene = new HelpScene();
            break;
            case SceneType.Credits:
                CurrentScene = new CreditsScene();
            break;
                //case SceneType.Over:
                //    CurrentScene = new OverScene(Score);
                //    break;
        }

        // Making sure that the scene loads once not every frame
        m_IsSceneChanged = false;
    }
    
    public void Render(SpriteBatch spriteBatch)
    {
        // Only rendering the current scene
        CurrentScene.Render(spriteBatch);
    }

    public void OnSceneChange(SceneType sceneType)
    {
        Type = sceneType;

        // Resetting the score when the game is replayed
        if (Type == SceneType.Game)
        {
            Score = 0;
        }

        m_IsSceneChanged = true;
    }
    #endregion
}