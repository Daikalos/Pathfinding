using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    public interface IPathfinder
    {
        List<Vertex> PathTo(Vertex start, Vertex goal);
    }
}
