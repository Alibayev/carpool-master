using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarPoolDemo.Models
{
    public class Trip
    {
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public int departureTime { get; set; }
        public string departureStringTime { get; set; }
        public string pickupTime { get; set; }
        public int distance { get; set; }
        public int durationTime { get; set; }
        public int trafficTime { get; set; }
        public int timeDifference { get; set; }
        public Trip(string fromAddress, string toAddress, string time, string distance, string duration, string trafficTime)
        {
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            DateTime myTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm",
            System.Globalization.CultureInfo.InvariantCulture).AddHours(6);
            TimeSpan span = (myTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            this.departureTime = (int)span.TotalSeconds;
            this.distance= Int32.Parse(distance);
            this.durationTime = Int32.Parse(duration);
            this.trafficTime = Int32.Parse(trafficTime);
            this.departureStringTime = time;
        }
        public Trip(string fromAddress, string toAddress, string time)
        {
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            DateTime myTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm",
            System.Globalization.CultureInfo.InvariantCulture).AddHours(6);
            TimeSpan span = (myTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            this.departureTime = (int)span.TotalSeconds;
            
        }


        public Trip()
        {

        }
    }
}