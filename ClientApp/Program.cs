using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ClientApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "igorn";
            NetTcpBinding binding = new NetTcpBinding();
            

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
           

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/DatabaseManagement"),
                                      new X509CertificateEndpointIdentity(srvCert));
           


            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                proxy.TestCommunication();
                Console.WriteLine("Test com");
                Consumer cons =  proxy.FindMaxInRegion("Vojvodina");
                Console.WriteLine(cons);
                Console.WriteLine(WindowsIdentity.GetCurrent().Name);
                proxy.CreateDatabase();
                List<int> lista = new List<int>() { 1, 23, 12, 12, 12, 41, 123, 4, 12, 32, 123, 4 };
                Consumer novi = new Consumer("Vojvodina", "Kikinda", 2013, lista);
                proxy.Write(44, novi);

               
                //Consumer cons =  proxy.FindMaxInRegion("Vojvodina");
               // Console.WriteLine(cons);
               // float value = proxy.MeanValByCity("Subotica");
               // Console.WriteLine(value);
                Console.WriteLine(srvCert.SubjectName.Name.ToString());
                Console.WriteLine(proxy.Role);
              
               
               // proxy.CreateDatabase();
                Console.ReadKey();

            }

            Console.ReadLine();


        }
    }
}
