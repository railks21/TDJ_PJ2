using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace TDJ_PJ2;

public abstract class IEntity
{
    #region Fields
    public abstract Vector2 Position {get; set;}
    public abstract Texture2D Texture {get; set;}
    public abstract Rectangle Collider {get; set;}
    public abstract int MaxHealth {get; set;}
    public abstract int Health {get; set;}
    public abstract bool IsActive {get; set;}
    #endregion

    #region Methods
    public abstract void Update(GameTime gameTime);
    public abstract void CollisionUpdate(List<IEntity> entities);
    public abstract void Render(SpriteBatch spriteBatch);
    public abstract void TakeDamage(int damage);
    #endregion
}

public class StaticEntity : IEntity
{
    #region Fields
    public override Vector2 Position { get; set; }
    public override Texture2D Texture { get; set; }
    public override Rectangle Collider
    {
        get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        set { Collider = value; }
    }
    public override int MaxHealth {get; set;}
    public override int Health {get; set;}
    public override bool IsActive { get; set; }
    #endregion

    #region Delegates
    public virtual Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

    // Collision delegates
    //public delegate void BulletCollision(Bullet bullet, Zombie zombie);
    public delegate void TilesCollision(Zombie zombie);
    
    // Audio delegates 
    //public delegate void BulletShotAudio(BulletType bulletType);
    public delegate void BarricadeHitAudio();
    public delegate void ZombieGrowlAudio(ZombieType zombieType);
    public delegate void ZombieDeathAudio();

    // Score delegates
    public delegate void ScoreIncrease(ZombieType zombieType);
    #endregion

    #region Constructor
    // Default constructor
    public StaticEntity()
    {
        Position = new Vector2(0.0f, 0.0f);
        
        Texture = null;
        
        MaxHealth = 0;
        Health = MaxHealth;
        
        IsActive = false;
    }

    public StaticEntity(Vector2 position, Texture2D texture, int maxHealth)
    {
        Position = position;
        
        Texture = texture;
        
        MaxHealth = maxHealth;
        Health = MaxHealth;
        
        IsActive = true;
    }
    #endregion

    #region Methods
    public override void Update(GameTime gameTime)
    {
        // Clamping the health to not go over the max or under 0
        if(Health > MaxHealth) Health = MaxHealth;
        else if(Health < 0) Health = 0;

        // Killing the entity once it is out of health
        if(Health == 0) IsActive = false;
    }

    public override void CollisionUpdate(List<IEntity> entities)
    {
        // Does nothing here   
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        if(IsActive)
            spriteBatch.Draw(Texture, Position, Color.White);
    }

    public override void TakeDamage(int damage)
    {
        // Taking damage only when there is health
        if(Health != 0) Health -= damage;
    }
    #endregion
}

public class DynamicEntity : StaticEntity
{
    #region Variables
    public Vector2 Velocity {get; set;}
    public bool IsMoving {get; set;}
    #endregion

    #region Constructor
    // HitBox Zombie
    //public override Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, 64, 64);

    public override Rectangle Hitbox => new Rectangle((int)Position.X - 32, (int)Position.Y - 32, 64, 64);

    public DynamicEntity(Vector2 position, Texture2D texture, int maxHealth)
        :base(position, texture, maxHealth)
    {
        Velocity = new Vector2(0.0f, 200.0f);
        IsMoving = true;
    }
    #endregion

    #region Methods
    public override void Update(GameTime gameTime)
    {
        if(IsMoving) Move(gameTime);

        // Clamping the position to the window's borders
        Position = new Vector2(MathHelper.Clamp(Position.X, -20.0f, Game1.ScreenWidth - Texture.Width + 15.0f), Position.Y);

        base.Update(gameTime);
    }

    public virtual void Move(GameTime gameTime)
    {
        // Basic movements
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    #endregion
}