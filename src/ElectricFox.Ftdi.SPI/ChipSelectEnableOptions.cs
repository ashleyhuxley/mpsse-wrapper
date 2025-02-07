namespace ElectricFox.Ftdi.SPI
{
    internal enum ChipSelectEnableOptions : uint
    {
        None = 0x00000000U,
        Enable = 0x00000002U,
        Disable = 0x00000004U,
    }
}
