using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using log4net;

namespace LoggingKata
{
    /// <summary>
    /// Parses a POI file to locate all the TacoBells
    /// </summary>
    public class TacoParser
    {
        public TacoParser()
        {

        }

        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ITrackable Parse(string line)
        {
            var listOfTacoBell = line.Split(',');
            if (listOfTacoBell.Length <= 3)
            {
                Logger.Error("Must have at least three items in the array");
                return null;
            }
            double lon = 0;
            double lat = 0;

            try
            {
                Logger.Debug("Attempt parsing longitude");
                lon = double.Parse(listOfTacoBell[0]);
                Logger.Debug("Attempt parsing latitude");
                lat = double.Parse(listOfTacoBell[1]);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to parse the location", e);
                Console.WriteLine(e);
                throw;
            }

            var tB = new TacoBell
            {
                Name = listOfTacoBell[2],
                Location = new Point
                {
                    Longitude = lon,
                    Latitude = lat
                }
            };

            return tB;
        }
    }
}