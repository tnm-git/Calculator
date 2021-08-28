
namespace Calculator
{
    public class Mult : BinaryOperation
    {
        public override int GetValue(int a, int b) => a * b;
        public override decimal GetValue(decimal a, decimal b) => a * b;
    }
}
