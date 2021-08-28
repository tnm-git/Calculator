using System.Collections.Generic;

namespace Calculator
{
    public class BracketsControl
    {

        /// <summary>
        /// Скобки для выражений, могут в принципе называться scopeO scopeC
        /// </summary>
        readonly private List<Bracket> brackets = new List<Bracket>()
        {
            new Bracket("(", ")", 1),
            //new Bracket("{", "}", 2)
        };

        public Bracket Current; // текущая пара скобок

        public BracketsControl()
        {
            Current = new Bracket(brackets[0]);
        }

        /// <summary>
        /// Содержат ли скобки элемент
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Contains(char s)
        {
            return GetAllBrackets().Contains(s);
        }

        /// <summary>
        /// Получить все скобки в виде строки
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает строку с ошибкой, если скобки некорректны
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public string CheckEquationBrackets(List<string> segments)
        {
            int openBracketsCount = 0; // счетчик открытых скобок
            int closeBracketsCount = 0; // счетчик закрытых скобок

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
}
