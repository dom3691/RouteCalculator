
1. Route Class:- Represents single route in the network, stores the starting location (From), destination(To) and the distance between them 
2. RouteParser class:- Parses users input in the formart "AB5, BC4, CD8", where A and B are locations, 5 is the distance. Ensures valid routes 
	by skipping invalid or malformed input, prevents duplicat routes
3. RouteCalculatorService Class: Implements the core logic for route calculations. It uses a dictionary (_routeMap) to store routes efficiently.
METHODS IMPLEMENTED:
-CalculateRouteDistance(string[] path):- Computes the distance of a given route (e.g., A-B-C), Returns null if the route does not exist.
-CountRoutesWithMaxStops(string start, string end, int maxStops):- Counts the number of routes between two locations with at most maxStops.
-CountRoutesWithExactStops(string start, string end, int exactStops):- Counts the number of routes between two locations with exactly exactStops.
-FindShortestRouteDistance(string start, string end):- Uses Dijkstra's Algorithm to find the shortest path between two points, If start == end, it finds the shortest circular route.
-CountRoutesWithMaxDistance(string start, string end, int maxDistance):- Counts routes where the total distance is less than maxDistance.

4. IRouteCalculatorService Interface: Defines the contract for RouteCalculatorService, ensuring testability and maintainability.
5. Program Class
-Provides a simple console-based UI.
-Prompts users to enter routes in the format "AB5, BC4, CD8".
-Executes predefined queries such as:
-Finding route distances.
-Counting routes with specific stops.
-Computing shortest paths.
-Counting routes within a max distance.

ASSUMPTIONS:
1. Graph Representation: Routes are directed, meaning AB5 does not imply BA5. No duplicate routes exist in the input.
2. Route Validations: Only valid routes are added (i.e., distances must be positive). If a route does not exist, NO SUCH ROUTE is displayed.

Dijkstra’s Algorithm is used for shortest path calculation.


TO RUN:
1- You need VS Code(for mac, windows and Linux) or Visual Studio(for windows)
2. the Project runs on .Net 8, so install .Net 8 SDK