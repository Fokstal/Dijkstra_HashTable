using System;
using System.Xml.XPath;

namespace Dijkstra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("\n\n");

            Dictionary<string, Dictionary<string, int>> graph = new()
            {
                { "start", new() {
                    { "1", 5 },
                    { "2", 8 }
                } },

                { "1", new() {
                    { "3", 12 },
                    { "5", 9 },
                } },

                { "2", new() {
                    { "4", 8 },
                    { "5", 4 },
                    { "end", 2 },
                } },

                { "3", new() {
                    { "4", 6 },
                    { "5", 3 },
                } },

                { "4", new() {
                    { "end", 7 },
                } },

                { "5", new() },

                { "end", new() },
            };

            try
            {
                UseDijkstra(graph);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("\n" + $"Exception: \n{ex}");
            }

            System.Console.WriteLine("\n\n");
        }

        private static void UseDijkstra(Dictionary<string, Dictionary<string, int>> graph)
        {
            Dictionary<string, string?> parents = GetParentsFromGraph(graph);
            Dictionary<string, int> costs = new();

            foreach (var vertex in parents.Keys)
            {
                costs.Add(vertex, int.MaxValue);

                if (parents[vertex] is not null)
                {
                    costs[vertex] = graph["start"][vertex];
                }
            }

            List<string> processedNode = new();
            string? node = FindLowestCostNode(costs, processedNode);


            while (node is not null)
            {
                int cost = costs[node];
                Dictionary<string, int> neighbors = graph[node];

                foreach (string vertex in neighbors.Keys)
                {
                    int newCost = cost + neighbors[vertex];

                    if (costs[vertex] > newCost)
                    {
                        costs[vertex] = newCost;
                        parents[vertex] = node;
                    }
                }

                processedNode.Add(node);
                node = FindLowestCostNode(costs, processedNode);
            }

            foreach (var parent in parents.Keys)
            {
                Console.WriteLine($"From {parent} to {parents[parent]}");
            }
        }

        private static string? FindLowestCostNode(Dictionary<string, int> costs, List<string> processedNode)
        {
            string? lowestNode = null;
            int lowestCost = Int32.MaxValue;

            foreach (string key in costs.Keys)
            {
                if (costs[key] < lowestCost && processedNode.Contains(key) is false)
                {
                    lowestNode = key;
                    lowestCost = costs[key];
                }
            }

            return lowestNode;
        }

        private static Dictionary<string, string?> GetParentsFromGraph(Dictionary<string, Dictionary<string, int>> graph)
        {
            Dictionary<string, string?> parents = new();

            foreach (var vertex in graph["start"].Keys)
            {
                parents.Add(vertex, "start");
            }

            foreach (var vertex in graph.Keys)
            {
                if (parents.Keys.Contains(vertex) is false)
                {
                    parents.Add(vertex, null);
                }
            }

            parents.Remove("start");

            return parents;
        }

        private static string ToJson(Dictionary<string, string?> hash, char separator = ',')
        {
            string result = "";

            foreach (var key in hash.Keys)
            {
                result += $" {{{key} : {hash[key]}}}" + separator;
            }

            return result;
        }
    }
}