using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SnusProjekat
{
    public class Alarm : IAlarm
    {
        public delegate void MessageReceived(string message);
        public static MessageReceived onAlarmMessageReceived;

        public void ConsoleSubInit()
        {
            onAlarmMessageReceived += OperationContext.Current.GetCallbackChannel<IAlarmCallback>().WriteToConsole;
            //onAlarmMessageReceived += OperationContext.Current.GetCallbackChannel<ITrendingCallback>().WriteToAlarmFile;
        }
    }
}
