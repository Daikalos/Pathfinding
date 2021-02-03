using System.Collections.Generic;

namespace Pathfinding
{
    interface IPathfinder
    {
        List<Vertex> PathTo(Grid grid, Graph graph, Vertex start, Vertex goal);
    }
}
