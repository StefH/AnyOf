using ClassLibrary1;
using ClassLibrary2;

namespace ConsoleAppConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var class1 = new Class1();
            var result1 = class1.X1("");

            var class2 = new Class2();
            var result2 = class2.X2(result1); // 👈 CS1503 error
        }
    }
}