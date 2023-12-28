using LabRunner;
using System;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введіть команду (version, run lab1/lab2/lab3, set-path -p/--path <шлях>):");
                string input = Console.ReadLine();
                string[] inputTokens = input.Split(' ');

                if (inputTokens.Length == 0)
                {
                    Console.WriteLine("Невірна команда. Введіть команду ще раз.");
                    continue;
                }

                string command = inputTokens[0].ToLower();

                switch (command)
                {
                    case "version":
                        ShowVersionInfo();
                        break;
                    case "run":
                        ProcessRunCommand(inputTokens);
                        break;
                    case "set-path":
                        SetLabPath(inputTokens);
                        break;
                    default:
                        Console.WriteLine("Невірна команда. Введіть команду ще раз.");
                        break;
                }
            }
        }

        static void ShowVersionInfo()
        {
            Console.WriteLine("Автор: kovtunenko denys");
            Console.WriteLine("Версія: 1.0");
        }

        static void ProcessRunCommand(string[] inputTokens)
        {
            if (inputTokens.Length < 2)
            {
                Console.WriteLine("Недостатньо аргументів для команди 'run'.");
                return;
            }

            string subCommand = inputTokens[1].ToLower();
            string inputFilePath = null;
            string outputFilePath = null;

            for (int i = 2; i < inputTokens.Length; i++)
            {
                switch (inputTokens[i])
                {
                    case "-I":
                    case "--input":
                        inputFilePath = i + 1 < inputTokens.Length ? inputTokens[++i] : null;
                        break;
                    case "-o":
                    case "--output":
                        outputFilePath = i + 1 < inputTokens.Length ? inputTokens[++i] : null;
                        break;
                }
            }

            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath))
            {
                string labPath = Environment.GetEnvironmentVariable("LAB_PATH");
                if (!string.IsNullOrEmpty(labPath))
                {
                    inputFilePath ??= Path.Combine(labPath, "input.txt");
                    outputFilePath ??= Path.Combine(labPath, "output.txt");
                }
            }

            if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath))
            {
                Console.WriteLine("Не вказано шлях до вхідного або вихідного файлу.");
                return;
            }

            switch (subCommand)
            {
                case "lab1":
                    Lab1.Run1(inputFilePath, outputFilePath);
                    break;
                case "lab2":
                    Lab2.Run2(inputFilePath, outputFilePath);
                    break;
                case "lab3":
                    Lab3.Run3(inputFilePath, outputFilePath);
                    break;
                default:
                    Console.WriteLine("Невідома підкоманда для 'run'.");
                    break;
            }
        }

        static void SetLabPath(string[] inputTokens)
        {
            if (inputTokens.Length < 3 || (inputTokens[1] != "-p" && inputTokens[1] != "--path"))
            {
                Console.WriteLine("Невірна команда 'set-path'.");
                return;
            }

            string labPath = inputTokens[2];
            Environment.SetEnvironmentVariable("LAB_PATH", labPath);
            Console.WriteLine($"Шлях до папки з інпут та аутпут файлами встановлено на '{labPath}'");
        }
    }
}
