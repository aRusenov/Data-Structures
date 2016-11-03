using System.Linq;

namespace AvlTree.Tests
{
    public static class TestUtils
    {
        public static int[] ToIntArrayUnique(string input)
        {
            return input.Split(' ')
                .Select(int.Parse)
                .Distinct()
                .ToArray();
        }
    }
}
