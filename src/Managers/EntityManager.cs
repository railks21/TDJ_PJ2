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
    private const int PLAYER_HEALTH = 100;
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

        /* Adding entities */
        // Player
        //Entities.Add(new Player(new Vector2(128.0f, Game1.ScreenHeight - 100.0f), AssetManager.Instance().GetSprite("Player"), 100));

        // Subscribing to events(doing a collision event here since the health for the barricade is here. Very bad design)
        Zombie.BarricadeCollisionEvent += FinalDestination;
    }
    #endregion

    #region Methods
    public void Update(GameTime gameTime)
    {
        // Criando uma cópia da lista Entities
        List<IEntity> entitiesCopy = Entities.ToList();

        // Deletando a entidade da lista se estiver inativa
        for (int i = 0; i < entitiesCopy.Count; i++)
        {
            if (!entitiesCopy[i].IsActive)
            {
                Entities.Remove(entitiesCopy[i]);
            }
        }

        // Atualizando as entidades
        foreach (var entity in entitiesCopy)
        {
            // Atualização para as colisões
            entity.CollisionUpdate(Entities);

            // Atualização normal da entidade
            entity.Update(gameTime);
        }

        // Terminando o jogo uma vez que a barricada seja destruída
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