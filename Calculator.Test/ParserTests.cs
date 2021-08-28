using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Calculator.Test
{
    [TestClass]
    public class ParserTests
    {
        [DataTestMethod]
        [DataRow("-3.2-5", "0 - 3.2 - 5")] // 1
        [DataRow("3+5", "3 + 5")] // 2
        [DataRow("3+4*2/(1.7-5)^2", "3 + 4 * 2 / ( 1.7 - 5 ) ^ 2")] // 3
        [DataRow("(1.2+(7-11/(2+5))*4.1)*(2.4)", "( 1.2 + ( 7 - 11 / ( 2 + 5 ) ) * 4.1 ) * ( 2.4 )")] // 4
        [DataRow("sin(5)", "sin ( 5 )")] // 5
        [DataRow("2*sin(5)-7/sin(3+2)", "2 * sin ( 5 ) - 7 / sin ( 3 + 2 )")] // 6
        public void TestConvertToSegments(string equation, string expected)
        {
            Parser parser = new Parser();
            List<string> actual = parser.ConvertEquationToSegments(equation);
            Assert.AreEqual(expected, String.Join(" ", actual.ToArray()));
        }

        [DataTestMethod]
        [DataRow("-3.2-5", "0 3.2 - 5 -")] // 1
        [DataRow("3+5", "3 5 +")] // 2
        [DataRow("3^5", "3 5 ^")] // 3
        [DataRow("3.2*5.2", "3.2 5.2 *")] // 4
        [DataRow("3.2/5.2", "3.2 5.2 /")] // 5 
        [DataRow("2+3.5-5*4/5", "2 3.5 5 4 * 5 / - +")] // 6
        [DataRow("3+4*2/(1.7-5)^2", "3 4 2 * 1.7 5 - 2 ^ / +")] // 7
        [DataRow("(1.2+(7-11/(2+5))*4.1)*(2.4)", "1.2 7 11 2 5 + / - 4.1 * + 2.4 *")] // 8
        [DataRow("sin(5)", "5 sin")] // 9
        [DataRow("2*sin(5)-7/sin(3+2)", "2 5 sin * 7 3 2 + sin / -")] // 10
        [DataRow("-3", "0 3 -")] // 11
        [DataRow("3.2", "3.2")] // 12
        public void TestToPostfixNotation(string equation, string expected)
        {
            Parser parser = new Parser();
            List<string> actual = parser.ConvertToPostfixNotation(parser.ConvertEquationToSegments(equation));
            Assert.AreEqual(expected, String.Join(" ", actual.ToArray()));
        }
    }

    

}
