using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TDJ_PJ2
{
    public class Camera
    {
        private GraphicsDevice graphicsDevice;

        public Vector2 Position { get; private set; }
        public Matrix TransformMatrix => Matrix.CreateTranslation(-Position.X, -Position.Y, 0);

        public Camera(GraphicsDevice graphicsDevice, Vector2 initialPosition)
        {
            this.graphicsDevice = graphicsDevice;
            Position = initialPosition;
        }

        public void Follow(Vector2 targetPosition)
        {
            // Adjust the camera's position to keep the target centered on the screen
            Position = targetPosition - new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
        }
    }
}
