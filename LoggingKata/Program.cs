using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using System.Threading;
using System.Linq.Expressions;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
                       
            logger.LogInfo("Log initialized");
           
            var lines = File.ReadAllLines(csvPath);
            if(lines.Length == 0)
            {
                logger.LogError("There is no input");
            }
            if(lines.Length == 1)
            {
                logger.LogWarning("Warning, needs more data");
            }

            logger.LogInfo($"Lines: {lines[0]}");
           
            var parser = new TacoParser();
           
            var locations = lines.Select(parser.Parse).ToArray();

            
            ITrackable start = null;
            ITrackable finish = null;
            double farthestDistance = 0;
                                                                                                        
            foreach (var location in locations)
            {
                var locA = location.Location;
                var corA = new GeoCoordinate(locA.Latitude, locA.Longitude);

                foreach (var destination in locations)
                {
                    var locB = destination.Location;
                    var corB = new GeoCoordinate(locB.Latitude, locB.Longitude);
                    var distance = corA.GetDistanceTo(corB);
                    if (distance > farthestDistance)
                    {
                        start = location;
                        finish = destination;
                        farthestDistance = distance;
                        logger.LogInfo($"Distance from {location.Name} to {destination.Name} is {distance}");
                    }
                }
            }


            logger.LogInfo($"The farthest distance is {farthestDistance} between {start.Name} and {finish.Name}");



        }
    }
}
