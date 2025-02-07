using ElectricFox.Ftdi.Mpsse;

namespace ElectricFox.Ftdi.I2C
{
    public class I2CException : Exception
    {
        public FtStatus? FtcStatus { get; set; }

        public I2CException() { }

        public I2CException(string message) : base(message) { }

        public I2CException(string message, FtStatus ftcStatus)
            : base(message)
        {
            this.FtcStatus = ftcStatus;
        }
    }
}
