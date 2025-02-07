using ElectricFox.Ftdi.Mpsse;
using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.I2C
{
    /// <summary>
    /// See https://www.ftdichip.com/Support/Documents/ProgramGuides/FTCI2CPG11.pdf
    /// </summary>
    public partial class I2CDevice : MpsseDevice
    {
        [LibraryImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", EntryPoint = "I2C_GetNumChannels")]
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

        protected I2CDevice() { }

        public static uint GetNumberOfChannels()
        {
            uint channels = 0;
            var result = (FtStatus)I2C_GetNumChannels(ref channels);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to get number of channels", result);
            }

            return channels;
        }

        public static DeviceListInfoNode GetChannelInfo(uint index)
        {
            var info = new DeviceListInfoNode();

            var result = (FtStatus)I2C_GetChannelInfo(index, ref info);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to get channel info at device index {index}", result);
            }

            return info;
        }

        public static I2CDevice OpenChannel(uint index)
        {
            var newHandle = IntPtr.Zero;

            var result = (FtStatus)I2C_OpenChannel(index, ref newHandle);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to open device at index {index}", result);
            }

            var device = new I2CDevice
            {
                _handle = newHandle
            };

            return device;
        }

        // TODO: Don't expose pin here, use a struct instead
        public void Initialize(
            I2C_ClockRate clockRate,
            byte latencyTimer,
            LowerBytePins pins,
            bool disable3PhaseClocking = false,
            bool enableDriveOnlyZero = false)
        {

            uint options = 0;

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
                ClockRate = clockRate,
                LatencyTimer = latencyTimer,
                Options = options,
                Pin = pins.Value
            };

            var result = (FtStatus)I2C_InitChannel(this._handle, ref config);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to initialize device", result);
            }
        }

        public void Close()
        {
            var result = (FtStatus)I2C_CloseChannel(_handle);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to close device", result);
            }
        }

        public byte[] Read(uint address, uint bytes)
        {
            uint bytesTransferred = 0;

            byte[] data = new byte[bytes];

            var result = (FtStatus)I2C_DeviceRead(
            this._handle,
                address,
                (uint)data.Length,
                data,
                ref bytesTransferred,
            0);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to write data to device at address {address}", result);
            }

            return data;
        }

        public uint Write(uint address, byte[] data)
        {
            uint bytesTransferred = 0;

            var result = (FtStatus)I2C_DeviceWrite(
                this._handle,
                address, 
                (uint)data.Length,
                data, 
                ref bytesTransferred,
                0);

            if (result != FtStatus.Success)
            {
                throw new I2CException($"Unable to write data to device at address {address}", result);
            }

            return bytesTransferred;
        }
    }
}
