using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Pathfinding
{
    class Maze
    {
        private readonly Grid grid;
        private readonly Graph graph;

        private IMazeGenerator mazeGenerator;

        public Maze(Grid grid, Graph graph)
        {
            this.grid = grid;
            this.graph = graph;

            mazeGenerator = new RDFS();
        }

        public void Generate()
        {
            mazeGenerator.Generate(graph, grid, 
                graph.AtPos(
                    StaticRandom.RandomNumber(0, graph.Width),
                    StaticRandom.RandomNumber(0, graph.Height)));
        }
    }
}
