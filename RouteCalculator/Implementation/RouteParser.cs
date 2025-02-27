using RouteCalculator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteCalculator.Implementation
{
    public class RouteParser
    {
        public static List<Route> ParseRoutes(string input)
        {
            var routes = new List<Route>();
            var routeSet = new HashSet<string>(); // To track duplicates

            if (string.IsNullOrWhiteSpace(input))
                return routes;

            var routeStrings = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var routeString in routeStrings)
            {
                var trimmed = routeString.Trim();
                if (trimmed.Length < 3)
                    continue;

                var from = trimmed[0].ToString();
                var to = trimmed[1].ToString();

                // Check for duplicates
                string routeKey = $"{from}{to}";
                if (routeSet.Contains(routeKey))
                    continue;

                routeSet.Add(routeKey);

                if (int.TryParse(trimmed.Substring(2), out int distance))
                {
                    if (distance <= 0)
                    {
                        // Either skip or throw an exception for negative distances
                        continue;
                    }
                    routes.Add(new Route(from, to, distance));
                }
            }

            return routes;
        }
    }
}
