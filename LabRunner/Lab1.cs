using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LabRunner
{
   

    class Lab1
    {
        private static Dictionary<int, long> cache = new Dictionary<int, long>();

        public static void Run1(string inputFilePath, string outputFilePath)
        {
            // Читання даних з файлу
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Не знайдено файл INPUT.TXT");
                return;
            }

            int N = int.Parse(File.ReadAllText(inputFilePath).Trim());

            if (N < 2)
            {
                File.WriteAllText(outputFilePath, "Замале значення");
                return;
            }

            if (N > 100)
            {
                File.WriteAllText(outputFilePath, "Переповнення");
                return;
            }

            // Розрахунок кількості можливих мереж
            long result = A(N);

            // Запис результату в файл
            File.WriteAllText(outputFilePath, result.ToString());
        }

        private static long A(int n)
        {
            if (n == 0) return 1;

            if (cache.ContainsKey(n)) return cache[n];

            long sum = 0;
            for (int k = 0; k < n; k++)
            {
                sum += A(k) * Binomial(n, k);
            }

            long result = (long)Math.Pow(2, Binomial(n, 2)) - sum;

            cache[n] = result;
            return result;
        }

        private static long Binomial(int n, int k)
        {
            if (k < 0 || k > n) return 0;
            if (k == 0 || k == n) return 1;

            long result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= n - (k - i);
                result /= i;
            }
            return result;
        }
    }
}
