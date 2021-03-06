﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader.ACR122UManager
{
    public class ACRCardAcceptedCardScanEventArg : EventArgs
    {
        public string ReaderName { get; protected set; }
        public int UserData { get; protected set; }
        public SmartCardStates CurrentState { get; protected set; }
        public SmartCardStates EventState { get; protected set; }
        public byte[] ATR { get; protected set; }

        public string ATRString { get => BitConverter.ToString(ATR); }

        public ACR122UManager EventsACR122UManager { get; protected set; }

        internal ACRCardAcceptedCardScanEventArg(ACR122UManager EventsReader, ACRCardStateChangeEventArg State)
        {
            EventsACR122UManager = EventsReader;

            ReaderName = string.Copy(State.ReaderName);
            UserData = State.UserData;
            CurrentState = State.CurrentState;
            EventState = State.EventState;
            ATR = (byte[])State.ATR?.Clone();
        }
    }
}
