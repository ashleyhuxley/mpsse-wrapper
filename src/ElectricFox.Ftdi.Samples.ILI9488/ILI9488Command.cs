namespace ElectricFox.Ftdi.Samples.ILI9488
{
    public enum ILI9488Command : byte
    {
        SoftReset = 0x01,
        PositiveGamma = 0xE0,
        NegativeGamma = 0xE1,
        Power1 = 0xC0,
        Power2 = 0xC1,
        VcomControl = 0xC5,
        MemoryAccessControl = 0x36,
        InterfacePixelFormat = 0x3A,
        InterfaceModeControl = 0xB0,
        FrameRateControl = 0xB1,
        DisplayInversionControl = 0xB4,
        DisplayFunctionControl = 0xB6,
        EntryMode = 0xB7,
        AdjustControl3 = 0xF7,
        SleepOut = 0x11,
        DisplayOn = 0x29,
        ColumnAddressSet = 0x2A,
        PageAddressSet = 0x2B,
        MemoryWrite = 0x2C
    }
}
