using ElectricFox.Ftdi.Mpsse;
using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.SPI
{
    public class SpiDevice : MpsseDevice
    {
        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_GetNumChannels(ref uint numChannels);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_GetChannelInfo(uint index, ref DeviceListInfoNode deviceInfo);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_OpenChannel(uint index, ref IntPtr handle);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_InitChannel(IntPtr handle, ref ChannelConfig config);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_CloseChannel(IntPtr handle);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_Read(IntPtr handle, ref byte[] buffer, uint length, ref uint transferred, uint options);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_Write(IntPtr handle, ref byte[] buffer, uint length, ref uint transferred, uint options);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_ReadWrite(IntPtr handle, ref byte[] inBuffer, ref byte[] outBuffer, uint length, ref uint transferred, uint options);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_IsBusy(IntPtr handle, ref bool isBusy);

        [DllImport(@"D:\Code\me\mpsse-wrapper\src\libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern FtStatus SPI_ChangeCS(IntPtr handle, uint configOptions);

        private SpiDevice()
        { }

        /// <summary>
        /// This function gets the number of SPI channels that are connected to the host system. The number of ports available on each of these chips is different.
        /// </summary>
        /// <returns>The number of available SPI channels</returns>
        /// <exception cref="SpiException"></exception>
        public static uint GetNumberOfChannels()
        {
            uint channels = 0;
            var result = SPI_GetNumChannels(ref channels);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Unable to get number of channels", result);
            }

            return channels;
        }

        /// <summary>
        /// Returns information about a given channel index
        /// </summary>
        /// <param name="index">Index of the channel. Must be within the range of available channls returned by GetNumberOfChannels</param>
        /// <returns></returns>
        /// <exception cref="SpiException"></exception>
        public static DeviceListInfoNode GetChannelInfo(uint index)
        {
            var info = new DeviceListInfoNode();

            var result = SPI_GetChannelInfo(index, ref info);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Unable to get channel info at device index {index}", result);
            }

            return info;
        }

        /// <summary>
        /// Open an SPI channel at a given index.
        /// </summary>
        /// <param name="channelIndex">Index of the channel. Must be within the range of available channls returned by GetNumberOfChannels</param>
        /// <returns>An SpiDevice that can be used to interact with the SPI channel</returns>
        /// <exception cref="SpiException"></exception>
        public static SpiDevice Open(uint channelIndex)
        {
            var newHandle = IntPtr.Zero;

            var result = SPI_OpenChannel(channelIndex, ref newHandle);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Unable to open device at index {channelIndex}", result);
            }

            var device = new SpiDevice
            {
                _handle = newHandle
            };

            return device;
        }

        /// <summary>
        /// Initialise the SPI device
        /// </summary>
        /// <param name="clockRate">Clock rate of the SPI Bus in hertz (maximum 30MHz)</param>
        /// <param name="mode">SPI Mode</param>
        /// <param name="chipSelectLine">Which of the available lines should be used for chip select</param>
        /// <param name="chipSelectActiveMode">Chip Select Active High or Active Low</param>
        /// <param name="latencyTimer">Latency timer in milliseconds. FTDI recommends 2-255 for FT2232D or 1-255 for FT232H, FT2232H, FT4232H </param>
        /// <param name="pins">Initial and final states of the lower byte pins</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SpiException"></exception>
        public void Initialize(
            uint clockRate,
            SpiMode mode,
            ChipSelectLine chipSelectLine,
            ChipSelectActiveMode chipSelectActiveMode,
            byte latencyTimer,
            LowerBytePins pins)
        {
            if (clockRate > 30000000)
            {
                throw new ArgumentOutOfRangeException(nameof(clockRate), "Max clock rate is 30MHz");
            }

            uint options = (uint)mode 
                | (uint)chipSelectLine 
                | (uint)chipSelectActiveMode;

            var config = new ChannelConfig
            {
                ClockRate = clockRate,
                LatencyTimer = latencyTimer,
                Options = options,
                Pins = pins.Value
            };

            var result = SPI_InitChannel(this._handle, ref config);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Unable to initialize device", result);
            }
        }

        /// <summary>
        /// Close the device
        /// </summary>
        /// <exception cref="SpiException"></exception>
        public void Close()
        {
            var result = SPI_CloseChannel(this._handle);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Unable to close channel", result);
            }
        }

        /// <summary>
        /// Read a number of bits or bytes from the SPI device
        /// </summary>
        /// <param name="length">Length of the data to read. May be in bits or bytes depending on the value of the lengthInBits parmeter</param>
        /// <param name="lengthInBits">If true, the length is specified in bits rather than bytes</param>
        /// <param name="enableChipSelectBeforeTransfer">If true, the Chip Select line is enabled before transgfer</param>
        /// <param name="disableChipSelectAfterTransfer">If true, the Chip Select line is disabled after transfer</param>
        /// <returns></returns>
        /// <exception cref="SpiException"></exception>
        public byte[] Read(
            uint length,
            bool lengthInBits = false,
            bool enableChipSelectBeforeTransfer = true,
            bool disableChipSelectAfterTransfer = true)
        {
            uint options = 0;

            options |= 
                lengthInBits 
                    ? (uint)SpiTransferOptions.SizeInBits 
                    : (uint)SpiTransferOptions.SizeInBytes;

            options |=
                enableChipSelectBeforeTransfer
                    ? (uint)ChipSelectEnableOptions.Enable
                    : (uint)ChipSelectEnableOptions.None;

            options |=
                disableChipSelectAfterTransfer
                    ? (uint)ChipSelectEnableOptions.Disable
                    : (uint)ChipSelectEnableOptions.None;

            int bufferSize = lengthInBits ? (int)Math.Ceiling(length / 8.0) : (int)length;
            var buffer = new byte[bufferSize];

            uint transferred = 0;
            var result = SPI_Read(this._handle, ref buffer, length, ref transferred, options);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"SPI Read Failed", result);
            }

            return buffer;
        }

        /// <summary>
        /// Write the specified data to the SPI device
        /// </summary>
        /// <param name="data">The data to write</param>
        /// <returns>The number of bytes transferred</returns>
        /// <exception cref="SpiException"></exception>
        public uint Write(
            byte[] buffer,
            bool lengthInBits = false,
            bool enableChipSelectBeforeTransfer = true,
            bool disableChipSelectAfterTransfer = true)
        {
            uint transferred = 0;

            uint options = 0;

            options |=
                lengthInBits
                    ? (uint)SpiTransferOptions.SizeInBits
                    : (uint)SpiTransferOptions.SizeInBytes;

            options |=
                enableChipSelectBeforeTransfer
                    ? (uint)ChipSelectEnableOptions.Enable
                    : (uint)ChipSelectEnableOptions.None;

            options |=
                disableChipSelectAfterTransfer
                    ? (uint)ChipSelectEnableOptions.Disable
                    : (uint)ChipSelectEnableOptions.None;

            var result = SPI_Write(this._handle, ref buffer, (uint)buffer.Length, ref transferred, options);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"SPI Write Failed", result);
            }

            return transferred;
        }

        /// <summary>
        /// Simultaneously read and write data from the SPI device
        /// </summary>
        /// <param name="writeBuffer">The data to send</param>
        /// <param name="length">The number of bits or bytes to read</param>
        /// <param name="lengthInBits">If true, the length of data to read is specified in bits rather than bytes</param>
        /// <param name="enableChipSelectBeforeTransfer">If true, the Chip Select line is enabled before transgfer</param>
        /// <param name="disableChipSelectAfterTransfer">If true, the Chip Select line is disabled after transfer</param>
        /// <returns></returns>
        /// <exception cref="SpiException"></exception>
        public byte[] ReadWrite(
            byte[] writeBuffer,
            uint length,
            bool lengthInBits = false,
            bool enableChipSelectBeforeTransfer = true,
            bool disableChipSelectAfterTransfer = true)
        {
            uint options = 0;

            options |=
                lengthInBits
                    ? (uint)SpiTransferOptions.SizeInBits
                    : (uint)SpiTransferOptions.SizeInBytes;

            options |=
                enableChipSelectBeforeTransfer
                    ? (uint)ChipSelectEnableOptions.Enable
                    : (uint)ChipSelectEnableOptions.None;

            options |=
                disableChipSelectAfterTransfer
                    ? (uint)ChipSelectEnableOptions.Disable
                    : (uint)ChipSelectEnableOptions.None;

            uint transferred = 0;

            int bufferSize = lengthInBits ? (int)Math.Ceiling(length / 8.0) : (int)length;
            var readBuffer = new byte[bufferSize];

            var result = SPI_ReadWrite(
                this._handle,
                ref readBuffer,
                ref writeBuffer,
                (uint)writeBuffer.Length,
                ref transferred,
                options);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"SPI ReadWrite Failed", result);
            }

            return readBuffer;
        }

        /// <summary>
        /// Reads the state of the MISO line without clocking the SPI bus
        /// </summary>
        /// <returns>True if the MISO line is busy, otherwise false</returns>
        /// <exception cref="SpiException"></exception>
        public bool IsBusy()
        {
            bool isBusy = false;

            var result = SPI_IsBusy(this._handle, ref isBusy);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Could not get busy status", result);
            }

            return isBusy;
        }

        /// <summary>
        /// Change the Chip Select options
        /// </summary>
        /// <param name="mode">SPI Mode</param>
        /// <param name="chipSelectLine">Which of the available lines should be used for chip select</param>
        /// <param name="chipSelectActiveMode">Chip Select Active High or Active Low</param>
        /// <exception cref="SpiException"></exception>
        public void ChangeChipSelect(
            ChipSelectActiveMode mode,
            ChipSelectLine chipSelectLine,
            ChipSelectActiveMode chipSelectActiveMode)
        {
            uint options = (uint)mode
            | (uint)chipSelectLine
            | (uint)chipSelectActiveMode;

            var result = SPI_ChangeCS(this._handle, options);

            if (result != FtStatus.Success)
            {
                throw new SpiException($"Could not change chip select", result);
            }
        }
    }
}
