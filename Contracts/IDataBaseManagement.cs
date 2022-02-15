using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Contracts
{
    [ServiceContract]
    public interface IDataBaseManagement
    {
        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        void CreateDatabase();

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        bool ArchiveDatabase();

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        void DeleteDatabase();

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        bool Write(int id, Consumer consumer);

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        bool Modify(int id, Consumer consumer);

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        float MeanValByCity(string city);

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        float MeanValByRegion(string region);

        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        Consumer FindMaxInRegion(string region);

        [OperationContract]
        void TestCommunication();

       
    }
}
