using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabRunner
{
    

    class Lab3
    {
        public static void Run3(string inputFilePath, string outputFilePath)
        {
            var (participants, problems, corruptedEntries, sympathies) = ReadInput(inputFilePath);
            var interpretations = InterpretCorruptedData(corruptedEntries, participants, problems);
            var maxScore = CalculateMaxScore(interpretations, sympathies, participants);

            File.WriteAllText(outputFilePath, maxScore.ToString());
        }

        private static (List<string>, List<string>, List<string>, HashSet<int>) ReadInput(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            int participantCount = int.Parse(lines[0].Split(' ')[0]);
            int problemCount = int.Parse(lines[0].Split(' ')[1]);

            var participants = lines.Skip(1).Take(participantCount).ToList();
            var problems = lines.Skip(1 + participantCount).Take(problemCount).ToList();

            int corruptedEntryCount = int.Parse(lines[1 + participantCount + problemCount]);
            var corruptedEntries = lines.Skip(2 + participantCount + problemCount).Take(corruptedEntryCount).ToList();

            var sympathiesIndices = lines.Last().Split(' ').Select(int.Parse).ToHashSet();

            return (participants, problems, corruptedEntries, sympathiesIndices);
        }

        private static List<(string participant, string problem, int score)> InterpretCorruptedData(
            List<string> corruptedEntries, List<string> participants, List<string> problems)
        {
            var interpretations = new List<(string participant, string problem, int score)>();
            foreach (var entry in corruptedEntries)
            {
                var parts = entry.Split('-');
                var namePattern = "^" + Regex.Escape(parts[0]).Replace("\\?", ".") + "$";
                var problemPattern = "^" + Regex.Escape(parts[1]).Replace("\\?", ".") + "$";
                var score = int.Parse(parts[2]);

                foreach (var participant in participants)
                {
                    if (Regex.IsMatch(participant, namePattern))
                    {
                        foreach (var problem in problems)
                        {
                            if (Regex.IsMatch(problem, problemPattern))
                            {
                                interpretations.Add((participant, problem, score));
                            }
                        }
                    }
                }
            }
            return interpretations;
        }

        private static int CalculateMaxScore(
            List<(string participant, string problem, int score)> interpretations,
            HashSet<int> sympathies,
            List<string> participants)
        {
            var maxScores = new Dictionary<string, int>();

            foreach (var interpretation in interpretations)
            {
                var (participant, problem, score) = interpretation;
                if (sympathies.Contains(participants.IndexOf(participant) + 1))
                {
                    var key = participant + problem; // Unique key for participant-problem pair
                    if (!maxScores.ContainsKey(key) || maxScores[key] < score)
                    {
                        maxScores[key] = score; // Store the maximum score for this participant-problem pair
                    }
                }
            }

            int totalMaxScore = sympathies.Select(index => participants[index - 1]) // Get participant names from indices
                                         .Distinct() // Ensure each participant is only considered once
                                         .Sum(participant => maxScores
                                             .Where(kv => kv.Key.StartsWith(participant))
                                             .Sum(kv => kv.Value)); // Sum the maximum scores for each participant

            return totalMaxScore;
        }
    }
}
