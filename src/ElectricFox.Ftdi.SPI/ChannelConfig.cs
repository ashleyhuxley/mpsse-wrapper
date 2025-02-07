using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.SPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelConfig
    {
        public uint ClockRate {  get; set; }
        public byte LatencyTimer { get; set; }
        public uint Options { get; set; }
        public uint Pins { get; set; }
        public uint Reserved { get; set; }
    }
}
