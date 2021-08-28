using System;

namespace Calculator
{
    /// <summary>
    /// а в степени b, можно вызывать из sqrt
    /// </summary>
    public class Pow : BinaryOperation
    {
        public override int GetValue(int a, int b) => (int)Math.Pow(a, b);
        public override decimal GetValue(decimal a, decimal b) => (decimal)Math.Pow((double)a, (double)b);
    }
}
