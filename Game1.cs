using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;

        #region Utility variables
        public static int ScreenWidth;
        public static int ScreenHeight;
        #endregion

        #region Managers
        public TileManager Tiles;
        public SceneManager Scenes;

        private Camera _camera;
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            Window.Position = Point.Zero;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.Instance().LoadAssets(Content);

            Tiles = new TileManager();
            Scenes = new SceneManager(GraphicsDevice, _graphics, _contentManager);

            // Inicialize a câmera com um zoom inicial
            float initialZoom = 1.0f; // Defina o valor de zoom desejado aqui
            _camera = new Camera(GraphicsDevice, Vector2.Zero, initialZoom);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update camera to follow player
            if (Scenes.CurrentScene is GameScene gameScene)
            {
                _camera.Follow(gameScene.GetPlayerPosition(), gameScene.LevelWidth, gameScene.LevelHeight, gameScene.TileSize);
            }

            HandleZoom();

            Scenes.Update(gameTime);

            base.Update(gameTime);
        }

        private void HandleZoom()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus) || Keyboard.GetState().IsKeyDown(Keys.Add))
            {
                _camera.Zoom = 1.9f; // Zoom in
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus) || Keyboard.GetState().IsKeyDown(Keys.Subtract))
            {
                _camera.Zoom = 1.0f; // Zoom out
            }

            _camera.Zoom = MathHelper.Clamp(_camera.Zoom, 0.5f, 3f); // Clamp zoom between 0.5 and 3
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _camera.TransformMatrix);

            Tiles.Render(_spriteBatch);
            Scenes.Render(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 CenterText(SpriteFont font, string text)
        {
            return new Vector2(ScreenWidth / 2 - font.MeasureString(text).X / 2,
                               ScreenHeight / 4 - font.MeasureString(text).Y / 4);
        }
    }
}
