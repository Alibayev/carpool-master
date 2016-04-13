using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using CarPoolDemo.Models;

namespace CarPoolDemo.Controllers
{
    

    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult check(string from, string to, string time)
        {
            string origin = from;
            string destination = to;
            //Console.WriteLine("origin: " + origin);
            //Console.WriteLine("destination: " + destination);
            
            
            ViewBag.Message = GetMap(from, to, time);
            ViewBag.from = from;
            ViewBag.to = to;
            ViewBag.time = time;
            Trip newTrip = new Trip(from,to,time);
           ViewBag.list = getResult(newTrip);

            return View();
        }

        private static LinkedList<Trip> readDb()
        {
            LinkedList<Trip> list = new LinkedList<Trip>();

            string[] lines = System.IO.File.ReadAllLines(@"G:\carpool-master\CarPoolDemo\db.txt");
            
            foreach (string line in lines)
            {
                string[] item = line.Split('\t');
                Trip trip = new Trip(item[0].Trim(), item[1].Trim(), item[2].Trim(), item[3].Trim(), item[4].Trim(), item[5].Trim());
                list.AddFirst(trip);
            }
            return list;
        }


        private static LinkedList<Trip> getResult(Trip newTrip)
        {
            LinkedList<Trip> listTrip = readDb();
            foreach(Trip trip in listTrip)
            {
                trip.timeDifference = compare(trip, newTrip);
            }
            return listTrip;
        }


        private static int compare(Trip trip1, Trip trip2)
        {
            int t1 = getTime(trip1.fromAddress, trip2.fromAddress, trip1.departureTime);
            int pickupTimeInt = trip1.departureTime + t1;
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddHours(-6);
            

            dateTime = dateTime.AddSeconds(pickupTimeInt);
            trip1.pickupTime = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            int t2=getTime(trip2.fromAddress, trip2.toAddress, trip1.departureTime+t1);
            int t3 = getTime(trip2.fromAddress, trip2.toAddress, trip1.departureTime + t1+t2);
            return t1+t2+t3 - trip1.trafficTime;
        }



        private static int getTime(string origin, string destination, int time)
        {
            int current = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (current > time)
            {
                return -1;
                //return;
            }

            string url = @"https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +
              "&destination=" + destination+ "&departure_time=" + time + "&key=AIzaSyBO9khwJBKM4II1pxZT89ItprSLiYj9eho";
            var client = new WebClient();
            var result = client.DownloadData(url);

            var json = Encoding.UTF8.GetString(result);
            dynamic dynObj = JsonConvert.DeserializeObject(json);
            var res = dynObj.routes[0].legs[0];
            string s = "Distance: " + res.distance.value + "             Duration: " + res.duration.value + "             Duration in traffic: " + res.duration_in_traffic.value;
            int trafficTime = res.duration_in_traffic.value;
            int durationTime = res.duration.value;
            int distance = res.distance.value;
            return trafficTime;
        }

        private static string GetMap(string origin, string destination, string time)
        {
            DateTime myTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm",
            System.Globalization.CultureInfo.InvariantCulture).AddHours(6);
            TimeSpan span = (myTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            int timestamp = (int)span.TotalSeconds;
            int current = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (current > timestamp)
            {
                return "this is past time. plz use future time";
                //return;
            }

            string url = @"https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +
                "&destination=" + destination + "&departure_time=" + timestamp + "&key=AIzaSyBO9khwJBKM4II1pxZT89ItprSLiYj9eho";
            var client = new WebClient();
            var result = client.DownloadData(url);

            var json = Encoding.UTF8.GetString(result);
            dynamic dynObj = JsonConvert.DeserializeObject(json);
            var res = dynObj.routes[0].legs[0];
            string s="Distance: " + res.distance.text+ "             Duration: " + res.duration.text + "             Duration in traffic: " + res.duration_in_traffic.text;
            return s;
        }


    }
}