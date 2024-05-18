using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TDJ_PJ2
{
    public class SpawnManager
    {
        #region Consts
        // Basic zombie consts
        private const int BASIC_HEALTH = 40;
        private const int BASIC_DAMAGE = 12;
        private const float BASIC_SPEED = 1.0f;

        // Brute zombie consts
        private const int BRUTE_HEALTH = 80;
        private const int BRUTE_DAMAGE = 20;
        private const float BRUTE_SPEED = 1.0f;
        #endregion

        #region Fields
        private List<Vector2> m_SpawnPoints = new List<Vector2>();
        private char[,] m_Map;
        private EntityManager m_EntityManager;
        private int m_Timer;
        private int m_MaxTime;
        private int m_SpawnInterval;
        private int m_NumSpawnPerInterval;
        private int m_SpawnCounter;
        #endregion

        #region Constructor
        public SpawnManager(char[,] map, EntityManager entityManager, int spawnInterval = 120, int numSpawnPerInterval = 10)
        {
            m_Map = map;
            m_EntityManager = entityManager;

            // Find spawn points from the map
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 'S')
                    {
                        m_SpawnPoints.Add(new Vector2(x * 64, y * 64));
                    }
                }
            }

            m_Timer = 0;
            m_MaxTime = spawnInterval;
            m_SpawnInterval = spawnInterval;
            m_NumSpawnPerInterval = numSpawnPerInterval;
            m_SpawnCounter = 0;
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            m_Timer += gameTime.ElapsedGameTime.Milliseconds;

            // This timer will define the difficulty of the game.
            // Once this timer is passed a certain threshold, the zombies will begin to spawn more frequently.
            if (m_Timer >= m_SpawnInterval)
            {
                m_Timer = 0;

                // Increase the spawn counter
                m_SpawnCounter++;

                // Spawning zombies
                SpawnEntities();
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawnPerInterval; i++)
            {
                // Adding a zombie
                SpawnEntity();
            }
        }

        private void SpawnEntity()
        {

            if (m_EntityManager.Entities.Count == 0)
            {
                foreach (Vector2 spawnPoint in m_SpawnPoints)
                {
                    // Spawn either a basic or a brute zombie, depending on the spawn counter
                    if (m_SpawnCounter % 5 == 0)
                    {
                        Rectangle hitBox = new Rectangle((int)spawnPoint.X, (int)spawnPoint.Y, 64, 64);
                        m_EntityManager.Entities.Add(new Zombie(new Vector2(spawnPoint.X + 32, spawnPoint.Y + 32),
                                                                AssetManager.Instance().GetSprite("BruteZombie"),
                                                                BRUTE_HEALTH,
                                                                BRUTE_DAMAGE,
                                                                BRUTE_SPEED,
                                                                GetPathPoints(spawnPoint),
                                                                m_Map));
                    }
                    else
                    {
                        Rectangle hitBox = new Rectangle((int)spawnPoint.X, (int)spawnPoint.Y, 64, 64);
                        m_EntityManager.Entities.Add(new Zombie(new Vector2(spawnPoint.X, spawnPoint.Y),
                                                                AssetManager.Instance().GetSprite("BasicZombie"),
                                                                BASIC_HEALTH,
                                                                BASIC_DAMAGE,
                                                                BASIC_SPEED,
                                                                GetPathPoints(spawnPoint),
                                                                m_Map));
                    }
                }
            }
        }

        private List<Vector2> GetPathPoints(Vector2 spawnPoint)
        {
            // Get the path points
            List<Vector2> pathPoints = new List<Vector2>();

            // Convert spawn point to map coordinates
            int startX = (int)(spawnPoint.X / 64);
            int startY = (int)(spawnPoint.Y / 64);

            // Current position of the zombie (centralizado no tile)
            float x = startX * 64 + 32;
            float y = startY * 64 + 32;

            // Add the initial spawn point to the path
            pathPoints.Add(new Vector2(x, y));

            // Directions
            int dx = 0;
            int dy = 0;

            while (m_Map[startX, startY] != 'F')
            {
                // Check all directions
                if (IsInMap(startX + 1, startY) && m_Map[startX + 1, startY] == 'C' && dx != -1)
                {
                    dx = 1;
                    dy = 0;
                }
                else if (IsInMap(startX - 1, startY) && m_Map[startX - 1, startY] == 'C' && dx != 1)
                {
                    dx = -1;
                    dy = 0;
                }
                else if (IsInMap(startX, startY + 1) && m_Map[startX, startY + 1] == 'C' && dy != -1)
                {
                    dx = 0;
                    dy = 1;
                }
                else if (IsInMap(startX, startY - 1) && m_Map[startX, startY - 1] == 'C' && dy != 1)
                {
                    dx = 0;
                    dy = -1;
                }
                else
                {
                    // No valid path found
                    break;
                }

                // Move to the new position
                startX += dx; // Atualizando a posição no mapa
                startY += dy; // Atualizando a posição no mapa
                x = startX * 64 + 32; // Centralize no próximo tile
                y = startY * 64 + 32; // Centralize no próximo tile

                // Add the new position to the path
                pathPoints.Add(new Vector2(x, y));
            }

            return pathPoints;
        }

        private bool IsInMap(int x, int y)
        {
            return x >= 0 && x < m_Map.GetLength(0) && y >= 0 && y < m_Map.GetLength(1);
        }
        #endregion
    }
}
