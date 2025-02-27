using RouteCalculator.Implementation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Teacher Computer Retrieval Route Calculator");
        Console.WriteLine("Please enter routes in the format: AB5, BC4, CD8, etc.");

        string input = Console.ReadLine();
        var routes = RouteParser.ParseRoutes(input);

        var calculator = new RouteCalculatorService(routes);

        // Question 1: The distance of the route A-B-C
        var distance1 = calculator.CalculateRouteDistance(new[] { "A", "B", "C" });
        Console.WriteLine($"1. {(distance1.HasValue ? distance1.ToString() : "NO SUCH ROUTE")}");

        // Question 2: The distance of the route A-E-B-C-D
        var distance2 = calculator.CalculateRouteDistance(new[] { "A", "E", "B", "C", "D" });
        Console.WriteLine($"2. {(distance2.HasValue ? distance2.ToString() : "NO SUCH ROUTE")}");

        // Question 3: The distance of the route A-E-D
        var distance3 = calculator.CalculateRouteDistance(new[] { "A", "E", "D" });
        Console.WriteLine($"3. {(distance3.HasValue ? distance3.ToString() : "NO SUCH ROUTE")}");

        // Question 4: The number of trips starting at C and ending at C with a maximum of 3 stops
        var count4 = calculator.CountRoutesWithMaxStops("C", "C", 3);
        Console.WriteLine($"4. {count4}");

        // Question 5: The number of trips starting at A and ending at C with exactly 4 stops
        var count5 = calculator.CountRoutesWithExactStops("A", "C", 4);
        Console.WriteLine($"5. {count5}");

        // Question 6: The length of the shortest route from A to C
        var shortest6 = calculator.FindShortestRouteDistance("A", "C");
        Console.WriteLine($"6. {shortest6}");

        // Question 7: The length of the shortest route from B to B
        var shortest7 = calculator.FindShortestRouteDistance("B", "B");
        Console.WriteLine($"7. {shortest7}");

        // Question 8: The number of different routes from C to C with a distance of less than 30
        var count8 = calculator.CountRoutesWithMaxDistance("C", "C", 30);
        Console.WriteLine($"8. {count8}");

        Console.ReadLine();
    }
}