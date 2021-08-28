using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string equ = "1+7-11/2+5.3*4*2";
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
