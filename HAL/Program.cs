using System.Diagnostics;
using static GpioLib;

namespace HAL;
internal class Program
{
    // Example method to interact with GPIO
    public static void ToggleGpioPin(string chipName, uint offset)
    {
        int lineValue = 0;
        bool isOutput = true;

        while (true)
        {
            lineValue = (lineValue == 0) ? 1 : 0;

            // Set the GPIO value (output high/low)
            Console.WriteLine($"Setting pin {offset} to {lineValue}");
            System.Threading.Thread.Sleep(1000);
        }
    }

    static string[] gpioNames = { "SODIMM_206", "SODIMM_208", "SODIMM_210", "SODIMM_212", "SODIMM_216", "SODIMM_218", "SODIMM_220", "SODIMM_222" };
    static void Main(string[] args)
    {
        Console.WriteLine("Waiting for debugger!");
        while (!Debugger.IsAttached)
        {
            Thread.Sleep(100);
        }
        Console.Clear();
        while (true)
        {
            uint readedPin = 0;
            Console.Write("Please specify the gpio pin number: ");
            while (!uint.TryParse(Console.ReadLine(), out readedPin))
            {
                Console.WriteLine("Invalid input!");
                Console.Write("Please specify the gpio pin number: ");
            }
            string chipPath = readedPin < 5 ? "/dev/gpiochip1" : "/dev/gpiochip2";
            int lineValue = 0;
            IntPtr request = RequestOutputLine(chipPath, readedPin, GpiodLineValue.Inactive, "toggle-line-value");

            if (request == IntPtr.Zero)
            {
                Console.WriteLine("Failed to request line.");
                return;
            }
            //while (true)
            for (int j = 0; j < 20; j++)
            {
                Console.WriteLine($"{readedPin}={ValueStr(lineValue == 0 ? GpiodLineValue.Inactive : GpiodLineValue.Active)}");
                Thread.Sleep(1000);
                lineValue = (lineValue == 0) ? 1 : 0;
                gpiod_line_request_set_value(request, readedPin, lineValue == 0 ? GpiodLineValue.Inactive : GpiodLineValue.Active);
            }
            gpiod_line_request_release(request);
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
