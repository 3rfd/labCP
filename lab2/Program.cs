using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class PlanetData
{
    public char Type { get; set; }
    public int ChatlanTo { get; set; }
    public int PatsakTo { get; set; }
    public int ChatlanFrom { get; set; }
    public int PatsakFrom { get; set; }
}

class Program
{
    static void Main()
    {
        string inputPath = "INPUT.TXT";
        string outputPath = "OUTPUT.TXT";

        List<PlanetData> planetsData = ReadPlanetDataFromFile(inputPath);
        var result = FindOptimalRoute(planetsData);
        WriteOutputToFile(outputPath, result);
    }

    static List<PlanetData> ReadPlanetDataFromFile(string filePath)
    {
        var planetsData = new List<PlanetData>();
        var lines = File.ReadAllLines(filePath);
        int N = int.Parse(lines[0]);

        for (int i = 1; i <= N; i++)
        {
            var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            planetsData.Add(new PlanetData
            {
                Type = parts[0][0],
                ChatlanTo = int.Parse(parts[1]),
                PatsakTo = int.Parse(parts[2]),
                ChatlanFrom = int.Parse(parts[3]),
                PatsakFrom = int.Parse(parts[4])
            });
        }

        return planetsData;
    }

    static void WriteOutputToFile(string filePath, Tuple<int, List<int>> result)
    {
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(result.Item1);
            writer.WriteLine(string.Join(" ", result.Item2));
        }
    }

    static Tuple<int, List<int>> FindOptimalRoute(List<PlanetData> planetsData)
    {
        int optimalTsaks = int.MaxValue;
        List<int> optimalRoute = null;

        var planetsIndices = Enumerable.Range(1, planetsData.Count).ToList();
        var permutations = GetPermutations(planetsIndices, planetsData.Count);

        foreach (var perm in permutations)
        {
            var route = new List<int> { 0 }.Concat(perm).Concat(new List<int> { 0 }).ToList();
            int tsaks = CalculateTsaks(route, planetsData);
            if (tsaks < optimalTsaks)
            {
                optimalTsaks = tsaks;
                optimalRoute = route;
            }
        }

        return Tuple.Create(optimalTsaks, optimalRoute);
    }

    static int CalculateTsaks(List<int> route, List<PlanetData> planetsData)
    {
        int tsaks = 0;
        int chatlansOnShip = 0;
        int patsaksOnShip = 0;

        // Initialize the number of passengers on the ship from Plyuk
        foreach (var planet in planetsData)
        {
            chatlansOnShip += planet.ChatlanTo;
            patsaksOnShip += planet.PatsakTo;
        }

        for (int i = 0; i < route.Count - 1; i++)
        {
            var currentPlanetIndex = route[i];
            var nextPlanetIndex = route[i + 1];

            // Skip Plyuk (0) when accessing planet data
            if (currentPlanetIndex > 0)
            {
                var currentPlanet = planetsData[currentPlanetIndex - 1];

                // Apply tsaks for Patsaks on a Chatlan planet and for Chatlans on a Patsak planet
                if (currentPlanet.Type == 'C')
                {
                    tsaks += patsaksOnShip;
                }
                else if (currentPlanet.Type == 'P')
                {
                    tsaks += chatlansOnShip;
                }
            }

            // Adjust the number of passengers on board for the next planet
            if (nextPlanetIndex > 0)
            {
                var nextPlanet = planetsData[nextPlanetIndex - 1];
                chatlansOnShip -= nextPlanet.ChatlanTo;
                patsaksOnShip -= nextPlanet.PatsakTo;
            }

            // Add passengers boarding from the current planet
            if (currentPlanetIndex > 0)
            {
                var currentPlanet = planetsData[currentPlanetIndex - 1];
                chatlansOnShip += currentPlanet.ChatlanFrom;
                patsaksOnShip += currentPlanet.PatsakFrom;
            }
        }

        return tsaks;
    }

    static IEnumerable<IEnumerable<int>> GetPermutations(IEnumerable<int> list, int length)
    {
        if (length == 1) return list.Select(t => new int[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                        (t1, t2) => t1.Concat(new int[] { t2 }));
    }
}
