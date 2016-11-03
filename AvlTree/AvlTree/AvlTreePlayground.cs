using System;

namespace AvlTree
{
    class AvlTreePlayground
    {
        static void Main()
        {
            var tree = new AvlTree<int>();
            tree.Add(5);
            tree.Add(8);
            tree.Add(10);
            tree.ForeachDfs((depth, val) =>
            {
                Console.WriteLine("{0}{1}", new string('-', depth), val);
            });
        }
    }
}
