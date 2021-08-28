using System;

namespace Calculator
{
    public class Sinus : ArgumentOperation
    {
        public override int GetValue(int a) => (int)Math.Sin(a);
        public override decimal GetValue(decimal a) => (decimal)Math.Sin((double)a);
    }
}
