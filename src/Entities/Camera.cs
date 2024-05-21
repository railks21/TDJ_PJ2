using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TDJ_PJ2
{
    public class Camera
    {
        private GraphicsDevice graphicsDevice;

        public Vector2 Position { get; private set; }
        public float Zoom { get; set; } = 1.0f; // Default zoom level to 1 (no zoom)

        public Matrix TransformMatrix =>
            Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
            Matrix.CreateScale(Zoom, Zoom, 1); // Apply zoom factor

        public Camera(GraphicsDevice graphicsDevice, Vector2 initialPosition)
        {
            this.graphicsDevice = graphicsDevice;
            Position = initialPosition;
        }

        public void Follow(Vector2 targetPosition)
        {
            Position = targetPosition - new Vector2(graphicsDevice.Viewport.Width / 2 / Zoom, graphicsDevice.Viewport.Height / 2 / Zoom);
        }
    }
}
