using System.Collections.Generic;
using Graph;

namespace Pathfinding
{
    interface IPathfinder
    {
        List<Vertex> PathTo(Grid grid, WGraph graph, Vertex start, Vertex goal);
    }
}
