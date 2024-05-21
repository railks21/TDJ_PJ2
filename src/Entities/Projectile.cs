using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJ_PJ2;

public class Projectile
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Texture2D Texture { get; set; }
    public bool IsActive { get; set; }
    public float Speed { get; set; }

    public Projectile(Texture2D texture, Vector2 position, Vector2 direction, float speed)
    {
        Texture = texture;
        Position = position;
        Speed = speed;
        Velocity = direction * Speed;
        IsActive = true;
    }

    public void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Logic to deactivate the projectile if it goes off-screen or hits something
        // Example: if the projectile goes out of bounds
        if (Position.X < 0 || Position.X > Game1.ScreenWidth || Position.Y < 0 || Position.Y > Game1.ScreenHeight)
        {
            IsActive = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }

    public Rectangle Collider
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
    }
}
