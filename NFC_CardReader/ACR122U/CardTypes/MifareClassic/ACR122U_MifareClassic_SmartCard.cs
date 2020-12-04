using NFC_CardReader.WinSCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U.CardTypes.MifareClassic
{
    public class ACR122U_MifareClassic_SmartCard : ACR122U_ISO14443TypeAStandard_SmartCard
    {
        protected internal ACR122U_MifareClassic_SmartCard(WinSmartCard MakeFrom) : base(MakeFrom)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }

        protected internal ACR122U_MifareClassic_SmartCard(WinSmartCardContext Parent, IntPtr Card) : base(Parent, Card)
        {
            CardType = ACR122U_SupportedRFCardTypes.MifareClassics;
        }

        #region CardCommandsAllFor1KForNow
        /// <summary>
        /// Gets the UID as as a string
        /// </summary>
        /// <returns></returns>
        public string GetcardUID()//only for mifare 1k cards
        {

            string cardUID = "";
            byte[] receivedUID;

            LastACRResultCode = GetcardUIDBytes(out receivedUID);

            if (LastACRResultCode != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(LastACRResultCode, LastResultCode);

            cardUID = BitConverter.ToString(receivedUID);

            return cardUID;
        }

        /*Get ATS 
         *Send
         FF CA 01 00 00
         *Returns
         [Data] [Data] [Data] [Data] [SWCodeP1] [SWCodeP2]
         */
        /// <summary>
        /// Gets The ATS Current particuar hardware or firmware or card doesnt support
        /// </summary>
        /// <param name="receivedUID"></param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes GetcardATSBytes(out byte[] receivedUID)//only for mifare 1k cards
        {

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xCA, 0x01, 0x00, 0x00 };

            TransmitData(CommandAsBytes, out receivedUID);

            LastACRResultCode = RetrieveDataCodes(ref receivedUID);
            return LastACRResultCode;
        }

        /// <summary>
        /// Gets The ATS Current particuar hardware or firmware or card doesnt support
        /// </summary>
        /// <param name="receivedUID"></param>
        /// <returns></returns>
        public string GetcardATS()//only for mifare 1k cards
        {

            string cardATS = "";
            byte[] receivedUID;

            LastACRResultCode = GetcardATSBytes(out receivedUID);

            if (LastACRResultCode != ACR122U_ResposeErrorCodes.Success)
                throw new ACR122U_SmartCardException(LastACRResultCode, LastResultCode);

            cardATS = BitConverter.ToString(receivedUID);

            return cardATS;
        }

        /*Load Key
        *Send 
        FF 82 00 00 06 FF FF FF FF FF FF
        or
        FF 82 [KeyStructure:00(all others are reserved for fu)] [KeyNumber:00-01] 06 [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**] [KeyChunk0:**]
        *Returns
        90 00
        */
        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as 1 or 2 for posible memory locations</param>
        /// <param name="KeyValue">The Key Value to use. Length must be six</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes LoadAthenticationKeys(ACR122U_KeyMemories Key, byte[] KeyValue)//only for mifare 1k cards
        {
            byte[] Return;
            if (!Enum.IsDefined(typeof(ACR122U_KeyMemories), Key))
                throw new Exception("Your Key Load location is not a valid one.");
            if (KeyValue.Length != 6)
                throw new Exception("Your Key has too many or too few bites.\nKeys must have 6 bytes.");

            byte[] CommandAsBytes = new byte[11] { 0xFF, 0x82, 0x00, (byte)Key, 0x06, KeyValue[0], KeyValue[1], KeyValue[2], KeyValue[3], KeyValue[4], KeyValue[5] };

            TransmitData(CommandAsBytes, out Return);

            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /// <summary>
        /// Loads Athentication Keys into the system
        /// </summary>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <param name="KeyValue">The Value Key to use. Length must be six</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes LoadAthenticationKeys(ACR122U_Keys Key, byte[] KeyValue)
        {
            if (!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection is not a valid one.");

            return LoadAthenticationKeys((ACR122U_KeyMemories)Key, KeyValue);
        }

        /*Load Key
        *Send 
        FF 86 00 00 05 01 00 00 60 01
        or
        FF 86 00 00 05 01 00 [Block:00-FF] 6{000<KeyType:0=A,1=B>} [KeyNumber:00-01]
        *Returns
        90 00
        Note in a 4 block sector the last block is the key block
        */
        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="KeyToUse">A enumeration as 1 or 2 for posible memory locations</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_KeyMemories KeyToUse)
        {
            if (!Enum.IsDefined(typeof(ACR122U_KeyMemories), KeyToUse))
                throw new Exception("Your Key Load location is not a valid one.");
            if (!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection is not a valid one.");

            byte[] Return;
            byte[] CommandAsBytes = new byte[10] { 0xFF, 0x86, 0x00, 0x00, 0x05, 0x01, 0x00, BlockToAthenticate, (byte)(0x60 | (byte)Key), (byte)KeyToUse };

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /// <summary>
        /// Uses prev loaded Athentication Keys to Athenticate
        /// </summary>
        /// <param name="Key">A enumeration as A or B</param>
        /// <param name="Key">A enumeration as A or B for if you want to match keys to memory(A is read key B is master)</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Athentication(byte BlockToAthenticate, ACR122U_Keys Key, ACR122U_Keys KeyToUse)
        {
            if (!Enum.IsDefined(typeof(ACR122U_Keys), KeyToUse))
                throw new Exception("Your Key Selection for KeyToUse is not a valid one.");
            if (!Enum.IsDefined(typeof(ACR122U_Keys), Key))
                throw new Exception("Your Key Selection for Key is not a valid one.");
            LastACRResultCode = Athentication(BlockToAthenticate, Key, (ACR122U_KeyMemories)KeyToUse);
            return LastACRResultCode;
        }

        /*Read Bock
        *Send 
        FF B0 00 51 10
        or
        FF B0 00 [Block:00-FF] [NumberToRead:00-16]
        *Returns
        90 00
        */
        /// <summary>
        /// Reads a block
        /// </summary>
        /// <param name="DataOut">The data returned</param>
        /// <param name="BlockToRead">The block to read</param>
        /// <param name="NumberToRead">The number to read</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes ReadBlock(out byte[] DataOut, byte BlockToRead, byte NumberToRead = 16)
        {
            if (NumberToRead > 16 || NumberToRead < 0)// only one they let you with? :-/
                throw new Exception("Your Data In has too or too few bites.\nRead commands must have 0-16 bytes.");

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xB0, 0x00, BlockToRead, NumberToRead };

            TransmitData(CommandAsBytes, out DataOut);

            LastACRResultCode = RetrieveDataCodes(ref DataOut);
            return LastACRResultCode;
        }

        /*WriteBlock
        *Send 
        FF D6 00 51 10 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
        or
        FF D6 00 51 10 [Data]...
        *Returns
        90 00
        */
        /// <summary>
        /// Writes to a block. Note Must be athenticated with Key B first
        /// </summary>
        /// <param name="DataIn">Data to write </param>
        /// <param name="BlockToWrite">The Block to write the data to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes WriteBlock(byte[] DataIn, byte BlockToWrite)
        {
            byte[] Return;
            if (DataIn.Length != 16)
                throw new Exception("Your Data In has too or too few bites.\nWrite commands must have 16 bytes.");

            byte[] CommandAsBytes = new byte[21] { 0xFF, 0xD6, 0x00, BlockToWrite, 0x10, DataIn[0], DataIn[1], DataIn[2], DataIn[3], DataIn[4], DataIn[5], DataIn[6], DataIn[7], DataIn[8], DataIn[9], DataIn[10], DataIn[11], DataIn[12], DataIn[13], DataIn[14], DataIn[15] };

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /*ReadValue
        *Send 
        FF B1 00 34 04
        or
        FF B1 00 [Block] 04
        *Returns
        90 00
        */
        /// <summary>
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">The number output</param>
        /// <param name="BlockToRead">The Block to Read from</param>
        /// <returns></returns>
        // dont believe the compiler it lies my int is bigger.....
        public ACR122U_ResposeErrorCodes ReadValueFromBlock(out Int32 Value, byte BlockToRead)
        {
            ACR122U_ResposeErrorCodes Return;
            byte[] DataBack;

            byte[] CommandAsBytes = new byte[5] { 0xFF, 0xB1, 0x00, BlockToRead, 0x04 };

            TransmitData(CommandAsBytes, out DataBack);
            Return = RetrieveDataCodes(ref DataBack);
            if (Return == ACR122U_ResposeErrorCodes.Success)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(DataBack);
                Value = BitConverter.ToInt32(DataBack, 0);
            }
            else
                Value = 0;
            LastACRResultCode = Return;
            return LastACRResultCode;
        }

        /*WriteValue
        *Send 
        FF D7 00 00 05 00 FF FF FF FF
        or
        FF D7 00 [Block] 05 00 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
        *Returns
        90 00
        */
        /// <summary>
        /// Writes a 32 bit interger to the block
        /// </summary>
        /// <param name="Value">the number to write</param>
        /// <param name="BlockToWrite">The Block to write to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes WriteValueToBlock(Int32 Value, byte BlockToWrite)
        {
            byte[] Return;
            byte[] ValuesBytes;
            ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToWrite, 0x05, 0x00, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] };//BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /*IncrementValue
        *Send 
        FF D7 00 00 05 01 FF FF FF FF
        or
        FF D7 00 [Block] 05 01 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
        *Returns
        90 00
        */
        /// <summary>
        /// Increment number
        /// </summary>
        /// <param name="Value">The amount to add</param>
        /// <param name="BlockToIncrement">The block to add to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes IncrementValue(Int32 Value, byte BlockToIncrement)
        {
            byte[] Return;
            byte[] ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToIncrement, 0x05, 0x01, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] }; //BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /*IncrementValue
       *Send 
       FF D7 00 00 05 02 FF FF FF FF
       or
       FF D7 00 [Block] 05 02 [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] [ValueAsHex:00-FF] (short)
       *Returns
       90 00
       */
        /// <summary>
        /// decrement number
        /// </summary>
        /// <param name="Value">The amount to subtract</param>
        /// <param name="BlockToDecrement">The block to subtract to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes DecrementValue(Int32 Value, byte BlockToDecrement)
        {
            byte[] Return;
            byte[] ValuesBytes = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(ValuesBytes);

            byte[] CommandAsBytes = new byte[10] { 0xFF, 0xD7, 0x00, BlockToDecrement, 0x05, 0x02, ValuesBytes[0], ValuesBytes[1], ValuesBytes[2], ValuesBytes[3] };

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        /*Copy
        *Send 
        FF D7 00 00 02 03 00
        or
        FF D7 00 [SourceBlock] 02 03 [TargetBlock]
        *Returns
        90 00
        */
        /// <summary>
        /// Copies one block to another
        /// </summary>
        /// <param name="SourceBlock">Source Block copy from</param>
        /// <param name="TargetBlock">Target Block copy to</param>
        /// <returns></returns>
        public ACR122U_ResposeErrorCodes Copy(byte SourceBlock, byte TargetBlock)
        {
            byte[] Return;

            byte[] CommandAsBytes = new byte[7] { 0xFF, 0xD7, 0x00, SourceBlock, 0x02, 0x03, TargetBlock };//BitConverter.GetBytes(_GetcardUIDBytesCommand);

            TransmitData(CommandAsBytes, out Return);
            LastACRResultCode = RetrieveDataCodes(ref Return);
            return LastACRResultCode;
        }

        #endregion
    }
}
