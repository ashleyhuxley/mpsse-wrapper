using ElectricFox.Ftdi.I2C;
using ElectricFox.Ftdi.Mpsse;

namespace ElectricFox.Ftdi.CmdLine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(I2CDevice.GetNumberOfChannels());
        }
    }
}
