using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SnusProjekat
{
    [ServiceContract]
    public interface IDatabaseManager
    {
        [OperationContract]
        void addUser(string name, string pass,bool ad,bool isAdmin);

        [OperationContract]
        User login(string name, string pass);

        [OperationContract]
        void logout();

        [OperationContract]
        void inputDI(DI tag);

        [OperationContract]
        void inputDO(DO tag);

        [OperationContract]
        void inputAO(AO tag);

        [OperationContract]
        void inputAI(AI tag);

        [OperationContract]
        bool InputValuesDO(string id, string value);

        [OperationContract]
        bool InputValuesAO(string id, string value);

        [OperationContract]
        void onOffAITag(string id);

        [OperationContract]
        void onOffDITag(string id);

        [OperationContract]
        void deleteAITag(string id);

        [OperationContract]
        void deleteDITag(string id);

        [OperationContract]
        void deleteAOTag(string id);

        [OperationContract]
        void deleteDOTag(string id);

        [OperationContract]
        string displayTagValueAO(string id);
        [OperationContract]
        string displayTagValueAI(string id);
        [OperationContract]
        string displayTagValueDO(string id);
        [OperationContract]
        string displayTagValueDI(string id);

        [OperationContract]
        void turningOnScaning(string tagName, string tagId);

        [OperationContract]
        void turningOffScaning(string tagName, string tagId);

        [OperationContract]
        void readConfig();

        [OperationContract]
        void printToConfigAI(AI tag);
        [OperationContract]
        void printToConfigDI(DI tag);
        [OperationContract]
        void printToConfigAO(AO tag);
        [OperationContract]
        void printToConfigDO(DO tag);

        [OperationContract]
        void addAlarm(string type, int priority, double value, string tagName);

        [OperationContract]
        void deleteAlarmFromFile(string tagId, string type, string value);
    }
}

