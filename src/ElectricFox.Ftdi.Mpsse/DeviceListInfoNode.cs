using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.Mpsse
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceListInfoNode
    {
        public uint Flags;
        public uint Type;
        public uint ID;
        public uint LocId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SerialNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Description;

        public IntPtr Handle;
    }
}
