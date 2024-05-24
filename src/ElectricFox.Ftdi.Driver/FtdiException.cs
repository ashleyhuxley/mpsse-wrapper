namespace ElectricFox.Ftdi.Mpsse
{
    public class FtdiException : Exception
    {
        public FtdiException() { }

        public FtdiException(string message) : base(message) { }

        public FtdiException(ReturnCode returnCode) : base($"Return Code: {returnCode}") { }
    }
}
