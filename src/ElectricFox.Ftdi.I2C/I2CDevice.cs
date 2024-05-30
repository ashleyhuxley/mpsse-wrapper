using System.Formats.Asn1;
using System.Net;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectricFox.Ftdi.I2C
{
    /// <summary>
    /// See https://www.ftdichip.com/Support/Documents/ProgramGuides/FTCI2CPG11.pdf
    /// </summary>
    public partial class I2CDevice
    {
        [LibraryImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", EntryPoint = "I2C_GetNumChannels", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_GetNumChannels(ref uint numberOfChannels);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", EntryPoint = "I2C_GetChannelInfo")]
        private static extern int I2C_GetChannelInfo(uint index, ref DeviceListInfoNode channelInfo);

        [LibraryImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll")]
        private static partial int I2C_OpenChannel(uint index, ref IntPtr handle);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll")]
        private static extern int I2C_InitChannel(IntPtr handle, ref ChannelConfig config);

        [LibraryImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll")]
        private static partial int I2C_CloseChannel(IntPtr handle);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll")]
        private static extern int I2C_DeviceRead(
            IntPtr handle,
            uint deviceAddress,
            uint sizeToTransfer,
            byte[] buffer,
            ref uint sizeTransferred,
            uint options);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int I2C_DeviceWrite(
            IntPtr handle,
            uint deviceAddress,
            uint sizeToTransfer,
            byte[] buffer,
            ref uint sizeTransferred,
            uint options);

        private IntPtr handle;

        protected I2CDevice() { }

        public static uint GetNumberOfChannels()
        {
            uint channels = 0;
            var result = (FtcStatus)I2C_GetNumChannels(ref channels);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to get number of channels", result);
            }

            return channels;
        }

        public static DeviceListInfoNode GetChannelInfo(uint index)
        {
            var info = new DeviceListInfoNode();

            var result = (FtcStatus)I2C_GetChannelInfo(index, ref info);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to get channel info at device index {index}", result);
            }

            return info;
        }

        public static I2CDevice Open(uint index)
        {
            var newHandle = IntPtr.Zero;

            var result = (FtcStatus)I2C_OpenChannel(index, ref newHandle);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to open device at index {index}", result);
            }

            var device = new I2CDevice
            {
                handle = newHandle
            };

            return device;
        }

        public void Initialize(
            I2CClockRate clockRate,
            byte latencyTimer,
            bool disable3PhaseClocking = false,
            bool enableDriveOnlyZero = false)
        {

            int options = 0;

            if (disable3PhaseClocking)
            {
                options |= 1;
            }

            if (enableDriveOnlyZero)
            {
                options |= (1 << 1);
            }

            var config = new ChannelConfig
            {
                clockRate = clockRate,
                latencyTimer = latencyTimer,
                options = options
            };

            var result = (FtcStatus)I2C_InitChannel(this.handle, ref config);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to initialize device", result);
            }
        }

        public void Close()
        {
            var result = (FtcStatus)I2C_CloseChannel(handle);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to close device", result);
            }
        }

        public byte[] Read(uint address, uint bytes)
        {
            uint bytesTransferred = 0;

            byte[] data = new byte[bytes];

            var result = (FtcStatus)I2C_DeviceRead(
            this.handle,
                address,
                (uint)data.Length,
                data,
                ref bytesTransferred,
            0);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to write data to device at address {address}", result);
            }

            return data;
        }

        public uint Write(uint address, byte[] data)
        {
            uint bytesTransferred = 0;

            var result = (FtcStatus)I2C_DeviceWrite(
                this.handle,
                address, 
                (uint)data.Length,
                data, 
                ref bytesTransferred,
                0);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to write data to device at address {address}", result);
            }

            return bytesTransferred;
        }
    }
}
