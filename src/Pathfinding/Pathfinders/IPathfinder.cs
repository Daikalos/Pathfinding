using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    interface IPathfinder
    {
        List<Vertex> PathTo(Grid grid, Graph graph, Vertex start, Vertex goal);
    }
}
