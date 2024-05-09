using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJ_PJ2;

public class Tile
{
    #region Fields  
    public Vector2 Position {get; set;}
    public Texture2D Texture {get; private set;}
    #endregion

    #region Constructor
    public Tile(Vector2 position, Texture2D texture)
    {
        Position = position;
        Texture = texture;
    }
    #endregion

    #region Methods
    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
    #endregion
}