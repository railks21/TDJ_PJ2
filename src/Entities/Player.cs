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
        private Vector2 position {  get; set; }

        // Player texture
        private Texture2D texture;
        private GameScene gameScene; // apagar

        private int delta = 0;
        private Texture2D[][] sprites;
        private Direction direction = Direction.Down;
        private Vector2 directionVector;
        //private int speed = 2; // NOTA: tem de ser divisor de tileSize
        private float speed = 10f;

        public int CurrentRow { get; set; }
        private int m_CurrentPathIndex = 0;
        private Vector2 m_CurrentTarget;
        // Animation fields
        // A timer that stores milliseconds.
        float timer;
        // An int that is the threshold for the timer.
        int threshold;
        // A Rectangle array that stores sourceRectangles for animations.
        Rectangle[] sourceRectangles;
        // These bytes tell the spriteBatch.Draw() what sourceRectangle to display.
        byte previousAnimationIndex;
        byte currentAnimationIndex;



        int test = 0;

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

            // Set an initial threshold of 250ms, you can change this to alter the speed of the animation (lower number = faster animation).
            threshold = 250;

            // Define the dimensions of each frame in the spritesheet.
            int frameWidth = 32;
            int frameHeight = 48;
            int framesPerRow = 6;

            // Choose the desired row (0 for the first row, 1 for the second row, etc.).
            int currentRow = CurrentRow; // For example, choosing the third row.

            // Initialize the sourceRectangles array based on the number of frames.
            sourceRectangles = new Rectangle[6];
            for (int i = 0; i < sourceRectangles.Length; i++)
            {
                int x = (i % framesPerRow) * frameWidth;
                int y = currentRow * frameHeight; // Adjust Y to the current row
                sourceRectangles[i] = new Rectangle(x, y, frameWidth, frameHeight);
            }

            // This tells the animation to start on the left-side sprite.
            previousAnimationIndex = 2;
            currentAnimationIndex = 1;
        }



        public void Update(GameTime gameTime)
        {
            // Get the elapsed time since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Get the keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // Create a copy of the current position
            Vector2 newPosition = position;

            // Move the player based on keyboard input
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                newPosition.X -= speed * deltaTime;
                CurrentRow = 7;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                newPosition.X += speed * deltaTime;
                CurrentRow = 19;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                newPosition.Y -= speed * deltaTime;
                CurrentRow = 13;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                newPosition.Y += speed * deltaTime;
                CurrentRow = 1;
            }

            // Update the position
            position = newPosition;

            // update animation 
            UpdateAnimation(gameTime);

        }



        private void UpdateAnimation(GameTime gameTime)
        {
            // Check if the timer has exceeded the threshold.
            if (timer > threshold)
            {
                // If Alex is in the middle sprite of the animation.
                if (currentAnimationIndex == 1)
                {
                    // If the previous animation was the left-side sprite, then the next animation should be the right-side sprite.
                    if (previousAnimationIndex == 0)
                    {
                        currentAnimationIndex = 2;
                    }
                    else
                    {
                        // If not, then the next animation should be the left-side sprite.
                        currentAnimationIndex = 0;
                    }

                    // Track the animation.
                    previousAnimationIndex = currentAnimationIndex;
                }
                // If Alex was not in the middle sprite of the animation, he should return to the middle sprite.
                else
                {
                    currentAnimationIndex = 1;
                }

                // Reset the timer.
                timer = 0;
            }
            // If the timer has not reached the threshold, then add the milliseconds that have past since the last Update() to the timer.
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }



        public void Draw(SpriteBatch sb) 
        {
            Vector2 pos = position * 64;

            //int frame = 0;
            //if (delta > 0)
            //{
            //    pos -= (64 - delta) * directionVector;
            //    float animSpeed = 8f;
            //    frame = (int)((delta / speed) % ((int)animSpeed * 6) / animSpeed);
            //}

            Rectangle rect = new Rectangle(pos.ToPoint(), new Point(64));
            //// sb.Draw(sprites[(int)direction][frame], rect, Color.White);
            //sb.Draw(texture, rect, Color.White);

            
            sb.Draw(AssetManager.Instance().GetSprite("Player1"), rect, sourceRectangles[currentAnimationIndex], Color.White);
        }

    }
}
