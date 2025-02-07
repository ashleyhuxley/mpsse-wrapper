using ElectricFox.Ftdi.Mpsse;
using ElectricFox.Ftdi.SPI;
using System.Drawing;

namespace ElectricFox.Ftdi.Samples.ILI9488
{
    public class ILI9488Device : IDisposable
    {
        // The ILI9488 needs two additional GPIO pins
        private const int PIN_CS = 4;
        private const int PIN_DC = 1;   // Data/Command. Bring LOW to enter command mode. HIGH for data mode.
        private const int PIN_RST = 2;  // Reset the device. Bring LOW temporarily (~0.1s)

        // TFT Dimensions
        private const int TFT_WIDTH = 320;
        private const int TFT_HEIGHT = 480;

        private const int CLOCK_RATE = 30000000;

        private readonly SpiDevice device;
        private bool disposedValue;

        public ILI9488Device(uint channelIndex)
        {
            device = SpiDevice.Open(channelIndex);

            device.SetPinMode(PIN_CS, PinMode.Output);
            device.SetPinMode(PIN_DC, PinMode.Output);
            device.SetPinMode(PIN_RST, PinMode.Output);
        }

        public void Initialize()
        {
            device.Initialize(
                CLOCK_RATE,
                SpiMode.SpiMode0,
                ChipSelectLine.DbusLine3,
                ChipSelectActiveMode.ActiveLow,
                1,
                new LowerBytePins());
        }

        public void Test()
        {
            for (int i = 0; i < 3; i++)
            {
                device.DigitalWrite(PIN_RST, true);
                Thread.Sleep(200);
                device.DigitalWrite(PIN_RST, false);
                Thread.Sleep(200);
            }

            for (int i = 0; i < 3; i++)
            {
                device.DigitalWrite(PIN_CS, true);
                Thread.Sleep(200);
                device.DigitalWrite(PIN_CS, false);
                Thread.Sleep(200);
            }

            for (int i = 0; i < 3; i++)
            {
                device.DigitalWrite(PIN_DC, true);
                Thread.Sleep(200);
                device.DigitalWrite(PIN_DC, false);
                Thread.Sleep(200);
            }
        }

        public void DisplayImage(Bitmap image)
        {
            if (image.Width != TFT_WIDTH || image.Height != TFT_HEIGHT)
            {
                throw new ArgumentException($"Image must be {TFT_WIDTH}x{TFT_HEIGHT}");
            }

            InitializeDisplay();

            SendCommand(ILI9488Command.ColumnAddressSet);
            SendData([0x00, 0x00, 0x01, 0x3F]);  // 320 columns (0x013F)

            SendCommand(ILI9488Command.PageAddressSet);
            SendData([0x00, 0x00, 0x01, 0xDF]);  // 480 rows (0x1DF)


            SendCommand(ILI9488Command.MemoryWrite);

            for (int x = 0; x < TFT_WIDTH; x++)
            {
                for (int y = 0; y < TFT_HEIGHT; y++)
                {
                    var colour = image.GetPixel(x, y);
                    SendData([colour.R, colour.G, colour.B]);
                }
            }
        }

        private void SendCommand(ILI9488Command command)
        {
            device.DigitalWrite(PIN_DC, false);

            //device.DigitalWrite(PIN_CS, false);
            device.Write([(byte)command], false, true, true);
            //device.DigitalWrite(PIN_CS, true);
        }

        private void SendData(byte[] data)
        {
            device.DigitalWrite(PIN_DC, true);

            //device.DigitalWrite(PIN_CS, false);
            device.Write(data, false, true, true);
            //device.DigitalWrite(PIN_CS, true);
        }

        public void SoftReset()
        {
            SendCommand(ILI9488Command.SoftReset);
        }

        private void InitializeDisplay()
        {
            // Perform a hard reset by bringing the RST pin low temporarily
            device.DigitalWrite(PIN_RST, false);
            Thread.Sleep(100);
            device.DigitalWrite(PIN_RST, true);
            Thread.Sleep(100);

            SetGammaControl();
            SetPowerControl();
            SetVcomControl();
            SetMemoryAccessControl();
            SetInterfacePixelFormat();
            SetInterfaceModeControl();
            SetFrameRateControl();
            SetDisplayInversionControl();
            SetDisplayFunctionControl();
            SetEntryMode();
            SetAdjustControl3();
            SetSleepOut();
            SetDisplayOn();
        }

        private void SetDisplayOn()
        {
            SendCommand(ILI9488Command.DisplayOn);
            SendData([0x80, 0x78]);
        }

        private void SetSleepOut()
        {
            SendCommand(ILI9488Command.SleepOut);
            SendData([0x80, 0x78]);
        }

        private void SetAdjustControl3()
        {
            SendCommand(ILI9488Command.AdjustControl3);
            SendData([0xA9, 0x51, 0x2C, 0x82]);
        }

        private void SetEntryMode()
        {
            SendCommand(ILI9488Command.EntryMode);
            SendData([0x06]);
        }

        private void SetDisplayFunctionControl()
        {
            SendCommand(ILI9488Command.DisplayFunctionControl);
            SendData([0x02, 0x02, 0x3b]);
        }

        private void SetDisplayInversionControl()
        {
            SendCommand(ILI9488Command.DisplayInversionControl);
            SendData([0x02]);
        }

        private void SetFrameRateControl()
        {
            SendCommand(ILI9488Command.FrameRateControl);
            SendData([0xA0, 0x11]);
        }

        private void SetInterfaceModeControl()
        {
            SendCommand(ILI9488Command.InterfaceModeControl);
            SendData([0x00]);
        }

        private void SetInterfacePixelFormat()
        {
            SendCommand(ILI9488Command.InterfacePixelFormat);
            SendData([0x66]);
        }

        private void SetMemoryAccessControl()
        {
            SendCommand(ILI9488Command.MemoryAccessControl);
            SendData([0x48]);
        }

        private void SetGammaControl()
        {
            // Positive Gamma Control
            SendCommand(ILI9488Command.PositiveGamma);
            SendData([0x00, 0x07, 0x0C, 0x05, 0x13, 0x09, 0x36, 0xAA, 0x46, 0x09, 0x10, 0x0D, 0x1A, 0x1E, 0x1F]);

            // Negative Gamma Control
            SendCommand(ILI9488Command.NegativeGamma);
            SendData([0x00, 0x20, 0x23, 0x04, 0x10, 0x06, 0x37, 0x56, 0x49, 0x04, 0x0C, 0x0A, 0x33, 0x37, 0x0F]);
        }

        private void SetPowerControl()
        {
            SendCommand(ILI9488Command.Power1);
            SendData([0x0E, 0x0E]);

            SendCommand(ILI9488Command.Power2);
            SendData([0x44]);
        }

        private void SetVcomControl()
        {
            SendCommand(ILI9488Command.VcomControl);
            SendData([0x00, 0x40, 0x00, 0x40]);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    device.Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
