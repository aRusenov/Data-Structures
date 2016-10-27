using IntervalTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalTree
{
    class IntervalTreePlayground
    {
        static void Main(string[] args)
        {
            var timetable = new []
            {
                new Interval<int>(20, 36),
                new Interval<int>(3, 41),
                new Interval<int>(29, 99),
                new Interval<int>(0, 1),
                new Interval<int>(10, 15)
            };

            var intervalTree = new IntervalTree<int>();
            foreach (var interval in timetable)
            {
                intervalTree.Insert(interval);
            }

            Console.WriteLine("Count: {0}", intervalTree.Count);

            var overlappingIntervals = new List<Interval<int>>();
            intervalTree.GetOverlappingIntervals(new Interval<int>(10, 22), overlappingIntervals);
            foreach (var interval in overlappingIntervals)
            {
                Console.WriteLine("{0}-{1}", interval.Low, interval.High);
            }
        }
    }
}
