using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// A C# friendly outside cover for SCARD_READERSTATE
    /// </summary>
    public struct ReadersCurrentState
    {
        public string ReaderName;
        public int UserData;
        public SmartCardStates CurrentState;
        public SmartCardStates EventState;
        public byte[] ATR;
    }

    /// <summary>
    /// A class to help with the quick transfer from the Cpp import to C# wrapper class
    /// contains callable extention fuctions for their respective arrays
    /// </summary>
    static internal class StateHelpers
    {
        /// <summary>
        /// ReadersCurrentState Reader to SCARD_READERSTATE
        /// </summary>
        internal static ReadersCurrentState[] ToReadersCurrentState(this SCARD_READERSTATE[] In)
        {
            ReadersCurrentState[] Return = new ReadersCurrentState[In.Count()];
            for (int i = 0; i < In.Length; i++)
            {
                Return[i] = new ReadersCurrentState();
                Return[i].ReaderName = In[i].szReader;
                Return[i].CurrentState = (SmartCardStates)In[i].dwCurrentState;
                Return[i].EventState = (SmartCardStates)In[i].dwEventState;
                Return[i].UserData = In[i].pvUserData;
                Return[i].ATR = new byte[In[i].cbAtr];
                Array.Copy(In[i].rgbAtr, Return[i].ATR, In[i].cbAtr);
            }
            return Return;
        }

        /// <summary>
        /// SCARD_READERSTATE Reader to ReadersCurrentState
        /// </summary>
        internal static SCARD_READERSTATE[] ToSCARD_READERSTATE(this ReadersCurrentState[] In)
        {
            SCARD_READERSTATE[] Return = new SCARD_READERSTATE[In.Length];
            for (int i = 0; i < In.Length; i++)
            {
                Return[i] = new SCARD_READERSTATE();
                Return[i].szReader = In[i].ReaderName;
                Return[i].dwCurrentState = (int)In[i].CurrentState;
                Return[i].dwEventState = (int)In[i].EventState;
                Return[i].pvUserData = In[i].UserData;
                if (In[i].ATR != null && Return[i].rgbAtr != null)
                    Array.Copy(In[i].ATR, Return[i].rgbAtr, In[i].ATR.Length);
            }
            return Return;
        }
    }
}
