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
        private GameScene gameScene; // apagar

        private int delta = 0;
        private Texture2D[][] sprites;
        private Direction direction = Direction.Down;
        private Vector2 directionVector;
        private int speed = 2; // NOTA: tem de ser divisor de tileSize

        // Constructor
        public Player(Texture2D texture, int x, int y)
        {
            this.texture = texture;
            position = new Point(x, y);
        }



        public void Update(GameTime gameTime)
        {
            if (delta > 0)
            {
                delta = (delta + speed) % 64;
            }
            else
            {
                KeyboardState kState = Keyboard.GetState();

                if (kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.Left))
                {
                    position.X--;
                    direction = Direction.Left;
                    delta = speed;
                    // directionVector = new Vector2(-1, 0);
                    directionVector = -Vector2.UnitX;
                }
                else if (kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.Up))
                {
                    position.Y--;
                    direction = Direction.Up;
                    delta = speed;
                    directionVector = -Vector2.UnitY;
                }
                else if (kState.IsKeyDown(Keys.S) || kState.IsKeyDown(Keys.Down))
                {
                    position.Y++;
                    direction = Direction.Down;
                    delta = speed;
                    directionVector = Vector2.UnitY;
                }
                else if (kState.IsKeyDown(Keys.D) || kState.IsKeyDown(Keys.Right))
                {
                    position.X++;
                    direction = Direction.Right;
                    delta = speed;
                    directionVector = Vector2.UnitX;
                }
            }
        }




        public void Draw(SpriteBatch sb) 
        {
            Vector2 pos = position.ToVector2() * 64;

            //int frame = 0;
            //if (delta > 0)
            //{
            //    pos -= (64 - delta) * directionVector;
            //    float animSpeed = 8f;
            //    frame = (int)((delta / speed) % ((int)animSpeed * sprites[(int)direction].Length) / animSpeed);
            //}

            Rectangle rect = new Rectangle(pos.ToPoint(), new Point(64));
            // sb.Draw(sprites[(int)direction][frame], rect, Color.White);
            sb.Draw(texture, rect, Color.White);
        }

    }
}
