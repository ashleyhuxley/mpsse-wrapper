﻿using ElectricFox.Ftdi.Driver;

namespace ElectricFox.Ftdi.CmdLine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(FtdiDriver.GetNumberOfDevices(ListDeviceOptions.NumberOnly));
        }
    }
}