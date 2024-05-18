using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDJ_PJ2.src.Entities
{

    enum Direction
    {
        Up, Down, Left, Right // 0, 1, 2, 3
    }
    public class Player
    {
        // Player position
        private Point position;

        // Player texture
        private Texture2D texture;
        private GameScene gameScene;

        // Constructor
        public Player(Texture2D texture, int x, int y)
        {
            this.texture = texture;
            position = new Point(x, y);
        }

        public void Draw(SpriteBatch sb) 
        {
            // codigo que devia funcionar, gameScene.tileSize = 64
            //Vector2 pos = position.ToVector2() * gameScene.tileSize;
            //Rectangle rect = new Rectangle(pos.ToPoint(), new Point(gameScene.tileSize));

            Vector2 pos = position.ToVector2() * 64;
            Rectangle rect = new Rectangle(pos.ToPoint(), new Point(64));
            sb.Draw(texture, rect, Color.White);
        }

    }
}
