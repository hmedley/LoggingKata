using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using Geolocation;

namespace LoggingKata
{
    class Program
    {
        //Why do you think we use ILog?

        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory + "//Taco_Bell-US-AL-Alabama.csv";

            if (path.Length == 0)
            {
                Console.WriteLine("You must provide a filename as an argument");
                Logger.Fatal("Cannot import without filename specified in our path variable");
                return;
            }

            Logger.Info("Log initialized");
            Logger.Info("Grabbing from path" + path);

            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                Logger.Error("No locations to check for");
            }
            else if (lines.Length == 1)
            {
                Logger.Warn("Only one location provided");
            }
            var parser = new TacoParser();
            Logger.Debug("Initialized our Parser");

            var locations = lines.Select(line => parser.Parse(line))
                .OrderBy(loc => loc.Location.Latitude)
                .ThenBy(loc => loc.Location.Longitude)
                .ToArray();

            //TODO:  Find the two TacoBells in Alabama that are the furthurest from one another.
            ITrackable a = null;
            ITrackable b = null;
            double distance = 0;
            foreach (var locA in locations)
            {
                var origin = new Coordinate
                {
                    Latitude = locA.Location.Latitude,
                    Longitude = locA.Location.Longitude
                };
                foreach (var locB in locations)
                {
                    var dest = new Coordinate
                    {
                        Latitude = locB.Location.Latitude,
                        Longitude = locB.Location.Longitude
                    };

                    var nDist = GeoCalculator.GetDistance(origin, dest);

                    if (nDist > distance)
                    {
                        a = locA;
                        b = locB;
                        distance = nDist;
                    }
                }
            }

            if (a == null || b == null)
            {
                Logger.Error("Failed to find furthest locations");
                Console.WriteLine("Couldn't find the distance between furthest locations");
                Console.ReadLine();
            }

            Console.WriteLine($"the two taco bell's that are farthest apart are {a.Name} and : {b.Name}");
            Console.WriteLine($"These two locations are {distance} miles away");
            Console.ReadLine();
        }
    }
}