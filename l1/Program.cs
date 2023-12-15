using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    private static Dictionary<int, long> cache = new Dictionary<int, long>();

    static void Main()
    {
        // Читання даних з файлу
        if (!File.Exists("INPUT.TXT"))
        {
            Console.WriteLine("Не знайдено файл INPUT.TXT");
            return;
        }

        int N = int.Parse(File.ReadAllText("INPUT.TXT").Trim());

        if (N < 2)
        {
            File.WriteAllText("OUTPUT.TXT", "Замале значення");
            return;
        }

        if (N > 100)
        {
            File.WriteAllText("OUTPUT.TXT", "Переповнення");
            return;
        }

        // Розрахунок кількості можливих мереж
        long result = A(N);

        // Запис результату в файл
        File.WriteAllText("OUTPUT.TXT", result.ToString());
    }

    static long A(int n)
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

    static long Binomial(int n, int k)
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
