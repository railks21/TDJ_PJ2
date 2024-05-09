using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace TDJ_PJ2;

public class TileManager
{
    #region Fields
    private const int TILE_SIZE = 32;
    private List<Tile> m_Tiles = new List<Tile>();
    private Texture2D m_TileSrcTexture;
    private Texture2D m_PropSrcTexture;
    private Color[] m_TileRawData;
    private Color[] m_PropRawData;
    #endregion

    #region Constructor
    public TileManager()
    {
        // Setting the main texture
        m_TileSrcTexture = AssetManager.Instance().GetMap("TileMap");
        m_PropSrcTexture = AssetManager.Instance().GetMap("PropMap");

        // Setting the size of the array for the raw data
        m_TileRawData = new Color[m_TileSrcTexture.Width * m_TileSrcTexture.Height];
        m_PropRawData = new Color[m_PropSrcTexture.Width * m_PropSrcTexture.Height];

        // Getting the raw color data from the texture
        m_TileSrcTexture.GetData<Color>(m_TileRawData);
        m_PropSrcTexture.GetData<Color>(m_PropRawData);

        // Adds the tiles to the list
        for (int i = 0; i < m_TileSrcTexture.Height; i++)
        {
            for (int j = 0; j < m_TileSrcTexture.Width; j++)
            {
                //// Getting the color of the current pixel
                //Color currentColor = m_TileRawData[i * m_TileSrcTexture.Width + j];

                ///* Tiles */
                //// Yellow = Withered grass
                //if (currentColor == new Color(255, 255, 0))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("WitheredGrass")));
                //// Green = Grass
                //else if (currentColor == new Color(0, 255, 0))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Grass")));
                //// Red = Grass patch
                //if (currentColor == new Color(255, 0, 0))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("GrassPatch")));
            }
        }

        // Adds the props to the list
        for (int i = 0; i < m_PropSrcTexture.Height; i++)
        {
            for (int j = 0; j < m_PropSrcTexture.Width; j++)
            {
                //Color currentColor = m_PropRawData[i * m_PropSrcTexture.Width + j];

                ///* Props */
                //// Brown = Box1
                //if (currentColor == new Color(100, 58, 27))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Box1")));
                //// Rosy brown = Box2
                //else if (currentColor == new Color(188, 143, 143))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Box2")));
                //// Blue = Flower1
                //else if (currentColor == new Color(0, 0, 255))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Flower1")));
                //// Yellow = Flower2
                //else if (currentColor == new Color(255, 255, 0))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Flower2")));
                //// Gray = Rock
                //else if (currentColor == new Color(80, 80, 80))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("Rock")));
                //// Black = Withered bush
                //else if (currentColor == new Color(0, 0, 0))
                //    m_Tiles.Add(new Tile(new Vector2(j * TILE_SIZE, i * TILE_SIZE), AssetManager.Instance().GetTile("WitheredBush")));
            }
        }
    }
    #endregion

    #region Methods
    public void Render(SpriteBatch spriteBatch)
    {
        foreach(var tile in m_Tiles)
        {
            tile.Render(spriteBatch);
        }
    }
    #endregion
}