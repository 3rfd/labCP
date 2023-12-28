using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static int GenerateSequences(int n, int k)
    {
        return GenerateAllSequences(n, k, new List<int> { 1 });
    }

    static int GenerateAllSequences(int n, int k, List<int> sequence)
    {
        if (sequence.Count == n)
        {
            return IsBeautiful(sequence) ? 1 : 0;
        }

        int count = 0;
        for (int nextNum = 1; nextNum <= k; nextNum++)
        {
            List<int> newSequence = new List<int>(sequence) { nextNum };
            count += GenerateAllSequences(n, k, newSequence);
        }

        return count;
    }

    static bool IsBeautiful(List<int> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            if (sequence[i] != 1 && !sequence.GetRange(0, i).Contains(sequence[i] - 1))
            {
                return false;
            }
        }
        return true;
    }

    static void Main()
    {
        string inputFilePath = "INPUT.TXT";
        string outputFilePath = "OUTPUT.TXT";

        // Read the input from file
        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string[] tokens = reader.ReadLine().Split(' ');
            int n = int.Parse(tokens[0]);
            int k = int.Parse(tokens[1]);

            // Calculate the number of beautiful sequences
            int result = GenerateSequences(n, k);

            // Write the output to file
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine(result);
            }
        }

        Console.WriteLine("The number of beautiful sequences has been written to OUTPUT.TXT");
    }
}