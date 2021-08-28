using System;
using System.Collections.Generic;

namespace Calculator
{
    public class Parser
    {
        OperationProvider operationProvider;
        BracketsControl bracketsControl;

        /// <summary>
        /// Символы разделителей операндов выражения
        /// </summary>
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


        /// <summary>
        /// Проверить выражение по всем возможным символам (скобки, операции, функции)
        /// </summary>
        /// <param name="equation"></param>
        /// <returns>пусто, если нет символов, не входящих в выр-я</returns>
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

        /// <summary>
        /// Чтобы при парсинге в decimal использовать точку
        /// </summary>
        private void SetCultureToInvariant()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Удалить все разделители
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Добавить скрытые операторы для правильной обработки выражений
        /// </summary>
        /// <param name="segments">раздлеленные операнды выражения</param>
        /// <returns></returns>
        private List<string> AddUnvisibleOperands(List <string> segments)
        {
            int idx = 0;
            string prev = string.Empty;
            List<string> resultSegments = new List<string>();
            foreach (string s in segments)
            {
                // добавить 0 перед минусом или 0 перед плюсом
                if ((idx > 0 && (s.Equals("-") || s.Equals("+")) && prev.Equals(bracketsControl.Current.Open)) || 
                    (idx == 0 && (s.Equals("-") || s.Equals("+"))))
                {
                    resultSegments.Add("0");
                }

                // добавить * перед скобкой
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

        /// <summary>
        /// Преобразовать выражение в последовательность операндов
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
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
                    start = true; // начало имени функции или выражения с length > 1
                    segment += equation[i]; // заполняем строку
                }
                else
                {
                    if (start) 
                    {
                        stop = true;
                        start = false;
                    }
                }

                if (stop) // по окончанию строки
                {
                    stop = false;
                    segments.Add(segment); // добавляем в коллекцию
                    segment = string.Empty;
                }
                // для символом операндов с длиной 1
                if (operationProvider.ContainsNameLength(equation[i]) == 1 || 
                    bracketsControl.Contains(equation[i]))
                {
                    segments.Add(equation[i].ToString());
                }
            }

            if (start) // для последнего символа
                segments.Add(segment);

            return AddUnvisibleOperands(segments);
        }

        /// <summary>
        /// Кодируем последовательность в постфиксную нотацию (обратная Польская) 
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Оболочка для Solve operationProvider
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
