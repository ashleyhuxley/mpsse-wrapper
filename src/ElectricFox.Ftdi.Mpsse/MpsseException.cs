namespace ElectricFox.Ftdi.Mpsse
{
    public class MpsseException : Exception
    {
        public MpsseException() { }

        public MpsseException(string message) : base(message) { }

        public MpsseException(ReturnCode returnCode) : base($"Return Code: {returnCode}") { }
    }
}
