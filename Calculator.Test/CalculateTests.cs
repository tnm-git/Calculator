using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Test
{
    [TestClass]
    public class CalculateTests
    {
        [DataTestMethod]
        [DataRow("1+7-11/2+5*4*2-sin(23^6)", "42.4332")] // 1
        [DataRow("(1.2+(7-11/(2+5)(24.004-8))*4.1)*(2.4)", "-175.7076")] // 2
        [DataRow("3(-25(62-7)+4*(3-2))/48^3", "-0.0372")] // 3
        [DataRow("2.2*4-sin(3)/2", "8.7294")] // 4
        [DataRow("sin(3.14 + 2 (-4) / 6)", "0.9723")] // 5
        public void SolveTest1(string equation, string expected)
        {
            Parser parser = new Parser();
            string actual = parser.Solve(equation);
            Assert.AreEqual(expected, decimal.Parse(actual).ToString("N4"));
        }

        [DataTestMethod]
        [DataRow("1+79228162514264337593543950335", "Вычисленное значение больше максимально возможного для данного типа")] // 1
        [DataRow("-1-79228162514264337593543950335", "Вычисленное значение больше максимально возможного для данного типа")] // 2
        [DataRow("2147483647+1", "2147483648")] // 3
        [DataRow("792281625142643375935439503350 + 1", "Вычисленное значение больше максимально возможного для данного типа")] // 6
        [DataRow("-1-2147483647", "-2147483648")] // 7
        [DataRow("1/79228162514264337593543950335", "0")] // 8
        [DataRow("1/0", "Попытка деления на ноль")] // 9
        [DataRow("1/0.0", "Попытка деления на ноль")] // 10
        public void SolveTest2(string equation, string expected)
        {
            Parser parser = new Parser();
            string actual = parser.Solve(equation);
            Assert.AreEqual(expected, actual);
        }
    }
}
