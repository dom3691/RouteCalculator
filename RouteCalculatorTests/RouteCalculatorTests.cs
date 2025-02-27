using System;
using System.Collections.Generic;
using Xunit;
using RouteCalculator;
using RouteCalculator.Domain;
using RouteCalculator.Implementation;
using RouteCalculator.Interfaces;

namespace RouteCalculator.Tests
{
    public class RouteCalculatorTests
    {
        private readonly IRouteCalculatorService _calculator;

        public RouteCalculatorTests()
        {
            // Setup test data: AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
            var routes = new List<Route>
            {
                new Route("A", "B", 5),
                new Route("B", "C", 4),
                new Route("C", "D", 8),
                new Route("D", "C", 8),
                new Route("D", "E", 6),
                new Route("A", "D", 5),
                new Route("C", "E", 2),
                new Route("E", "B", 3),
                new Route("A", "E", 7)
            };

            _calculator = new RouteCalculatorService(routes);
        }

        [Fact]
        public void RouteParser_ParseRoutes_ShouldParseValidInput()
        {
            // Arrange
            string input = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";

            // Act
            var routes = RouteParser.ParseRoutes(input);

            // Assert
            Assert.Equal(9, routes.Count);
            Assert.Equal("A", routes[0].From);
            Assert.Equal("B", routes[0].To);
            Assert.Equal(5, routes[0].Distance);
        }

        [Fact]
        public void CalculateRouteDistance_ShouldCalculateCorrectDistanceForValidRoute()
        {
            // Question 1: The distance of the route A-B-C
            var distance = _calculator.CalculateRouteDistance(new[] { "A", "B", "C" });
            Assert.Equal(9, distance);
        }

        [Fact]
        public void CalculateRouteDistance_ShouldCalculateCorrectDistanceForLongerRoute()
        {
            // Question 2: The distance of the route A-E-B-C-D
            var distance = _calculator.CalculateRouteDistance(new[] { "A", "E", "B", "C", "D" });
            Assert.Equal(22, distance);
        }

        [Fact]
        public void CalculateRouteDistance_ShouldReturnNullForInvalidRoute()
        {
            // Question 3: The distance of the route A-E-D
            var distance = _calculator.CalculateRouteDistance(new[] { "A", "E", "D" });
            Assert.Null(distance);
        }

        [Fact]
        public void CountRoutesWithMaxStops_ShouldReturnCorrectCount()
        {
            // Question 4: The number of trips starting at C and ending at C with a maximum of 3 stops
            var count = _calculator.CountRoutesWithMaxStops("C", "C", 3);
            Assert.Equal(2, count);
        }

        [Fact]
        public void CountRoutesWithExactStops_ShouldReturnCorrectCount()
        {
            // Question 5: The number of trips starting at A and ending at C with exactly 4 stops
            var count = _calculator.CountRoutesWithExactStops("A", "C", 4);
            Assert.Equal(3, count);
        }

        [Fact]
        public void FindShortestRouteDistance_ShouldReturnCorrectDistance()
        {
            // Question 6: The length of the shortest route from A to C
            var shortest = _calculator.FindShortestRouteDistance("A", "C");
            Assert.Equal(9, shortest);
        }

        [Fact]
        public void FindShortestRouteDistance_ShouldWorkForCircularRoutes()
        {
            // Question 7: The length of the shortest route from B to B
            var shortest = _calculator.FindShortestRouteDistance("B", "B");
            Assert.Equal(9, shortest);
        }

        [Fact]
        public void CountRoutesWithMaxDistance_ShouldReturnCorrectCount()
        {
            // Question 8: The number of different routes from C to C with a distance of less than 30
            var count = _calculator.CountRoutesWithMaxDistance("C", "C", 30);
            Assert.Equal(7, count);
        }

        [Fact]
        public void CalculateRouteDistance_ShouldHandleEmptyPath()
        {
            var distance = _calculator.CalculateRouteDistance(new string[] { });
            Assert.Null(distance);
        }

        [Fact]
        public void CalculateRouteDistance_ShouldHandleSingleStopPath()
        {
            var distance = _calculator.CalculateRouteDistance(new[] { "A" });
            Assert.Null(distance);
        }

        [Fact]
        public void CountRoutesWithMaxStops_ShouldReturnZeroWhenNoRouteExists()
        {
            var count = _calculator.CountRoutesWithMaxStops("X", "Y", 3);
            Assert.Equal(0, count);
        }

        [Fact]
        public void CountRoutesWithExactStops_ShouldReturnZeroWhenNoRouteExists()
        {
            var count = _calculator.CountRoutesWithExactStops("X", "Y", 4);
            Assert.Equal(0, count);
        }

        [Fact]
        public void FindShortestRouteDistance_ShouldReturnNegativeOneWhenNoRouteExists()
        {
            var shortest = _calculator.FindShortestRouteDistance("X", "Y");
            Assert.Equal(-1, shortest);
        }

        [Fact]
        public void CountRoutesWithMaxDistance_ShouldReturnZeroWhenNoRouteExists()
        {
            var count = _calculator.CountRoutesWithMaxDistance("X", "Y", 30);
            Assert.Equal(0, count);
        }
    }
}