using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TDJ_PJ2
{
    public class Camera
    {
        private GraphicsDevice graphicsDevice;

        public Vector2 Position { get; private set; }
        public float Zoom { get; set; }

        public Matrix TransformMatrix =>
            Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
            Matrix.CreateScale(Zoom, Zoom, 1);

        public Camera(GraphicsDevice graphicsDevice, Vector2 initialPosition, float initialZoom = 1.0f)
        {
            this.graphicsDevice = graphicsDevice;
            Position = initialPosition;
            Zoom = initialZoom;
        }

        public void Follow(Vector2 targetPosition, int mapWidth, int mapHeight, int tileSize)
        {
            Vector2 targetPositionCentered = targetPosition - new Vector2(graphicsDevice.Viewport.Width / 2 / Zoom, graphicsDevice.Viewport.Height / 2 / Zoom);

            float maxX = mapWidth * tileSize - graphicsDevice.Viewport.Width / Zoom;
            float maxY = mapHeight * tileSize - graphicsDevice.Viewport.Height / Zoom;

            float clampedX = MathHelper.Clamp(targetPositionCentered.X, 0, maxX);
            float clampedY = MathHelper.Clamp(targetPositionCentered.Y, 0, maxY);

            Position = new Vector2(clampedX, clampedY);
        }
    }
}
