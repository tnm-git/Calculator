using System;
using System.Collections.Generic;

namespace Calculator
{
    public interface IOperation 
    {
        public int Priority { set; get; }
    }

    public abstract class BinaryOperation : IOperation
    {
        public virtual int Priority { set; get; } = 1;
        public abstract int GetValue(int a, int b);
        public abstract decimal GetValue(decimal a, decimal b);
    }

    public abstract class ArgumentOperation : IOperation
    {
        public virtual int Priority { set; get; } = int.MaxValue;
        public abstract int GetValue(int a);
        public abstract decimal GetValue(decimal a);
    }


    public class Sum : BinaryOperation
    {
        public override int GetValue(int a, int b) => a + b;
        public override decimal GetValue(decimal a, decimal b) => a + b;
    }

    public class Diff : BinaryOperation
    {
        public override int GetValue(int a, int b) => a - b;
        public override decimal GetValue(decimal a, decimal b) => a - b;
    }

    public class Mult : BinaryOperation
    {
        public override int GetValue(int a, int b) => a * b;
        public override decimal GetValue(decimal a, decimal b) => a * b;
    }

    public class Divide : BinaryOperation
    {
        public override int GetValue(int a, int b) => a / b;
        public override decimal GetValue(decimal a, decimal b) => a / b;
    }

    public class Sinus : ArgumentOperation
    {
        public override int GetValue(int a) => (int)Math.Sin(a);
        public override decimal GetValue(decimal a) => (decimal)Math.Sin((double)a);
    }

    public class Pow : BinaryOperation
    {
        public override int GetValue(int a, int b) => (int)Math.Pow(a, b);
        public override decimal GetValue(decimal a, decimal b) => (decimal)Math.Pow((double)a, (double)b);
    }

    public class Factorial : ArgumentOperation
    {
        public override int GetValue(int a)
        {
            if (a == 0)
                return 1;
            else
                return a * GetValue(a - 1);
        }
        public override decimal GetValue(decimal a)
        {
            if (a == 0)
                return 1;
            else
                return a * GetValue(a - 1);
        }
    }


    // использовать лямбда выраженияв определении объектов (функций)
    // сумма, разность, умножением, деление можно реализовать перегрузкой

    /*
    public struct Type 
    {


        object val;
        public Type(object type)
        {
            val = type;
        }

        public static bool TryParse(string value, out Type output)
        {
            return Decimal.TryParse(value, out output);
        }
    }
    */

    public class OperationProvider
    {
        public string CalculateEquation(List <string> operands)
        {
            Stack<decimal> stack = new Stack<decimal>();

            for (int i = 0; i < operands.Count; i++)
            {
                decimal val;

                if (decimal.TryParse(operands[i], out val))
                {
                    stack.Push(val);
                }
                else
                {
                    IOperation currentOperation;
                    if (operations.TryGetValue(operands[i], out currentOperation))
                    {
                        if(currentOperation is ArgumentOperation)
                        {
                            if (stack.Count < 1)
                                return $"Некорректные аргументы функции '{operands[i]}'";

                            ArgumentOperation solver = currentOperation as ArgumentOperation;

                            decimal operation = stack.Pop();

                            try
                            {
                                stack.Push(solver.GetValue(operation));
                            }
                            catch (OverflowException ex)
                            {
                                return $"Вычисленное значение больше максимально возможного для данного типа";
                            }

                        }
                        else if (currentOperation is BinaryOperation)
                        {
                            if (stack.Count < 2)
                                return $"Некорректные аргументы операции '{operands[i]}'";

                            BinaryOperation solver = currentOperation as BinaryOperation;

                            decimal operation2 = stack.Pop();
                            decimal operation1 = stack.Pop();

                            try
                            {
                                stack.Push(solver.GetValue(operation1, operation2));
                            }
                            catch (DivideByZeroException ex)
                            {
                                return $"Попытка деления на ноль";
                            }
                            catch (OverflowException ex)
                            {
                                return $"Вычисленное значение больше максимально возможного для данного типа";
                            }

                        }
                        else
                        {
                            return $"Некорректная операция '{operands[i]}'";
                        }
                    }
                    else
                    {
                        return $"Некорректная операция '{operands[i]}'";
                    }
                }
            }
            return stack.Peek().ToString();
        }

        public int GetPriority(string key)
        {
            IOperation obj;
            if (!operations.TryGetValue(key, out obj))
                return -1;
            return obj.Priority;
        }

        private string GetOperandsKeys()
        {
            string res = string.Empty;
            foreach (var obj in operations)
            {
                res += obj.Key;
            }
            return res;
        }



        public bool IsNumber(string num)
        {
            Decimal val;
            return Decimal.TryParse(num, out val);
        }

        public bool Contains(char s)
        {
            return GetOperandsKeys().Contains(s);
        }

        public int ContainsNameLength(char s)
        {
            string res = string.Empty;
            foreach (var obj in operations)
            {
                if (obj.Key.Contains(s))
                    return obj.Key.Length;
            }
            return -1;
        }

        readonly private Dictionary<string, IOperation> operations = new Dictionary<string, IOperation>
        {
            { "+", new Sum() { Priority = 0 } },
            { "-", new Diff() { Priority = 1 } },
            { "*", new Mult() { Priority = 2 } },
            { "/",  new Divide() { Priority = 2 } },
            { "^",  new Pow() { Priority = 3 } },
            { "sin", new Sinus() },
            { "!", new Factorial() },
        };
    }


    public class Bracket
    {
        string open;
        string close;
        int priority;

        public Bracket(string open, string close, int priority)
        {
            this.open = open;
            this.close = close;
            this.priority = priority;
        }

        public Bracket(Bracket bracket)
        {
            this.open = bracket.open;
            this.close = bracket.close;
            this.priority = bracket.priority;
        }

        public string Open
        {
            get => open;
        }

        public string Close
        {
            get => close;
        }

    }

    public class BracketsControl
    {
        readonly private List<Bracket> brackets = new List<Bracket>()
        {
            new Bracket("(", ")", 1),
            //new Bracket("{", "}", 2)
        };

        public Bracket Current;

        public BracketsControl()
        {
            Current = new Bracket(brackets[0]);
        }

        public bool Contains(char s)
        {
            return GetAllBrackets().Contains(s);
        }

        private string GetAllBrackets()
        {
            string res = string.Empty;
            foreach (var obj in brackets)
            {
                res += obj.Open;
                res += obj.Close;
            }
            return res;
        }

        public string CheckEquationBrackets(List <string> segments)
        {
            int openBracketsCount = 0;
            int closeBracketsCount = 0;

            foreach (string s in segments)
            {
                if (s.Equals(brackets[0].Open))
                    openBracketsCount++;

                if (s.Equals(brackets[0].Close))
                    closeBracketsCount++;

                if (closeBracketsCount > openBracketsCount)
                    return "Некорректные скобки";
            }

            if (openBracketsCount != closeBracketsCount)
                return "Некорректные скобки";

            return string.Empty;
        }
    }


    public class Parser
    {
        OperationProvider operationProvider;
        BracketsControl bracketsControl;

        readonly private string[] symbolsSeparators =
        {
            " ",
            "\t",
            "\n"
        };

        public Parser()
        {
            operationProvider = new OperationProvider();
            bracketsControl = new BracketsControl();
            SetCultureToInvariant();
        }
        public bool IsDigitOrDot(char s)
        {
            return Char.IsDigit(s) || s.Equals('.');
        }

        public string CheckEquation(string equation)
        {
            for (int i = 0; i < equation.Length; i++)
            {
                if (!operationProvider.Contains(equation[i]) && 
                    !bracketsControl.Contains(equation[i]) &&
                    !IsDigitOrDot(equation[i]))
                {
                    return "Некорректное выражение";
                }
            }

            return string.Empty; 
        }

        private void SetCultureToInvariant()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo("en-US");
        }

        private string RemoveSeparators(string equation)
        {
            foreach (string s in symbolsSeparators)
                equation = equation.Replace(s, string.Empty);

            return equation;
        }

        private bool ContainsDigitOrDot(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (IsDigitOrDot(str[i]))
                {
                    return true;
                }
            }
            return false;
        }


        private List<string> AddUnvisibleOperands(List <string> segments)
        {
            int idx = 0;
            string prev = string.Empty;
            List<string> resultSegments = new List<string>();
            foreach (string s in segments)
            {
                if ((idx > 0 && (s.Equals("-") || s.Equals("+")) && prev.Equals(bracketsControl.Current.Open)) || 
                    (idx == 0 && (s.Equals("-") || s.Equals("+"))))
                {
                    resultSegments.Add("0");
                }

                if ((idx > 0 && s.Equals(bracketsControl.Current.Open) && (ContainsDigitOrDot(prev) ||
                    prev.Equals(bracketsControl.Current.Close))))
                {
                    resultSegments.Add("*");
                }

                resultSegments.Add(s);
                prev = s;
                idx++;
            }
            return resultSegments;
        }

        public List<string> ConvertEquationToSegments(string equation)
        {
            List<string> segments = new List<string>();

            bool start = false;
            bool stop = false;
            string segment = string.Empty;
            for (int i = 0; i < equation.Length; i++)
            {
                if (IsDigitOrDot(equation[i]) || operationProvider.ContainsNameLength(equation[i]) > 1)
                {
                    start = true;
                    segment += equation[i];
                }
                else
                {
                    if (start)
                    {
                        stop = true;
                        start = false;
                    }
                }

                if (stop)
                {
                    stop = false;
                    segments.Add(segment);
                    segment = string.Empty;
                }

                if (operationProvider.ContainsNameLength(equation[i]) == 1 || 
                    bracketsControl.Contains(equation[i]))
                {
                    segments.Add(equation[i].ToString());
                }
            }

            if (start)
                segments.Add(segment);

            return AddUnvisibleOperands(segments);
        }


        public List<string> ConvertToPostfixNotation(List<string> segments)
        {
            Stack<string> stack = new Stack<string>();
            List<string> result = new List<string>();

            foreach(string s in segments)
            {
                if (operationProvider.IsNumber(s))
                {
                    result.Add(s);
                }
                else
                {
                    if (s.Equals(bracketsControl.Current.Open))
                    {
                        stack.Push(s);
                    }
                    else if (s.Equals(bracketsControl.Current.Close))
                    {
                        while (!stack.Peek().Equals(bracketsControl.Current.Open))
                        {
                            result.Add(stack.Pop().ToString());
                        }
                        stack.Pop();
                    }
                    else
                    {
                        while (stack.Count > 0 && 
                            operationProvider.GetPriority(s) <= operationProvider.GetPriority(stack.Peek()))
                        {
                            result.Add(stack.Pop().ToString());
                        }
                        stack.Push(s);
                    }
                }
            }

            while (stack.Count > 0)
                result.Add(stack.Pop().ToString());

            return result;
        }

        public string Solve(string input)
        {
            string equation = RemoveSeparators(input);
            string errorStr = CheckEquation(equation);
            if (!string.IsNullOrEmpty(errorStr))
                return errorStr;

            List<string> segments = ConvertToPostfixNotation(ConvertEquationToSegments(equation));

            errorStr = bracketsControl.CheckEquationBrackets(segments);
            if (!string.IsNullOrEmpty(errorStr))
                return errorStr;

            return operationProvider.CalculateEquation(segments);
        }


    }
}
