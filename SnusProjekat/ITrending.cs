using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SnusProjekat
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrending" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(ITrendingCallback))]
    public interface ITrending
    {
        [OperationContract(IsOneWay = true)]
        void ConsoleTrendingInit();
    }

    public interface ITrendingCallback
    {
        [OperationContract]
        void WriteToConsole(string message);

        [OperationContract]
        void WriteToLogFile(string message);

        [OperationContract]
        void WriteToAlarmFile(string message);
    }
}
