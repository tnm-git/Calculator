using System;
using System.Collections.Generic;

namespace Calculator
{
    public class OperationProvider
    {
        /// <summary>
        /// Словарь операций, функций
        /// </summary>
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

        /// <summary>
        /// Непосредственный расчет выражения
        /// работаем только с decimal
        /// </summary>
        /// <param name="operands">операнды</param>
        /// <returns></returns>
        public string CalculateEquation(List<string> operands)
        {
            decimal val;
            Stack<decimal> stack = new Stack<decimal>(); // стек решений
                     
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

        /// <summary>
        /// Получить приоритет по ключу функции
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetPriority(string key)
        {
            IOperation obj;
            if (!operations.TryGetValue(key, out obj))
                return -1; // если ключ не найден
            return obj.Priority;
        }

        /// <summary>
        /// Объеденить все ключи словаря
        /// </summary>
        /// <returns>и вернуть</returns>
        private string GetOperandsKeys()
        {
            string res = string.Empty;
            foreach (var obj in operations)
            {
                res += obj.Key;
            }
            return res;
        }

        /// <summary>
        /// Строка преобразуется в decimal?
        /// </summary>
        /// <param name="num"></param>
        /// <returns>true если decimal число</returns>
        public bool IsNumber(string num)
        {
            decimal val;
            return decimal.TryParse(num, out val);
        }

        /// <summary>
        /// Содержится ли в названии ключей символ
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true если содержится</returns>
        public bool Contains(char s)
        {
            return GetOperandsKeys().Contains(s);
        }

        /// <summary>
        /// Получить число символов ключа
        /// </summary>
        /// <param name="s"></param>
        /// <returns>-1 если ключ не найден</returns>
        public int ContainsNameLength(char s)
        {
            foreach (var obj in operations)
            {
                if (obj.Key.Contains(s))
                    return obj.Key.Length;
            }
            return -1;
        }


    }
}
