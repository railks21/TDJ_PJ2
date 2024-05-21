using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;

namespace TDJ_PJ2;

public class EntityManager
{
    #region Events related
    public delegate void SceneChange(SceneType sceneType);
    public static event SceneChange SceneChangeEvent;
    #endregion

    #region Consts
    private const int HEALTH = 100;
    #endregion

    #region Fields
    public List<IEntity> Entities { get; private set; }
    public int Health;
    #endregion

    #region Constructor
    public EntityManager()
    {
        Entities = new List<IEntity>();
        Health = HEALTH;

        // Subscribing to events(doing a collision event here since the health for the barricade is here. Very bad design)
        Zombie.FinalDestionationEvent += FinalDestination;
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        // Deleting the entity from the list if it's inactive
        List<IEntity> entitiesCopy = Entities.ToList();

        for (int i = 0; i < entitiesCopy.Count; i++)
        {
            if (!entitiesCopy[i].IsActive)
            {
                Entities.Remove(entitiesCopy[i]);
            }
        }

        // Updating the entities
        foreach (var entity in entitiesCopy)
        {
            // Update for the collisions
            entity.CollisionUpdate(Entities);

            // Normal entity update
            entity.Update(gameTime);
        }

        // Ending the game once the barricade is broken
        if (Health <= 0)
            SceneChangeEvent?.Invoke(SceneType.Over);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var entity in Entities)
        {
            entity.Render(spriteBatch);
        }
    }

    public void FinalDestination(Zombie zombie)
    {
        Health -= 1;

        // Remove  zumbi from list
        for (int i = Entities.Count - 1; i >= 0; i--)
        {
            if (Entities[i] == zombie)
            {   
                Entities.RemoveAt(i); 
                break;
            }
        }
    }
    #endregion
}