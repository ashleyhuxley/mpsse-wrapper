using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.I2C
{
    /// <summary>
    /// See https://www.ftdichip.com/Support/Documents/ProgramGuides/FTCI2CPG11.pdf
    /// </summary>
    public partial class I2CDevice
    {
        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_GetNumDevices", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_GetNumDevices(ref int numberOfDevices);

        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_GetDeviceNameLocID", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_GetDeviceNameLocID(int deviceIndex, ref string deviceName, int bufferSize, ref int locationId);

        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_Open", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_Open(ref int handle);

        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_OpenEx", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_OpenEx(string deviceName, int locationId, ref int handle);

        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_Close", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_Close(int handle);

        [LibraryImport(@"C:\WINDOWS\System32\FTDI2C.DLL", EntryPoint = "I2C_InitDevice", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_InitDevice(int handle, int clockDivisor);
        

        public int GetNumberOfDevices()
        {
            int numberOfDevices = 0;
            var result = (FtcStatus)I2C_GetNumDevices(ref numberOfDevices);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to get number of devices", result);
            }

            return numberOfDevices;
        }

        public I2CDeviceInfo GetDeviceName(int deviceIndex)
        {
            string deviceName = string.Empty;
            int locationId = 0;
            const int BufferSize = 50;

            var result = (FtcStatus)I2C_GetDeviceNameLocID(deviceIndex, ref deviceName, BufferSize, ref locationId);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to get device name for index {deviceIndex}", result);
            }

            return new I2CDeviceInfo(deviceName, locationId);
        }

        public int OpenDevice()
        {
            int handle = 0;

            var result = (FtcStatus)I2C_Open(ref handle);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to open device", result);
            }

            return handle;
        }

        public int OpenDevice(I2CDeviceInfo device)
        {
            int handle = 0;

            var result = (FtcStatus)I2C_OpenEx(device.Name, device.Location, ref handle);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to open device", result);
            }

            return handle;
        }

        public void CloseDevice(int handle)
        {
            var result = (FtcStatus)I2C_Close(handle);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to close device", result);
            }
        }

        public void InitDevice(int handle, ushort clockDivisor = 0)
        {
            var result = (FtcStatus)I2C_InitDevice(handle, clockDivisor);

            if (result != FtcStatus.Success)
            {
                throw new I2CException($"Unable to initialize device", result);
            }
        }
    }
}
