using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Test
{
    [TestClass]
    public class ErrorsParserTests
    {
        [DataTestMethod]
        [DataRow("(", "Некорректные скобки")] // 1
        [DataRow(")", "Некорректные скобки")] // 2
        [DataRow("()", "")] // 3
        [DataRow("()()", "")] // 4
        [DataRow(")(", "Некорректные скобки")] // 5 
        [DataRow(")()", "Некорректные скобки")] // 6
        [DataRow("())", "Некорректные скобки")] // 7
        [DataRow("()(", "Некорректные скобки")] // 8
        [DataRow("(()(", "Некорректные скобки")] // 9
        [DataRow("(())", "")] // 10
        [DataRow("(()())", "")] // 11
        public void BracketsTest(string equation, string expected)
        {
            BracketsControl bracketsControl = new BracketsControl();
            Parser parser = new Parser();

            string actual = bracketsControl.CheckEquationBrackets(parser.ConvertEquationToSegments(equation));
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("3a + 2")] // 1
        [DataRow("3 ++ 2")] // 2
        [DataRow("ins (2)-3")] // 3
        [DataRow("((2)-3+")] // 4
        [DataRow("-sin25-2/")] // 5
        [DataRow("--sin(25*-2)/")] // 6
        [DataRow("/2")] // 7
        [DataRow("8.6.5")] // 8
        [DataRow("22,1 * 3 + (4)")] // 9
        public void FormatErrorsTest(string equation)
        {
            Parser parser = new Parser();
            string actual = parser.Solve(equation);
            Assert.IsTrue(actual.Contains("Некорр"));
        }
    }
}
