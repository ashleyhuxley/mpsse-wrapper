using ElectricFox.Ftdi.Mpsse;

namespace ElectricFox.Ftdi.SPI
{
    public class SpiException : Exception
    {
        public FtStatus? FtcStatus { get; set; }

        public SpiException() { }

        public SpiException(string message) : base(message) { }

        public SpiException(string message, FtStatus ftcStatus)
            : base(message)
        {
            this.FtcStatus = ftcStatus;
        }
    }
}
