using System;

namespace Calculator
{
    public class Pow : BinaryOperation
    {
        public override int GetValue(int a, int b) => (int)Math.Pow(a, b);
        public override decimal GetValue(decimal a, decimal b) => (decimal)Math.Pow((double)a, (double)b);
    }
}
