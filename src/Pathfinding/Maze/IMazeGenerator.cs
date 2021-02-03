using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    interface IMazeGenerator
    {
        void Generate(Graph graph, Grid grid, Vertex start);
    }
}
