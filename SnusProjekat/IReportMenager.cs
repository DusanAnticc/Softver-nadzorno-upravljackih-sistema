using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SnusProjekat
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReportMenager" in both code and config file together.
    [ServiceContract]
    public interface IReportMenager
    {
        [OperationContract]
        string alarmsInInterval(DateTime date1, DateTime date2);

        [OperationContract]
        string alarmsWithPriority(int priority);

        [OperationContract]
        string tagsInInterval(DateTime date1, DateTime date2);

        [OperationContract]
        string dItagsInInterval(DateTime date1, DateTime date2);

        [OperationContract]
        string aItagsInInterval(DateTime date1, DateTime date2);
    }
}
