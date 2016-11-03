using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{
    class StackPlayground
    {
        static void Main(string[] args)
        {
            var stack = new CustomStack<string>(2);
            stack.Push("Pesho");
            stack.Push("Gosho");
            stack.Push("Penio");

            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Capacity {0}", stack.Capacity);
            Console.WriteLine("Count {0}", stack.Count);

            stack.Pop();
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Capacity {0}", stack.Capacity);
            Console.WriteLine("Count {0}", stack.Count);
        }
    }
}
