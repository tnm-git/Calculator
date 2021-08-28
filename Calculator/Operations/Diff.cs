
namespace Calculator
{
    /// <summary>
    /// Разность двух чисел
    /// </summary>
    public class Diff : BinaryOperation
    {
        public override int GetValue(int a, int b) => a - b;
        public override decimal GetValue(decimal a, decimal b) => a - b;
    }
}
