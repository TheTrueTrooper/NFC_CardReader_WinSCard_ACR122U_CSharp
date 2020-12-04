using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public class ACR122U_MifareClassic_Status
    {
        public bool Card { get; private set; }
        public ACR122U_StatusErrorCodes ErrorCode { get; private set; }
        public bool FieldPresent { get; private set; }
        public byte NumberOfTargets { get; private set; }
        public byte LogicalNumber { get; private set; }
        public ACR122U_StatusBitRateInReception BitRateInReception { get; private set; }
        public ACR122U_StatusBitsRateInTransmiton BitRateInTransmition { get; private set; }
        public ACR122U_StatusModulationType ModulationType { get; private set; }

        internal ACR122U_MifareClassic_Status(bool Card, ACR122U_StatusErrorCodes ErrorCode, bool FieldPresent, byte NumberOfTargets, byte LogicalNumber, ACR122U_StatusBitRateInReception BitRateInReception, ACR122U_StatusBitsRateInTransmiton BitRateInTransmition, ACR122U_StatusModulationType ModulationType)
        {
            this.Card = Card;
            this.ErrorCode = ErrorCode;
            this.FieldPresent = FieldPresent;
            this.NumberOfTargets = NumberOfTargets;
            this.LogicalNumber = LogicalNumber;
            this.BitRateInReception = BitRateInReception;
            this.BitRateInTransmition = BitRateInTransmition;
            this.ModulationType = ModulationType;
        }
    }
}
