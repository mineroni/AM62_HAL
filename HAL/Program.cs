using System.Diagnostics;

namespace HAL;
internal class Program
{
    static void Main(string[] args)
    {
        while (!Debugger.IsAttached)
        {
            Thread.Sleep(100);
        }
        Console.WriteLine("Hello, World!");
        int i = 0;
        while (true)
        {
            Console.WriteLine(++i);
            Thread.Sleep(1000);
            if(i == 1000)
                break;
        }
    }
}
