namespace ElectricFox.Ftdi.Mpsse
{
    public class LowerBytePins
    {
        private readonly bool[] _initialPinDirections = new bool[8];
        private readonly bool[] _initialPinStates = new bool[8];
        private readonly bool[] _finalPinDirections = new bool[8];
        private readonly bool[] _finalPinStates = new bool[8];

        public bool[] InitialPinDirections => _initialPinDirections;
        public bool[] InitialPinStates => _initialPinStates;
        public bool[] FinalPinDirections => _finalPinDirections;
        public bool[] FinalPinStates => _finalPinStates;

        public uint Value
        {
            get
            {
                uint result = 0;

                // Pack _initialPinDirections into bits 0-7
                for (int i = 0; i < 8; i++)
                {
                    if (_initialPinDirections[i])
                    {
                        result |= (1u << i);
                    }
                }

                // Pack _initialPinStates into bits 8-15
                for (int i = 0; i < 8; i++)
                {
                    if (_initialPinStates[i])
                    {
                        result |= (1u << (i + 8));
                    }
                }

                // Pack _finalPinDirections into bits 16-23
                for (int i = 0; i < 8; i++)
                {
                    if (_finalPinDirections[i])
                    {
                        result |= (1u << (i + 16));
                    }
                }

                // Pack _finalPinStates into bits 24-31
                for (int i = 0; i < 8; i++)
                {
                    if (_finalPinStates[i])
                    {
                        result |= (1u << (i + 24));
                    }
                }

                return result;
            }
        }
    }
}
