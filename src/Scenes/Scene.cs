using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDJ_PJ2;

public interface IScene
{
    public void Update(GameTime gameTime);
    public void Render(SpriteBatch spriteBatch);
}