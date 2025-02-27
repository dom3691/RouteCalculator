using RouteCalculator.Domain;
using RouteCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteCalculator.Implementation
{
    public class RouteCalculatorService : IRouteCalculatorService
    {
        private readonly Dictionary<string, List<Route>> _routeMap;

        public RouteCalculatorService(IEnumerable<Route> routes)
        {
            _routeMap = new Dictionary<string, List<Route>>();

            foreach (var route in routes)
            {
                if (!_routeMap.ContainsKey(route.From))
                {
                    _routeMap[route.From] = new List<Route>();
                }

                _routeMap[route.From].Add(route);
            }
        }

        public int? CalculateRouteDistance(string[] path)
        {
            if (path == null || path.Length <= 1)
                return null;

            int totalDistance = 0;

            for (int i = 0; i < path.Length - 1; i++)
            {
                string from = path[i];
                string to = path[i + 1];

                // Check if route exists
                if (!_routeMap.ContainsKey(from))
                    return null;

                var route = _routeMap[from].FirstOrDefault(r => r.To == to);
                if (route == null)
                    return null;

                totalDistance += route.Distance;
            }

            return totalDistance;
        }



        public int CountRoutesWithMaxStops(string start, string end, int maxStops)
        {
            int count = 0;
            CountRoutesWithMaxStopsRecursive(start, end, 0, maxStops, ref count);
            return count;
        }

        private void CountRoutesWithMaxStopsRecursive(string current, string end, int currentStops, int maxStops, ref int count)
        {
            if (currentStops > maxStops)
                return;

            if (currentStops > 0 && current == end)
                count++;

            if (!_routeMap.ContainsKey(current))
                return;

            foreach (var route in _routeMap[current])
            {
                CountRoutesWithMaxStopsRecursive(route.To, end, currentStops + 1, maxStops, ref count);
            }
        }


        public int CountRoutesWithExactStops(string start, string end, int exactStops)
        {
            int count = 0;
            CountRoutesWithExactStopsRecursive(start, end, 0, exactStops, ref count);
            return count;
        }


        private void CountRoutesWithExactStopsRecursive(string current, string end, int currentStops, int exactStops, ref int count)
        {
            if (currentStops > exactStops)
                return;

            if (currentStops == exactStops && current == end)
                count++;

            if (currentStops >= exactStops)
                return;

            if (!_routeMap.ContainsKey(current))
                return;

            foreach (var route in _routeMap[current])
            {
                CountRoutesWithExactStopsRecursive(route.To, end, currentStops + 1, exactStops, ref count);
            }
        }

        public int FindShortestRouteDistance(string start, string end)
        {
            // First check if both start and end nodes exist in our graph
            bool startExists = _routeMap.Keys.Contains(start) || _routeMap.Values.Any(routes => routes.Any(r => r.To == start));
            bool endExists = _routeMap.Keys.Contains(end) || _routeMap.Values.Any(routes => routes.Any(r => r.To == end));

            // If either node doesn't exist, return -1 immediately
            if (!startExists || !endExists)
            {
                return -1;
            }

            var shortestDistances = new Dictionary<string, int>();
            var visited = new HashSet<string>();

            // Use a list to track nodes by distance
            var priorityQueue = new List<(int distance, string node)>();

            // Initialize distances only for nodes that actually exist
            foreach (var node in _routeMap.Keys.Union(_routeMap.Values.SelectMany(v => v.Select(r => r.To))))
            {
                shortestDistances[node] = int.MaxValue;
            }

            // Special case for circular routes (start == end)
            if (start == end)
            {
                return FindShortestCircularRouteDistance(start);
            }

            shortestDistances[start] = 0;
            priorityQueue.Add((0, start));

            while (priorityQueue.Count > 0)
            {
                // Find the node with minimum distance
                priorityQueue.Sort((a, b) => a.distance.CompareTo(b.distance));
                var (currentDistance, current) = priorityQueue[0];
                priorityQueue.RemoveAt(0);

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (current == end)
                    return currentDistance;

                if (!_routeMap.ContainsKey(current))
                    continue;

                foreach (var route in _routeMap[current])
                {
                    var newDistance = currentDistance + route.Distance;

                    if (newDistance < shortestDistances[route.To])
                    {
                        shortestDistances[route.To] = newDistance;
                        priorityQueue.Add((newDistance, route.To));
                    }
                }
            }

            // If we've checked all reachable nodes and haven't found the end, return -1
            return -1;
        }

        private int FindShortestCircularRouteDistance(string node)
        {
            if (!_routeMap.ContainsKey(node))
                return -1;

            int shortestDistance = int.MaxValue;

            foreach (var firstHop in _routeMap[node])
            {
                // Find shortest path from firstHop.To back to node
                var distance = firstHop.Distance;

                var tempRoutes = new List<Route>(_routeMap.SelectMany(kv => kv.Value));
                var tempCalculator = new RouteCalculatorService(tempRoutes);

                var returnDistance = tempCalculator.FindShortestRouteDistance(firstHop.To, node);

                if (returnDistance != -1 && distance + returnDistance < shortestDistance)
                {
                    shortestDistance = distance + returnDistance;
                }
            }

            return shortestDistance == int.MaxValue ? -1 : shortestDistance;
        }



        public int CountRoutesWithMaxDistance(string start, string end, int maxDistance)
        {
            int count = 0;
            CountRoutesWithMaxDistanceRecursive(start, end, 0, maxDistance, ref count);
            return count;
        }

        private void CountRoutesWithMaxDistanceRecursive(string current, string end, int currentDistance, int maxDistance, ref int count)
        {
            if (currentDistance >= maxDistance)
                return;

            if (currentDistance > 0 && current == end)
                count++;

            if (!_routeMap.ContainsKey(current))
                return;

            foreach (var route in _routeMap[current])
            {
                int newDistance = currentDistance + route.Distance;
                if (newDistance < maxDistance)
                {
                    CountRoutesWithMaxDistanceRecursive(route.To, end, newDistance, maxDistance, ref count);
                }
            }
        }
    }

}
