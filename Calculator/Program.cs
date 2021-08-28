using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string equ;
            Parser parser = new Parser();

            while (true)
            {
                Console.Write("Введите выражение: ");
                equ = Console.ReadLine();

                if (equ.Contains("e"))
                    break;

                Console.WriteLine($"Результат: {parser.Solve(equ)}");
                Console.WriteLine();
            }
        }
    }
}
