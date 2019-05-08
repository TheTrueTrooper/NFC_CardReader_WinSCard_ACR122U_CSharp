using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122U
{
    public enum ACR122U_StatusErrorCodes
    {
        /// <summary>
        /// No Error
        /// </summary>
        ACR122U_Error_NoError = 0x00,
        /// <summary>
        /// Time Out, the target has not answered
        /// </summary>
        TimeOut = 0x01,
        /// <summary>
        /// A CRC error has been detected by the contactless UART
        /// </summary>
        CRCError = 0x02,
        /// <summary>
        /// A Parity error has been detected by the contactless UART
        /// </summary>
        ParityError = 0x03,
        /// <summary>
        /// During a MIFARE anti-collision/select operation, an erroneous Bit Count has been detected 
        /// </summary>
        MIFAREAntiCollisionSelectErroneousBitCount = 0x04,
        /// <summary>
        /// Framing error during MIFARE operation 
        /// </summary>
        FramingError = 0x05,
        /// <summary>
        /// An abnormal bit-collision has been detected during bit wise anti-collision at 106 Kbps 
        /// </summary>
        AbnormalBitCollision = 0x06,
        /// <summary>
        /// Communication buffer size insufficient 
        /// </summary>
        CommunicationBufferSizeInsufficient = 0x07,
        /// <summary>
        /// RF Buffer overflow has been detected by the contactless UART (bit BufferOvfl of the register CL_ERROR) 
        /// </summary>
        RFBufferOverflow = 0x08,
        /// <summary>
        /// In active communication mode, the RF field has not been switched on in time by the counterpart (as defined in NFCIP-1 standard) 
        /// </summary>
        ActiveCommunicationModeNoRF = 0x0A,
        /// <summary>
        /// RF Protocol error (cf. reference [4], description of the CL_ERROR register) 
        /// </summary>
        RFProtocolError = 0x0B,
        /// <summary>
        /// Temperature error: the internal temperature sensor has detected overheating, and therefore has automatically switched off the antenna drivers 
        /// </summary>
        TemperatureErrorOverheating = 0x0D,
        /// <summary>
        /// Internal buffer overflow
        /// </summary>
        InternalBufferOverflow = 0x0E,
        /// <summary>
        /// Invalid parameter(range, format, …)
        /// </summary>
        InvalidParameter = 0x10,
        /// <summary>
        /// DEP Protocol: The chip configured in target mode does not support the command received from the initiator(the command received is not one of the following: ATR_REQ, WUP_REQ, PSL_REQ, DEP_REQ, DSL_REQ, RLS_REQ, ref. [1]). 
        /// </summary>
        DEPProtocolBadCommand = 0x12,
        /// <summary>
        /// DEP Protocol / MIFARE / ISO/IEC 14443-4: The data format does not match to the specification.Depending on the RF protocol used, it can be: • Bad length of RF received frame, • Incorrect value of PCB or PFB, • Invalid or unexpected RF received frame, • NAD or DID incoherence. 
        /// </summary>
        DEPProtocolBadDataFormat = 0x13,
        /// <summary>
        /// MIFARE: Authentication error 
        /// </summary>
        AuthenticationError = 0x14,
        /// <summary>
        /// ISO/IEC 14443-3: UID Check byte is wrong
        /// </summary>
        UIDCheckError = 0x23,
        /// <summary>
        /// DEP Protocol: Invalid device state, the system is in a state which does not allow the operation 
        /// </summary>
        DEPProtocolInvalidDeviceState = 0x25,
        /// <summary>
        /// Operation not allowed in this configuration (host controller interface) 
        /// </summary>
        OperationNotAllowedHost = 0x26,
        /// <summary>
        /// This command is not acceptable due to the current context of the chip(Initiator vs. Target, unknown target number, Target not in the good state, …)
        /// </summary>
        CommandNotAcceptableInContext = 0x27,
        /// <summary>
        /// The chip configured as target has been released by its initiator 2Ah ISO/IEC 14443-3B only: the ID of the card does not match, meaning that the expected card has been exchanged with another one.
        /// </summary>
        IDOfCardMismatch = 0x29
    }
}
