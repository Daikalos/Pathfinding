using System.Collections.Generic;

namespace Pathfinding
{
    public interface IPathfinder
    {
        List<Vertex> PathTo(Graph graph, Vertex start, Vertex goal);
    }
}
