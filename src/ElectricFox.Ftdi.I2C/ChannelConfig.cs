namespace ElectricFox.Ftdi.I2C
{
    internal struct ChannelConfig
    {
        public I2CClockRate clockRate;

        public byte latencyTimer;

        public int options;
    }
}
