using ElectricFox.Ftdi.I2C;

namespace ElectrcFox.Ftdi.Tests
{
    public class Tests
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        //[Test]
        //[TestCase(0, 1)]
        //[TestCase(1, 2)]
        //[TestCase(2, 4)]
        //[TestCase(3, 8)]
        //[TestCase(4, 16)]
        //[TestCase(5, 32)]
        //[TestCase(6, 64)]
        //[TestCase(7, 128)]
        //public void SinglePinModeTest(int pin, byte expected)
        //{
        //    I2CTestDevice device = new();
        //    device.PinMode(pin, PinMode.Output);
        //    Assert.That(device.PinDirections, Is.EqualTo(expected));
        //}

        //[TestCase(7, 127)]
        //[TestCase(6, 191)]
        //[TestCase(5, 223)]
        //[TestCase(4, 239)]
        //[TestCase(3, 247)]
        //[TestCase(2, 251)]
        //[TestCase(1, 253)]
        //[TestCase(0, 254)]
        //public void PinModeInput_DoesNotAffectOtherPins(int pin, byte expected)
        //{
        //    I2CTestDevice device = new();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        device.PinMode(i, PinMode.Output);
        //    }

        //    device.PinMode(pin, PinMode.Input);
            
        //    Assert.That(device.PinDirections, Is.EqualTo(expected));
        //}

    }
}