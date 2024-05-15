using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace TDJ_PJ2;

public class EntityManager
{
    #region Events related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion

    #region Consts
    private const int PLAYER_HEALTH = 100;
    private const int BARRICADE_HEALTH = 400;
    #endregion

    #region Fields
    public List<IEntity> Entities { get; private set; }
    public int BarricadeHealth;
    #endregion

    #region Constructor
    public EntityManager()
    {
        Entities = new List<IEntity>();
        //BarricadeHealth = BARRICADE_HEALTH;

        /* Adding entities */
        // Player
        //Entities.Add(new Player(new Vector2(128.0f, Game1.ScreenHeight - 100.0f), AssetManager.Instance().GetSprite("Player"), 100));

        // Subscribing to events(doing a collision event here since the health for the barricade is here. Very bad design)
        //Zombie.BarricadeCollisionEvent += OnBarricadeCollision;
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        // Deleting the entity from the list if it's inactive
        for (int i = 0; i < Entities.Count; i++)
        {
            // Decreasing the "i" so not to skip over any entities
            if (!Entities[i].IsActive)
            {
                Entities.RemoveAt(i);
                i--;
            }
        }

        // Updating the entities
        foreach (var entity in Entities)
        {
            // Update for the collisions
            entity.CollisionUpdate(Entities);

            // Normal entity update
            entity.Update(gameTime);
        }

        // Ending the game once the barricade is broken
        if (BarricadeHealth <= 0)
            SceneChangeEvent?.Invoke(SceneType.Over);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var entity in Entities)
        {
            entity.Render(spriteBatch);
        }
    }

    public void OnBarricadeCollision(Zombie zombie)
    {
        BarricadeHealth -= zombie.Damage;

        zombie.Velocity = new Vector2(0.0f, 0.0f);
    }
    #endregion
}