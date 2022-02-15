using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Manager;
using System.ServiceModel;
using System.Text;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ServiceModel.Description;

namespace ServiceApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            NetTcpBinding binding2 = new NetTcpBinding();
             

            ServiceHost host = new ServiceHost(typeof(DatabaseManagement));


            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding2.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;



            string address = "net.tcp://localhost:9999/DatabaseManagement";
            string address2 = "net.tcp://localhost:9998/IReplicate";

            host.AddServiceEndpoint(typeof(IDataBaseManagement), binding, address);
            host.AddServiceEndpoint(typeof(IReplicate), binding2, address2);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // dpodesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            //  host.Authorization.ServiceAuthorizationManager = new MyAuthorizationManager();

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);
            try
            {
                host.Open();
                Console.WriteLine(Formatter.ParseName(WindowsIdentity.GetCurrent().Name));
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                Console.WriteLine(host.Credentials.ServiceCertificate.Certificate.SubjectName.Name);
                Console.WriteLine("Ovde pises");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
               
            }


        }
        
    }
}
