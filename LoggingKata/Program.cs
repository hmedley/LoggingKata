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
            if (args.Length == 0)
            {
                Console.WriteLine("You must provide a filename as an argument");
                Logger.Fatal("Cannot import without filename specified in our path variable");
                return;
            }

            Logger.Info("Log initialized");
            Logger.Info("Grabbing from path" + path);
            var lines = File.ReadAllLines(args[0]);
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

            var locations = lines.Select(line => parser.Parse(line));

            //TODO:  Find the two TacoBells in Alabama that are the furthurest from one another.
            //HINT:  You'll need two nested forloops

        }
    }
}