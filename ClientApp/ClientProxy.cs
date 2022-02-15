using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ClientApp
{
    public class ClientProxy : ChannelFactory<IDataBaseManagement>, IDataBaseManagement, IDisposable
    {
        IDataBaseManagement factory;

        private string role;

        public string Role { get => role; set => role = value; }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            Role = CertManager.GetClientRole(this.Credentials.ClientCertificate.Certificate);
          
            factory = this.CreateChannel();
        }

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
                Console.WriteLine("{0}", WindowsIdentity.GetCurrent().Name);
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        public void CreateDatabase()
        {
            try
            {
                factory.CreateDatabase();
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Create DB : {0}", e.Detail.Message);
            }
        }

        public void DeleteDatabase()
        {
            try
            {
                factory.DeleteDatabase();
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Delete DB : {0}", e.Detail.Message);
            }
        }

        public Consumer FindMaxInRegion(string region)
        {
            Consumer cons = null;

            try
            {
                cons=factory.FindMaxInRegion(region);
               
            }
            catch(FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            return cons;
        }

        public float MeanValByCity(string city)
        {
            float result = 0;
            try
            {
                result = factory.MeanValByCity(city);

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            return result;
        }

        public float MeanValByRegion(string region)
        {
            float result = 0;
            try
            {
                result = factory.MeanValByRegion(region);

            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
            }
            return result;
        }

        public bool Modify(int id, Consumer consumer)
        {
            throw new NotImplementedException();
        }

        public bool ArchiveDatabase()
        {
            bool retVal = false;
            try
            {
                retVal = factory.ArchiveDatabase();
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Archive DB : {0}", e.Detail.Message);
            }
            return retVal;
        }

        public bool Write(int id, Consumer consumer)
        {
            bool retVal = false;
            try
            {
                retVal = factory.Write(id, consumer);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine("Error while trying to Write : {0}", e.Detail.Message);
            }
            return retVal;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
