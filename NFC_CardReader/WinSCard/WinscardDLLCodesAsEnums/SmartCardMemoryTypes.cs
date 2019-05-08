using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The posible memory types that SmartCards could possess
    /// </summary>
    public enum SmartCardMemoryTypes
    {
        /// <summary>
        /// MCU Memory Type
        /// </summary>
        CT_MCU = 0x00,
        /// <summary>
        /// IIC (Auto Detect Memory Size)
        /// </summary>
        CT_IIC_Auto = 0x01,
        /// <summary>
        /// IIC (1K) Memory Type
        /// </summary>
        CT_IIC_1K = 0x02,
        /// <summary>
        /// IIC (2K) Memory Type
        /// </summary>
        CT_IIC_2K = 0x03,
        /// <summary>
        /// IIC (4K) Memory Type
        /// </summary>
        CT_IIC_4K = 0x04,
        /// <summary>
        /// IIC (8K) Memory Type
        /// </summary>
        CT_IIC_8K = 0x05,
        /// <summary>
        /// IIC (16K) Memory Type
        /// </summary>
        CT_IIC_16K = 0x06,
        /// <summary>
        /// IIC (32K) Memory Type
        /// </summary>
        CT_IIC_32K = 0x07,
        /// <summary>
        /// IIC (32K) Memory Type
        /// </summary>
        CT_IIC_64K = 0x08,
        /// <summary>
        /// IIC (64K) Memory Type
        /// </summary>
        CT_IIC_128K = 0x09,
        /// <summary>
        /// IIC (128K) Memory Type
        /// </summary>
        CT_IIC_256K = 0x0A,
        /// <summary>
        /// IIC (256K) Memory Type
        /// </summary>
        CT_IIC_512K = 0x0B,
        /// <summary>
        /// IIC (512K) Memory Type
        /// </summary>
        CT_IIC_1024K = 0x0C,
        /// <summary>
        /// IIC (1024K) Memory Type
        /// </summary>
        CT_AT88SC153 = 0x0D,
        /// <summary>
        /// AT88SC153 Memory Type
        /// </summary>
        CT_AT88SC1608 = 0x0E,
        /// <summary>
        /// AT88SC1608 Memory Type
        /// </summary>
        CT_SLE4418 = 0x0F,
        /// <summary>
        /// SLE4418 Memory Type
        /// </summary>
        CT_SLE4428 = 0x10,
        /// <summary>
        /// SLE4428 Memory Type
        /// </summary>
        CT_SLE4432 = 0x11,
        /// <summary>
        /// SLE4442 Memory Type
        /// </summary>
        CT_SLE4442 = 0x12,
        /// <summary>
        /// SLE4406 Memory Type
        /// </summary>
        CT_SLE4406 = 0x13,
        /// <summary>
        /// SLE4436 Memory Type
        /// </summary>
        CT_SLE4436 = 0x14,
        /// <summary>
        /// SLE5536 Memory Type
        /// </summary>
        CT_SLE5536 = 0x15,
        /// <summary>
        /// MCU T=0 Memory Type
        /// </summary>
        CT_MCUT0 = 0x16,
        /// <summary>
        /// MCU T=1 Memory Type
        /// </summary>
        CT_MCUT1 = 0x17,
        /// <summary>
        /// MCU Autodetect
        /// </summary>
        CT_MCU_Auto = 0x18,
    }
}
