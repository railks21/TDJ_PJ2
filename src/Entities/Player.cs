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
        private int readyShoot = 0; // ready shoot at 0, if higher still in cooldown
        private int fireCooldown = 50; // cooldown to shoot

        // Flag to track if the player was moving in the previous frame
        private bool wasMoving = false;

        // projectile do player
        private Texture2D projectileTexture;
        public List<Projectile> ProjectilesP { get; private set; }

        //tower
        private Tower tower;


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

        public void Update(GameTime gameTime, EntityManager entityManager)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 newPosition = position;
            bool moved = false;

            bool moveLeft = keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A);
            bool moveRight = keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D);
            bool moveUp = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
            bool moveDown = keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S);

            // moviment of player
            #region movPlayer
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
            #endregion

            // player shoot
            var enemies = entityManager.Entities;
            readyShoot -= 1;

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                if (readyShoot <= 0)
                {
                    Shoot();
                    readyShoot = fireCooldown;
                }
            }

            // Update projectiles
            for (int i = ProjectilesP.Count - 1; i >= 0; i--)
            {
                ProjectilesP[i].Update(gameTime);
                if (!ProjectilesP[i].IsActive)
                {
                    ProjectilesP.RemoveAt(i);
                }
            }

            CheckProjectileCollisions(enemies);

            ProjectilesP.RemoveAll(p => !p.IsActive);
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

        private void Shoot()
        {
            Vector2 direction = position;
            direction.Normalize();
            // codigo meu leonel
            //Vector2 direction = new Vector2(1, 1);

            Projectile newProjectile = new Projectile(projectileTexture, position, direction, 300f);
            ProjectilesP.Add(newProjectile);
        }

        private void CheckProjectileCollisions(List<IEntity> enemies)
        {
            foreach (var projectile in ProjectilesP)
            {
                foreach (var enemy in enemies)
                {
                    if (projectile.Collider.Intersects(enemy.Collider))
                    {
                        enemy.TakeDamage(100); // example damage, adjust as needed
                        projectile.IsActive = false;

                        if (enemy.Health <= 0)
                        {
                            tower.Money += tower.moneyPerEnemy;
                        }

                    }
                }
            }
        }
    }
}
