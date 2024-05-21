using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

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
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Utility variables init
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Changing the game window size
            Window.Position = Point.Zero;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Assets init
            AssetManager.Instance().LoadAssets(Content);

            // Tiles init
            Tiles = new TileManager();

            // Scenes init
            Scenes = new SceneManager(GraphicsDevice, _graphics, _contentManager);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Scenes.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Rendering stuff here
            _spriteBatch.Begin();

            #region Managers render
            // Tiles render
            Tiles.Render(_spriteBatch);

            // Scenes render
            Scenes.Render(_spriteBatch);
            #endregion

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        // A util function that will center a text based on its font and contents
        public static Vector2 CenterText(SpriteFont font, string text)
        {
            return new Vector2(ScreenWidth / 2 - font.MeasureString(text).X / 2,
                               ScreenHeight / 4 - font.MeasureString(text).Y / 4);
        }
    }
}
