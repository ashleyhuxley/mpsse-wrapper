using ElectricFox.Ftdi.SPI;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ElectricFox.Ftdi.Samples.ILI9488
{
    /// <summary>
    /// This program is an example of using the ElectricFox SPI wrapper
    /// for the FT232H chip to drive an ILI9488 based TFT Display.
    /// The program loads an example bitmap image and sends the pixel
    /// data to the display over SPI.
    /// 
    /// Call the program with the path to the image as an argument.
    /// Image must be 320x480 pixels.
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please specify an image");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File does not exist");
                return;
            }

            var numberOfCahnnels = SpiDevice.GetNumberOfChannels();
            if (numberOfCahnnels < 1)
            {
                Console.WriteLine("No SPI channels detected. Ensure FTDII drivers are correctly installed.");
                return;
            }

            for (int i = 0; i < numberOfCahnnels; i++)
            {
                var channelInfo = SpiDevice.GetChannelInfo(0);
                Console.WriteLine($"Channel {0}:");
                Console.WriteLine($"  ID: {channelInfo.ID}");
                Console.WriteLine($"  Description: {channelInfo.Description}");
                Console.WriteLine($"  Location ID: {channelInfo.LocId}");
                Console.WriteLine($"  Serial Number: {channelInfo.SerialNumber}");
                Console.WriteLine();
            }

            uint chanelIndex = 0;
            if (numberOfCahnnels > 1)
            {
                Console.WriteLine("Enter cahnnel index: ");
                var input = Console.ReadLine();
                if (!uint.TryParse(input, out chanelIndex) || chanelIndex < 0 || chanelIndex >= numberOfCahnnels)
                {
                    Console.WriteLine("Invalid channel index. Using 0.");
                    chanelIndex = 0;
                }
            }

            try
            {
                using (var display = new ILI9488Device(chanelIndex))
                {
                    //display.Test();
                    display.Initialize();
                    display.DisplayImage(new Bitmap(args[0]));
                }
            }
            catch (SpiException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done.");
        }
    }
}
