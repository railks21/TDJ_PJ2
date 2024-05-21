using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TDJ_PJ2
{
    public class Zombie : DynamicEntity
    {
        #region Consts
        private float m_Speed;
        private char[,] m_Map;
        #endregion

        #region Fields
        public ZombieType Type { get; private set; }
        public int CurrentRow { get; set; }

        private List<Vector2> m_FinalPoint = new List<Vector2>();
        private List<Vector2> m_PathPoints;
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
        #endregion

        #region Events
        private SpawnManager Spawner;
        private EntityManager Entities;

        // Collision events
        public static event TilesCollision FinalDestionationEvent;

        // Score events
        public static event ScoreIncrease ScoreIncreaseEvent;
        #endregion

        #region Constructor
        public Zombie(Vector2 position, Texture2D texture, int health, int damage, float speed, List<Vector2> pathPoints, char[,] map, int currentRow)
            : base(position, texture, health)
        {
            Spawner = new SpawnManager(map, Entities);

            Velocity = new Vector2(0.0f, speed);
            m_PathPoints = pathPoints;
            m_Map = map;
            m_CurrentTarget = m_PathPoints[0];
            m_Speed = Spawner.Speed;
            CurrentRow = currentRow;

            // Determine which of the zombie types it is from the texture
            if (texture == AssetManager.Instance().GetSprite("BasicZombie"))
                Type = ZombieType.Basic;
            else if (texture == AssetManager.Instance().GetSprite("BruteZombie"))
                Type = ZombieType.Brute;
            else
                Type = ZombieType.Denizen;

            // Adding the final point for collision
            for (int x = 0; x < m_Map.GetLength(0); x++)
            {
                for (int y = 0; y < m_Map.GetLength(1); y++)
                {
                    if (map[x, y] == 'F')
                    {
                        m_FinalPoint.Add(new Vector2(x * 64, y * 64));
                    }
                }
            }

            // Initialize animation
            InitializeAnimation();
        }
        #endregion

        #region Methods
        private void InitializeAnimation()
        {
            timer = 0;

            // Set an initial threshold of 250ms, you can change this to alter the speed of the animation (lower number = faster animation).
            threshold = 250;

            // Define the dimensions of each frame in the spritesheet.
            int frameWidth = 48;
            int frameHeight = 64;
            int framesPerRow = 3;

            // Choose the desired row (0 for the first row, 1 for the second row, etc.).
            int currentRow = CurrentRow; // For example, choosing the third row.

            // Initialize the sourceRectangles array based on the number of frames.
            sourceRectangles = new Rectangle[4];
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

        public override void Update(GameTime gameTime)
        {
            MoveAlongPath();
            UpdateAnimation(gameTime);
            base.Update(gameTime);
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

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.Instance().GetSprite("ZombieG"), Position, sourceRectangles[currentAnimationIndex], Color.White);
        }

        public override void CollisionUpdate(List<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is DynamicEntity dynamicEntity)
                {
                    // Ajuste a lógica de colisão para levar em conta o novo tamanho da hitbox
                    Rectangle expandedHitbox = new Rectangle(Hitbox.X - 32, Hitbox.Y - 32, Hitbox.Width + 64, Hitbox.Height + 64);

                    if (expandedHitbox.Intersects(dynamicEntity.Hitbox))
                    {
                        // Lógica para lidar com a colisão entre o zombie e a outra entidade
                    }
                }
            }

            foreach (var point in m_FinalPoint)
            {
                if (Vector2.Distance(Position, point) <= 64)
                {
                    FinalDestionationEvent?.Invoke(this);
                }
            }
        }

        public override void Move(GameTime gameTime)
        {
            base.Move(gameTime);
        }

        public void MoveAlongPath()
        {
            if (m_CurrentPathIndex < m_PathPoints.Count)
            {
                m_CurrentTarget = m_PathPoints[m_CurrentPathIndex];

                // Check if the next path point is within the boundary of 'C'
                if (m_Map[(int)m_CurrentTarget.X / 64, (int)m_CurrentTarget.Y / 64] != 'C')
                {
                    // If the next point is not 'C', find the next 'C' point
                    FindNextPathPoint();
                }
                else
                {
                    // Calculate direction
                    Vector2 direction = Vector2.Normalize(m_CurrentTarget - Position);

                    // Update velocity
                    Velocity = direction * m_Speed;

                    // Move towards the target
                    Position += Velocity;

                    // Check if we reached the target
                    if (Vector2.Distance(Position, m_CurrentTarget) < 1f)
                    {
                        NextPathPoint();
                    }
                }
            }
        }

        private void NextPathPoint()
        {
            m_CurrentPathIndex++;

            if (m_CurrentPathIndex < m_PathPoints.Count)
            {
                m_CurrentTarget = m_PathPoints[m_CurrentPathIndex];
            }
        }

        private void FindNextPathPoint()
        {
            // Find the next 'C' point
            while (m_CurrentPathIndex < m_PathPoints.Count &&
                   m_Map[(int)m_PathPoints[m_CurrentPathIndex].X / 64, (int)m_PathPoints[m_CurrentPathIndex].Y / 64] != 'C')
            {
                m_CurrentPathIndex++;
            }

            if (m_CurrentPathIndex < m_PathPoints.Count)
            {
                m_CurrentTarget = m_PathPoints[m_CurrentPathIndex];
            }
        }
        #endregion
    }
}