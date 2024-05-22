using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TDJ_PJ2;

public class SceneManager
{
    #region Delegate
    public delegate void SceneChange(SceneType sceneType);
    #endregion

    #region Fields
    public SceneType Type {get; set;}
    public object Score { get; private set; }

    public IScene CurrentScene;
    private bool m_IsSceneChanged;
    private GraphicsDevice _graphicsDevice;
    private GraphicsDeviceManager _graphics;
    private ContentManager _contentManager;
    private GameScene _gameScene;
    #endregion

    #region Constructor
    public SceneManager(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, ContentManager contentManager)
    {
        _graphicsDevice = graphicsDevice;
        _graphics = graphics;
        _contentManager = contentManager;

        _gameScene = new GameScene(_graphicsDevice, _graphics, _contentManager);

        Type = SceneType.Menu;
        CurrentScene = new MainMenuScene(_graphicsDevice);

        m_IsSceneChanged = false;

        // Subscribing to events
        MainMenuScene.SceneChangeEvent += OnSceneChange;
        GameScene.SceneChangeEvent += OnSceneChange;
        HelpScene.SceneChangeEvent += OnSceneChange;
        CreditsScene.SceneChangeEvent += OnSceneChange;
        OverScene.SceneChangeEvent += OnSceneChange;
        WinScene.SceneChangeEvent += OnSceneChange;
        EntityManager.SceneChangeEvent += OnSceneChange;
        SpawnManager.SceneChangeEvent += OnSceneChange;
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
            case SceneType.Game:
                CurrentScene = new GameScene(_graphicsDevice, _graphics, _contentManager);
            break;
            case SceneType.Help:
                CurrentScene = new HelpScene(_graphicsDevice);
            break;
            case SceneType.Credits:
                CurrentScene = new CreditsScene(_graphicsDevice);
            break;
            case SceneType.Over:
                CurrentScene = new OverScene(_graphicsDevice);
            break;
            case SceneType.Win:
                CurrentScene = new WinScene(_graphicsDevice);
            break;
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

        m_IsSceneChanged = true;
    }
    #endregion
}