using System;
using System.Collections.Generic;

namespace KdTree
{
    class KdTreePlayground
    {
        static void Main()
        {
            var stars = new List<Star>()
            {
                new Star("Crescent Nebula", 5.5f, 5f),
                new Star("Krogan DMZ", 6f, 15f),
                new Star("Local Cluster", 8f, 16f),
                new Star("Kepler Verge", 15f, 8f),
                new Star("Hades Gamma", 19f, 13f),
                new Star("Exodus Cluster", 12f, 13.5f),
                new Star("Artemis Tau", 15.4f, 17f),
            };

            var kdTree = new KdTree<Star>(stars);

            var results = new List<Star>();
            kdTree.GetInRadius(6.3f, 16.5f, 2, results);

            Console.WriteLine(string.Join(Environment.NewLine, results));
        }
    }

    class Star : IDimensions
    {
        public Star(string name, float x, float y)
        {
            this.Name = name;
            this.Dimensions = new[] { x, y };
        }

        public float[] Dimensions { get; private set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1:F1}, {2:F2})", 
                this.Name, this.Dimensions[0], this.Dimensions[1]);
        }
    }
}
