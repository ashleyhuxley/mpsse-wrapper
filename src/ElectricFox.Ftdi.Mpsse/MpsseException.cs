namespace ElectricFox.Ftdi.Mpsse
{
    public class MpsseException : Exception
    {
        public MpsseException() { }

        public MpsseException(string message) : base(message) { }

        public MpsseException(FtStatus returnCode) : base($"Return Code: {returnCode}") { }
    }
}
