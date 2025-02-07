using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.I2C
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelConfig
    {
        public I2C_ClockRate ClockRate;
        public byte LatencyTimer;
        public uint Options;
        public uint Pin;
        public ushort CurrentPinState;
    }
}
