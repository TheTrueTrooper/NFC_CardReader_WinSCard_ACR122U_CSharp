using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    ///  <summary>
    ///  Smart card reader attribute enumeration.
    ///  </summary>
    /// <remarks>Can be used as parameter for the following methods:
    public enum SmartCardATR
    {
        /// <summary>
        /// Vendor name. (SCARD_ATTR_VENDOR_NAME)
        /// </summary>
        SCARD_ATTR_VendorName = 0x00400100, //(VendorInfo << 16) 400000(1)

        /// <summary>
        /// Vendor-supplied interface device type (model designation of reader). (SCARD_ATTR_VENDOR_IFD_TYPE)
        /// </summary>
        SCARD_ATTR_VendorInterfaceDeviceType = 0x00400101, 

        /// <summary>
        /// Vendor-supplied interface device version (DWORD in the form 0xMMmmbbbb where MM = major version, mm = minor version, and bbbb = build number).  (SCARD_ATTR_VENDOR_IFD_VERSION)
        /// </summary>
        SCARD_ATTR_VendorInterfaceDeviceTypeVersion = 0x00400102,

        /// <summary>
        /// Vendor-supplied interface device serial number. (SCARD_ATTR_VENDOR_IFD_SERIAL_NO)
        /// </summary>
        SCARD_ATTR_VendorInterfaceDeviceTypeSerialNumber = 0x00400103,

        /// <summary>
        /// DWORD encoded as 0xDDDDCCCC, where DDDD = data channel type and CCCC = channel number (SCARD_ATTR_CHANNEL_ID)
        /// </summary>
        SCARD_ATTR_ChannelId = 0x00800110, //(Communication << 16) 800000(2)

        /// <summary>
        /// Asynchronous protocol types (SCARD_ATTR_ASYNC_PROTOCOL_TYPES)
        /// </summary>
        SCARD_ATTR_AsyncProtocolTypes = 0x00C00120, //(Protocol << 16) C00000(3)

        /// <summary>
        /// Default clock rate, in kHz. (SCARD_ATTR_DEFAULT_CLK)
        /// </summary>
        SCARD_ATTR_DefaultClockRate = 0x00C00121,

        /// <summary>
        /// Maximum clock rate, in kHz. (SCARD_ATTR_MAX_CLK)
        /// </summary>
        SCARD_ATTR_MaxClockRate = 0x00C00122,

        /// <summary>
        /// Default data rate, in bps. (SCARD_ATTR_DEFAULT_DATA_RATE)
        /// </summary>
        SCARD_ATTR_DefaultDataRate = 0x00C00123,

        /// <summary>
        /// Maximum data rate, in bps. (SCARD_ATTR_MAX_DATA_RATE)
        /// </summary>
        SCARD_ATTR_MaxDataRate = 0x00C00124,

        /// <summary>
        /// Maximum bytes for information file size device. (SCARD_ATTR_MAX_IFSD)
        /// </summary>
        SCARD_ATTR_MaxInformationFileSizeDevice = 0x00C00125,

        /// <summary>
        /// Synchronous protocol types (SCARD_ATTR_SYNC_PROTOCOL_TYPES)
        /// </summary>
        SCARD_ATTR_SyncProtocolTypes = 0x00C00126,

        /// <summary>
        /// Zero if device does not support power down while smart card is inserted. Nonzero otherwise. (SCARD_ATTR_POWER_MGMT_SUPPORT)
        /// </summary>
        SCARD_ATTR_PowerManagementSupport = 0x001000131, // (PowerManagement << 16) 1000000(4)

        /// <summary>
        /// User to card authentication device (SCARD_ATTR_USER_TO_CARD_AUTH_DEVICE)
        /// </summary>
        SCARD_ATTR_USER_TO_CARD_AUTH_DEVICE = 0x001400140, //(Security << 16)(5)1400000

        /// <summary>
        /// User authentication input device (SCARD_ATTR_USER_AUTH_INPUT_DEVICE)
        /// </summary>
        SCARD_ATTR_USER_AUTH_INPUT_DEVICE = 0x001400142,

        /// <summary>
        /// DWORD indicating which mechanical characteristics are supported. If zero, no special characteristics are supported. Note that multiple bits can be set (SCARD_ATTR_CHARACTERISTICS)
        /// </summary>
        SCARD_ATTR_Characteristics = 0x001800150,//(Mechanical << 16)(6)

        /// <summary>
        /// Current protocol type (SCARD_ATTR_CURRENT_PROTOCOL_TYPE)
        /// </summary>
        SCARD_ATTR_CurrentProtocolType = 0x002000201,//(InterfaceDeviceProtocol << 16)(8)2000000

        /// <summary>
        /// Current clock rate, in kHz. (SCARD_ATTR_CURRENT_CLK)
        /// </summary>
        SCARD_ATTR_CurrentClockRate = 0x002000202,

        /// <summary>
        /// Clock conversion factor. (SCARD_ATTR_CURRENT_F)
        /// </summary>
        SCARD_ATTR_CurrentClockConversionFactor = 0x002000203,

        /// <summary>
        /// Bit rate conversion factor. (SCARD_ATTR_CURRENT_D)
        /// </summary>
        SCARD_ATTR_CurrentBitRateConversionFactor = 0x002000204,

        /// <summary>
        /// Current guard time. (SCARD_ATTR_CURRENT_N)
        /// </summary>
        SCARD_ATTR_CurrentGuardTime = 0x002000205,

        /// <summary>
        /// Current work waiting time. (SCARD_ATTR_CURRENT_W)
        /// </summary>
        SCARD_ATTR_CurrentWaitingTime = 0x002000206,

        /// <summary>
        /// Current byte size for information field size card. (SCARD_ATTR_CURRENT_IFSC)
        /// </summary>
        SCARD_ATTR_CurrentInformationFieldSizeCard = 0x002000207,

        /// <summary>
        /// Current byte size for information field size device. (SCARD_ATTR_CURRENT_IFSD)
        /// </summary>
        SCARD_ATTR_CurrentInformationFieldSizeDevice = 0x002000208,

        /// <summary>
        /// Current block waiting time. (SCARD_ATTR_CURRENT_BWT)
        /// </summary>
        SCARD_ATTR_CurrentBlockWaitingTime = 0x002000209,

        /// <summary>
        /// Current character waiting time. (SCARD_ATTR_CURRENT_CWT)
        /// </summary>
        SCARD_ATTR_CurrentCharacterWaitingTime = 0x00200020A,

        /// <summary>
        /// Current error block control encoding. (SCARD_ATTR_CURRENT_EBC_ENCODING)
        /// </summary>
        SCARD_ATTR_CurrentErrorBlockControlEncoding = 0x00200020B,

        /// <summary>
        /// Extended block wait time. (SCARD_ATTR_EXTENDED_BWT)
        /// </summary>
        SCARD_ATTR_ExtendedBlockWaitTime = 0x00200020C,

        /// <summary>
        /// Single byte indicating smart card presence(SCARD_ATTR_ICC_PRESENCE)
        /// </summary>
        SCARD_ATTR_ICCPresence = 0x002400300, // (ICCState << 16)2400000(9)

        /// <summary>
        /// Single byte. Zero if smart card electrical contact is not active; nonzero if contact is active. (SCARD_ATTR_ICC_INTERFACE_STATUS)
        /// </summary>
        SCARD_ATTR_ICCInterfaceStatus = 0x002400301,

        /// <summary>
        /// Current IO state (SCARD_ATTR_CURRENT_IO_STATE)
        /// </summary>
        SCARD_ATTR_CurrentIOState = 0x002400302,

        /// <summary>
        /// Answer to reset (ATR) string. (SCARD_ATTR_ATR_STRING)
        /// </summary>
        SCARD_ATTR_AtrString = 0x002400303,

        /// <summary>
        /// Answer to reset (ATR) string. (SCARD_ATTR_ATR_STRING)
        /// </summary>
        SCARD_ATTR_AnswerToResetString = SCARD_ATTR_AtrString,

        /// <summary>
        /// Single byte indicating smart card type (SCARD_ATTR_ICC_TYPE_PER_ATR)
        /// </summary>
        SCARD_ATTR_SCARD_ATTR_ICCTypePerAtr = 0x002400304,

        /// <summary>
        /// Esc reset (SCARD_ATTR_ESC_RESET)
        /// </summary>
        SCARD_ATTR_EscReset = 0x001C0A000, // (VendorDefined << 16)1C00000(7)

        /// <summary>
        /// Esc cancel (SCARD_ATTR_ESC_CANCEL)
        /// </summary>
        SCARD_ATTR_EscCancel = 0x001C0A0003,

        /// <summary>
        /// Esc authentication request (SCARD_ATTR_ESC_AUTHREQUEST)
        /// </summary>
        SCARD_ATTR_EscAuthRequest = 0x001C0A0005,

        /// <summary>
        /// Maximum input (SCARD_ATTR_MAXINPUT)
        /// </summary>
        SCARD_ATTR_MaxInput = 0x001C0A0007,

        /// <summary>
        /// Instance of this vendor's reader attached to the computer. The first instance will be device unit 0, the next will be unit 1 (if it is the same brand of reader) and so on. Two different brands of readers will both have zero for this value. (SCARD_ATTR_DEVICE_UNIT)
        /// </summary>
        //SCARD_ATTR_DeviceUnit = 0x1FFFC00001, //(System << 16)1FFFC00000(0x7fff)

        /// <summary>
        /// Reserved for future use. (SCARD_ATTR_DEVICE_IN_USE)
        /// </summary>
        //SCARD_ATTR_DeviceInUse = 0x1FFFC00002,

        /// <summary>
        /// Device friendly name ASCII (SCARD_ATTR_DEVICE_FRIENDLY_NAME_A)
        /// </summary>
        //SCARD_ATTR_DeviceFriendlyNameA = 0x1FFFC000013,

        /// <summary>
        /// Device system name ASCII (SCARD_ATTR_DEVICE_SYSTEM_NAME_A)
        /// </summary>
        //SCARD_ATTR_DeviceSystemNameA = 0x1FFFC00004,

        /// <summary>
        /// Device friendly name UNICODE (SCARD_ATTR_DEVICE_FRIENDLY_NAME_W)
        /// </summary>
        //SCARD_ATTR_DeviceFriendlyNameW = 0x1FFFC00005,

        /// <summary>
        /// Device system name UNICODE (SCARD_ATTR_DEVICE_SYSTEM_NAME_W)
        /// </summary>
        //SCARD_ATTR_DeviceSystemNameW = 0x1FFFC00006,

        /// <summary>
        /// Supress T1 information file size request (SCARD_ATTR_SUPRESS_T1_IFS_REQUEST)
        /// </summary>
        //SCARD_ATTR_SupressT1InformationFileSizeRequest = 0x1FFFC00007,

        /// <summary>
        /// Device friendly name (SCARD_ATTR_DEVICE_FRIENDLY_NAME)
        /// </summary>
        //SCARD_ATTR_DeviceFriendlyName = DeviceFriendlyNameW,

        /// <summary>
        /// Device system name (SCARD_ATTR_DEVICE_SYSTEM_NAME)
        /// </summary>
        //SCARD_ATTR_DeviceSystemName = DeviceSystemNameW
    }
}

