using System;
using System.Collections.Generic;

namespace Calculator
{
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
