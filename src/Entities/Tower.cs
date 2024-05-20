using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TDJ_PJ2;

public class Tower
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public float Range { get; set; }
    public float FireRate { get; set; }
    private float fireCooldown;
    private float readyShoot;
    public List<Projectile> Projectiles { get; private set; }
    private Texture2D projectileTexture;

    public Tower(Texture2D texture, Vector2 position, Texture2D projectileTexture)
    {
        Texture = texture;
        Position = position;
        Range = 200f; // Example range
        FireRate = 1f; // Example fire rate (1 shot per second)
        fireCooldown = 50f; // Cooldown Shoot
        readyShoot = 0f; // Ready to Shoot = 0
        Projectiles = new List<Projectile>();
        this.projectileTexture = projectileTexture;
    }

    public void Update(GameTime gameTime, EntityManager entityManager)
    {
        readyShoot -= 1;

        var enemies = entityManager.Entities;
        var target = GetClosestEnemy(enemies);
        if (target != null && readyShoot <= 0)
        {
            Shoot(target);
            readyShoot = fireCooldown;
        }

        // Update projectiles
        for (int i = Projectiles.Count - 1; i >= 0; i--)
        {
            Projectiles[i].Update(gameTime);
            if (!Projectiles[i].IsActive)
            {
                Projectiles.RemoveAt(i);
            }
        }

        CheckProjectileCollisions(enemies);

        Projectiles.RemoveAll(p => !p.IsActive);
    }

    private IEntity GetClosestEnemy(List<IEntity> enemies)
    {
        IEntity closestEnemy = null;
        float closestDistance = Range;

        foreach (var enemy in enemies)
        {
            if (enemy is Zombie zombie)
            {
                float distance = Vector2.Distance(Position, zombie.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = zombie;
                }
            }
        }

        return closestEnemy;
    }

    private void Shoot(IEntity target)
    {
        Vector2 direction = target.Position - Position;
        direction.Normalize();

        Projectile newProjectile = new Projectile(projectileTexture, Position, direction, 300f);
        Projectiles.Add(newProjectile);
    }

    private void CheckProjectileCollisions(List<IEntity> enemies)
    {
        foreach (var projectile in Projectiles)
        {
            foreach (var enemy in enemies)
            {
                if (projectile.Collider.Intersects(enemy.Collider))
                {
                    enemy.TakeDamage(100); // example damage, adjust as needed
                    projectile.IsActive = false;
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
        foreach (var projectile in Projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}