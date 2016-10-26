using System;

namespace List
{
    class ListPlayground
    {
        static void Main()
        {
            var list = new CustomList<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }

            list.RemoveAt(7);
            list.Insert(60, 2);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
