using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string equ = "1+7-11/2+5.3*4*2";
            equ = "(1.2+(7-11/(2+5))*4.1)*(2.4)";
            equ = "3+4*2/(1-5)*2"; // 342*15-2*/+
            equ = "(3-6)*(2+1)"; // – -  36-21+*
            equ = "22-1/2*(-6-2)"; // 212/62^2−∗−
            equ = "2.2*4-sin(3)/2"; //
            equ = "sin(3.14 + 2 (-4) / 6)"; //
            equ = "3+5-6";

            equ = "(";
            //equ = ")";
            //equ = "()";
            //equ = "()()";
            //equ = ")(";

            //equ = ")()";
            //equ = "())";
            //equ = "()(";
            //equ = "(()(";
            //equ = "(())";
            //equ = "(()())";

            equ = "3 + 2";
            equ = "3a + 2";
            equ = "3 ++ 2";
            equ = "ins(2)-3";
            equ = "((2)-3+";
            equ = "-2";
            equ = "+2";
            equ = "-sin25-2/";
            equ = "--sin(25*-2)/";
            equ = "3.22 + 4";

            //equ = $"1 - 1 + {decimal.MaxValue} + 1 + 2";
            equ = $"1 - 1 {decimal.MinValue} - 1 - 2";
            //equ = $"{int.MaxValue} + 1";
            //equ = $"{int.MinValue} - 1";

            //equ = $"1/{int.MaxValue}";
            //equ = $"1/{decimal.MaxValue}";
            //equ = $"1/0";
            //equ = $"1/0.0";

            //equ = "1+7-11/2+5.3*4*2";
            //equ = "(1.2+(7-11/(2+5))*4.1)*(2.4)";
            //equ = "3+4*2/(1-5)*2";
            //equ = "(3-6)*(2+1)"; 
            //equ = "22-1/2*(-6-2)"; 
            //equ = "2.2 * 4-sin(3)/2";
            //equ = "3-sin(+3.14 + 2 * (-4) / 6)";
            //equ = "+3 - 2";
            //equ = "2 + 3!";

            //equ = $"1 - 1 - 2 - 1 - 2";

            equ = "(4*6.01)(22-3)";

            Parser parser = new Parser();

            Console.WriteLine("Input = " + equ);
            
            Console.WriteLine("Segments = " + String.Join(" ", parser.ConvertEquationToSegments(equ).ToArray()));
            //Console.WriteLine("Postfix = " + String.Join(" ", parser.ConvertToPostfixNotation().ToArray()));
            Console.WriteLine("Solve = " + parser.Solve(equ));
            
            /*
            while (true)
            {
                Console.Write("Введите выражение: ");
                equ = Console.ReadLine();

                if (equ.Contains("e"))
                    break;

                p = new Parser(equ);
                Console.WriteLine($"Результат: {p.Iterate()}");
                Console.WriteLine();
            }
            */

        }
    }
}
