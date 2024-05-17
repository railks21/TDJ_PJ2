using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace TDJ_PJ2;

public class Zombie : DynamicEntity
{
    #region Consts
    private const int MAX_ATTACK_COOLDOWN = 200;
    #endregion

    #region Fields
    public readonly int MaxDamage;
    public int Damage {get; set;}
    public bool IsAbleToAttack {get; set;}
    public ZombieType Type {get; private set;}

    private int m_AttackCoolDown;
    private List<Vector2> m_BarricadePoints = new List<Vector2>();
    private List<Vector2> m_PathPoints;
    private int m_CurrentPathIndex = 0;
    private Vector2 m_CurrentTarget;
    private float m_Speed;
    private char[,] m_Map;
    #endregion

    #region Events
    // Collision events
    //public static event BarricadeCollision BarricadeCollisionEvent;

    // Audio events
    public static event BarricadeHitAudio BarricadeHitAudioEvent;
    public static event ZombieGrowlAudio ZombieGrowlAudioEvent;
    public static event ZombieDeathAudio ZombieDeathAudioEvent;

    // Score events
    public static event ScoreIncrease ScoreIncreaseEvent;
    #endregion

    #region Constructor
    public Zombie(Vector2 position, Texture2D texture, int health, int damage, float speed, List<Vector2> pathPoints, char[,] map)
            : base(position, texture, health)
        {
        Velocity = new Vector2(0.0f, speed);

        MaxDamage = damage;
        Damage = 0;
        IsAbleToAttack = true;

        // Determines which of the zombie types it is from the texture
        if(texture == AssetManager.Instance().GetSprite("BasicZombie"))
            Type = ZombieType.Basic;
        else if(texture == AssetManager.Instance().GetSprite("BruteZombie"))
            Type = ZombieType.Brute;
        else 
            Type = ZombieType.Denizen;

        m_AttackCoolDown = MAX_ATTACK_COOLDOWN;

        m_PathPoints = pathPoints;
        m_Map = map;
        m_CurrentTarget = m_PathPoints[0];
        m_Speed = speed;

        // Adding the barricade points for collision
        for (int i = 0; i < 26; i++)
        {
            m_BarricadePoints.Add(new Vector2(i * 32.0f, Game1.ScreenHeight - 160.0f));
        }
    }
    #endregion

    #region Methods
    public override void Update(GameTime gameTime)
    {
        // Decrease the attack cooldown gradually
        m_AttackCoolDown--;

        if(IsAbleToAttack)
            Attack();

        // Allowing the zombie to attack once the cooldown has reached 0
        if(m_AttackCoolDown == 0)
        {
            m_AttackCoolDown = MAX_ATTACK_COOLDOWN;
            IsAbleToAttack = true;
        }

        MoveAlongPath();

        base.Update(gameTime);
    
        // Plays the approriate sound when the zombie dies
        if(Health == 0) 
        {
            ZombieDeathAudioEvent?.Invoke();
            ScoreIncreaseEvent?.Invoke(Type);
        }
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
    }

    public override void Move(GameTime gameTime)
    {
        base.Move(gameTime);
    }

    public void Attack()
    {
        Damage = MaxDamage;
        IsAbleToAttack = false;
        
        // Plays the appropriate zombie sound depending on the type
        ZombieGrowlAudioEvent?.Invoke(Type);
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