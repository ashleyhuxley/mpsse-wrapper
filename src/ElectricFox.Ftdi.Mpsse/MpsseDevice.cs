using System;
using System.Runtime.InteropServices;

namespace ElectricFox.Ftdi.Mpsse
{
    public abstract partial class MpsseDevice
    {
        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int FT_WriteGPIO(
            IntPtr handle,
            byte dir,
            byte value);

        protected IntPtr _handle = IntPtr.Zero;
        private byte _pinModes;
        private byte _pinStates;

        /// <summary>
        /// Sets the mode of a GPIO pin.
        /// </summary>
        /// <param name="pin">The GPIO pin (0-7).</param>
        /// <param name="mode">The desired mode (Input or Output).</param>
        public void SetPinMode(int pin, PinMode mode)
        {
            byte pinSelect = GetPinMask(pin);

            if (mode == PinMode.Output)
            {
                _pinModes |= pinSelect;
            }
            else
            {
                _pinModes &= (byte)~pinSelect;
            }
        }

        /// <summary>
        /// Sets the state of a GPIO pin.
        /// </summary>
        /// <param name="pin">The GPIO pin (0-7).</param>
        /// <param name="state">The desired state (true for high, false for low).</param>
        public void DigitalWrite(int pin, bool state)
        {
            if (_handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Cannot write to GPIO: Device is not open");
            }

            byte pinSelect = GetPinMask(pin);

            if (state)
            {
                _pinStates |= pinSelect;
            }
            else
            {
                _pinStates &= (byte)~pinSelect;
            }

            int result = FT_WriteGPIO(_handle, _pinModes, _pinStates);
            if (result != 0)
            {
                throw new InvalidOperationException($"FT_WriteGPIO failed with error code: {result}");
            }
        }

        private static byte GetPinMask(int pin)
        {
            if (pin < 0 || pin > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }
            return (byte)(1 << pin);
        }
    }
}