namespace ElectricFox.Ftdi.I2C
{
    public class I2CException : Exception
    {
        public FtcStatus? FtcStatus { get; set; }

        public I2CException() { }

        public I2CException(string message) : base(message) { }

        public I2CException(string message, FtcStatus ftcStatus)
            : base(message)
        {
            this.FtcStatus = ftcStatus;
        }
    }
}
