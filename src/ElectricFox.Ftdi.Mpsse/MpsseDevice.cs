namespace ElectricFox.Ftdi.Mpsse
{
    using System.Runtime.InteropServices;

    public static partial class MpsseDevice
    {
        // ===========================================================================================================================
        // Declarations for device information functions in FTD2XX.dll:
        // ===========================================================================================================================
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", EntryPoint = "FT_ListDevices", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_GetNumberOfDevices(ref int lngNumberOfDevices, string pvArg2, int lngFlags);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_GetDeviceString(int lngDeviceIndex, string lpszDeviceString, int lngFlags);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_GetDeviceInfo(int lngHandle, ref int lngFT_Type, ref int lngID, string pucSerialNumber, string pucDescription, ref byte pvDummy);



        // ===========================================================================================================================
        // Declarations for functions in FTD2XX.dll:
        // ===========================================================================================================================
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_OpenByIndex(int intDeviceNumber, ref int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_OpenBySerialNumber(string SerialNumber, int lngFlags, ref int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_OpenByDescription(string Description, int lngFlags, ref int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_Close(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_Read_String(int lngHandle, string lpvBuffer, int lngBufferSize, ref int lngBytesReturned);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int FT_Write_String(int lngHandle, string lpvBuffer, int lngBufferSize, ref int lngBytesWritten);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_Read_Bytes(int lngHandle, ref byte lpvBuffer, int lngBufferSize, ref int lngBytesReturned);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_Write_Bytes(int lngHandle, ref byte lpvBuffer, int lngBufferSize, ref int lngBytesWritten);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetBaudRate(int lngHandle, int lngBaudRate);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetDivisor(int lngHandle, int intDivisor);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetDataCharacteristics(int lngHandle, byte byWordLength, byte byStopBits, byte byParity);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetFlowControl(int lngHandle, int intFlowControl, byte byXonChar, byte byXoffChar);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_ResetDevice(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetDtr(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_ClrDtr(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetRts(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_ClrRts(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_GetModemStatus(int lngHandle, ref int lngModemStatus);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetChars(int lngHandle, byte byEventChar, byte byEventCharEnabled, byte byErrorChar, byte byErrorCharEnabled);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_Purge(int lngHandle, int lngMask);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetTimeouts(int lngHandle, int lngReadTimeout, int lngWriteTimeout);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_GetQueueStatus(int lngHandle, ref int lngRxBytes);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_GetLatencyTimer(int lngHandle, ref byte ucTimer);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetLatencyTimer(int lngHandle, byte ucTimer);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_GetBitMode(int lngHandle, ref byte ucMode);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetBitMode(int lngHandle, byte ucMask, byte ucEnable);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetUSBParameters(int lngHandle, int lngInTransferSize, int lngOutTransferSize);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetBreakOn(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetBreakOff(int lngHandle);
        
        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_GetStatus(int lngHandle, ref int lngamountInRxQueue, ref int lngAmountInTxQueue, ref int lngEventStatus);

        [LibraryImport(@"C:\WINDOWS\System32\FTD2XX.DLL")]
        private static partial int FT_SetEventNotification(int lngHandle, int lngEventMask, int lngEvent);

        public static int GetNumberOfDevices(ListDeviceOptions options)
        {
            int result = 0;
            var returnCode = (ReturnCode)FT_GetNumberOfDevices(ref result, "/0", (int)options);
            if (returnCode != 0)
            {
                throw new MpsseException(returnCode);
            }

            return result;
        }
    }
}