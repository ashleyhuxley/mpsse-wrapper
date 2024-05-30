using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.I2C
{
    /// <summary>
    /// See https://www.ftdichip.com/Support/Documents/ProgramGuides/FTCI2CPG11.pdf
    /// </summary>
    public partial class I2CDevice
    {
        [LibraryImport(@"libmpsse.dll", EntryPoint = "I2C_GetNumChannels", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int I2C_GetNumChannels(ref int numberOfChannels);

        public static int GetNumberOfChannels()
        {
            var channels = 0;

            var result = (FtcStatus)I2C_GetNumChannels(ref channels);

            if (result != FtcStatus.Success)
            {
                throw new I2CException("Could not get number of channels");
            }

            return channels;
        }
    }
}
