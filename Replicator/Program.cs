using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            NetTcpBinding binding2 = new NetTcpBinding();
            byte[] cryptedData = null;

            try
            {
                ChannelFactory<IReplicate> cfService = new ChannelFactory<IReplicate>(binding, "net.tcp://localhost:9998/IReplicate");
                ChannelFactory<IReplicate> cfReplicator = new ChannelFactory<IReplicate>(binding2, "net.tcp://localhost:9998/IReplicate");
                IReplicate kService = cfService.CreateChannel();
                IReplicate kReplicator = cfReplicator.CreateChannel();
               
                Console.WriteLine(WindowsIdentity.GetCurrent().Name);

               byte[] data =  kService.ReadCryptedData();
                

                //cryptedData = kService.ReadCryptedData();
                kReplicator.WriteDecryptedData(data);


            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
