using ElectricFox.Ftdi.I2C;
using ElectricFox.Ftdi.Mpsse;

namespace ElectricFox.Ftdi.CmdLine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const uint htu_addr = 0x40;
            const byte htu_readTemp = 0xE3;
            const byte htu_readHumidity = 0xE3;
            const byte htu_writeRegister = 0xE6;
            const byte htu_readRegister = 0xE7;
            const byte htu_reset = 0xFE;

            Console.WriteLine(I2CDevice.GetNumberOfChannels());

            var info = I2CDevice.GetChannelInfo(0);

            Console.WriteLine($"Serial: {info.SerialNumber}");
            Console.WriteLine($"Description: {info.Description}");

            var device = I2CDevice.Open(0);
            device.Initialize(I2CClockRate.FastMode, 255);

            Thread.Sleep(200);

            try
            {
                device.Write(htu_addr, [htu_reset]);
            }
            catch (I2CException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(Enum.GetName(typeof(FtcStatus), ex.FtcStatus));
            }

            Thread.Sleep(20);

            device.Write(htu_addr, [htu_readTemp]);

            Thread.Sleep(50);

            var data = device.Read(htu_addr, 3);

            Console.WriteLine($"Data: {data[0]}, {data[1]}, {data[2]}");

            device.Close();
        }
    }
}
