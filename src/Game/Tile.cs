using Microsoft.Xna.Framework;
using Utilities;

namespace Pathfinding
{
    class Tile : GameObject
    {
        private readonly Vertex vertex;

        private Color color;
        private bool isWall;

        public Vertex Vertex => vertex;
        public bool IsWall => isWall;

        public Tile(Vector2 position, Point size, Vertex vertex) : base(position, size)
        {
            this.vertex = vertex;
        }

        public void UpdateColor()
        {
            color = isWall ? Color.LightGray : Color.DimGray;
        }

        public override void LoadContent()
        {
            AssignTexture("Node");
        }
    }
}
