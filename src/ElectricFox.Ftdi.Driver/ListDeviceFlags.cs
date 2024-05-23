namespace ElectricFox.Ftdi.Driver
{
    public enum ListDeviceOptions : uint
    {
        NumberOnly = 0x80000000,
        ByIndex = 0x40000000,
        All = 0x20000000
    }
}
