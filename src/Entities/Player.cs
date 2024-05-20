using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2.src.Entities
{
    enum Direction
    {
        Up, Down, Left, Right // 0, 1, 2, 3
    }

    public class Player
    {
        // Player position
        private Vector2 position { get; set; }

        // Player texture
        private Texture2D texture;

        private float speed = 4f;

        public int CurrentRow { get; set; }

        // Animation fields
        float timer;
        int threshold;
        Rectangle[] sourceRectangles;
        byte previousAnimationIndex;
        byte currentAnimationIndex;
        private int frameWidth = 32;
        private int frameHeight = 48;
        private int framesPerRow = 6;

        // Flag to track if the player was moving in the previous frame
        private bool wasMoving = false;

        // Constructor
        public Player(Texture2D texture, Vector2 Position)
        {
            this.texture = texture;
            position = Position;
            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            timer = 0;
            threshold = 100;
            int currentRow = CurrentRow;
            sourceRectangles = new Rectangle[6];
            for (int i = 0; i < sourceRectangles.Length; i++)
            {
                int x = (i % framesPerRow) * frameWidth;
                int y = currentRow * frameHeight;
                sourceRectangles[i] = new Rectangle(x, y, frameWidth, frameHeight);
            }
            previousAnimationIndex = 2;
            currentAnimationIndex = 1;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 newPosition = position;
            bool moved = false;

            bool moveLeft = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
            bool moveRight = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
            bool moveUp = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
            bool moveDown = keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);

            if (moveLeft && moveUp)
            {
                newPosition.X -= speed * deltaTime;
                newPosition.Y -= speed * deltaTime;
                if (CurrentRow != 10)
                {
                    CurrentRow = 10;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveLeft && moveDown)
            {
                newPosition.X -= speed * deltaTime;
                newPosition.Y += speed * deltaTime;
                if (CurrentRow != 4)
                {
                    CurrentRow = 4;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveRight && moveDown)
            {
                newPosition.X += speed * deltaTime;
                newPosition.Y += speed * deltaTime;
                if (CurrentRow != 16)
                {
                    CurrentRow = 16;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveRight && moveUp)
            {
                newPosition.X += speed * deltaTime;
                newPosition.Y -= speed * deltaTime;
                if (CurrentRow != 22)
                {
                    CurrentRow = 22;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveLeft)
            {
                newPosition.X -= speed * deltaTime;
                if (CurrentRow != 7)
                {
                    CurrentRow = 7;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveRight)
            {
                newPosition.X += speed * deltaTime;
                if (CurrentRow != 19)
                {
                    CurrentRow = 19;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveUp)
            {
                newPosition.Y -= speed * deltaTime;
                if (CurrentRow != 13)
                {
                    CurrentRow = 13;
                    InitializeAnimation();
                }
                moved = true;
            }
            else if (moveDown)
            {
                newPosition.Y += speed * deltaTime;
                if (CurrentRow != 1)
                {
                    CurrentRow = 1;
                    InitializeAnimation();
                }
                moved = true;
            }

            if (moved)
            {
                position = newPosition;
                UpdateAnimation(gameTime);
                wasMoving = true; // Player is moving
            }
            else
            {
                // Check if the player was moving in the previous frame
                if (wasMoving)
                {
                    CurrentRow--; // Set to idle animation row
                    InitializeAnimation();
                    wasMoving = false; // Reset the flag
                }
                UpdateAnimation(gameTime);
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > threshold)
            {
                if (currentAnimationIndex == 1)
                {
                    currentAnimationIndex = (previousAnimationIndex == 0) ? (byte)2 : (byte)0;
                    previousAnimationIndex = currentAnimationIndex;
                }
                else
                {
                    currentAnimationIndex = 1;
                }
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = position * 64; // Assuming tile size is 64
            Rectangle rect = new Rectangle(pos.ToPoint(), new Point(64));
            spriteBatch.Draw(texture, rect, sourceRectangles[currentAnimationIndex], Color.White);
        }
    }
}
