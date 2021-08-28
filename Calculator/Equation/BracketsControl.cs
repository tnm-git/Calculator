using System.Collections.Generic;

namespace Calculator
{
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

        public string CheckEquationBrackets(List<string> segments)
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
}
