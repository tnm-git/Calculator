using System;
using System.Collections.Generic;
using System.Reflection;

namespace Calculator
{
    public class OperationProvider
    {
        public string CalculateEquation(List<string> operands)
        {
            decimal val;
            Stack<decimal> stack = new Stack<decimal>();
                     
            for (int i = 0; i < operands.Count; i++)
            {

                if (decimal.TryParse(operands[i], out val))
                {
                    stack.Push(val);
                }
                else
                {
                    IOperation currentOperation;
                    if (operations.TryGetValue(operands[i], out currentOperation))
                    {
                        if (currentOperation is ArgumentOperation)
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
}
