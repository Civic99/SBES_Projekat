using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Contracts
{
    [DataContract]
    public class Consumer
    {
        int id;
        string region;
        string city;
        int year;
        List<int> amount;

        public Consumer()
        {
            Amount = new List<int>();
        }
        public Consumer(string region, string city, int year, List<int> amount)
        {

            
            this.Region = region;
            this.City = city;
            this.Year = year;
            Amount = new List<int>();
            Amount = amount;
            
        }

        [DataMember]
        public string Region { get => region; set => region = value; }
        [DataMember]
        public string City { get => city; set => city = value; }
        [DataMember]
        public int Year { get => year; set => year = value; }
        [DataMember]
        public List<int> Amount { get => amount; set => amount = value; }
        

        public override string ToString()
        {
            return string.Format("{0}, {1}", Region, City);
        }
    }
}
