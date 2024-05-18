using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace TDJ_PJ2;

public class Zombie : DynamicEntity
{
    #region Consts
    private float m_Speed;
    private char[,] m_Map;
    #endregion

    #region Fields
    public ZombieType Type {get; private set;}

    private List<Vector2> m_FinalPoint = new List<Vector2>();
    private List<Vector2> m_PathPoints;
    private int m_CurrentPathIndex = 0;
    private Vector2 m_CurrentTarget;
    #endregion

    #region Events
    private SpawnManager Spawner;
    private EntityManager Entities;

    // Collision events
    public static event BarricadeCollision BarricadeCollisionEvent;

    // Score events
    public static event ScoreIncrease ScoreIncreaseEvent;
    #endregion

    #region Constructor
    public Zombie(Vector2 position, Texture2D texture, int health, int damage, float speed, List<Vector2> pathPoints, char[,] map)
            : base(position, texture, health)
        {

        Spawner = new SpawnManager(map, Entities);

        Velocity = new Vector2(0.0f, speed);
        m_PathPoints = pathPoints;
        m_Map = map;
        m_CurrentTarget = m_PathPoints[0];
        m_Speed = Spawner.Speed;

        // Determines which of the zombie types it is from the texture
        if (texture == AssetManager.Instance().GetSprite("BasicZombie"))
            Type = ZombieType.Basic;
        else if(texture == AssetManager.Instance().GetSprite("BruteZombie"))
            Type = ZombieType.Brute;
        else 
            Type = ZombieType.Denizen;

        // Adding the barricade points for collision
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
    }
    #endregion

    #region Methods
    public override void Update(GameTime gameTime)
    {
        MoveAlongPath();

        base.Update(gameTime);
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
                BarricadeCollisionEvent?.Invoke(this);
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