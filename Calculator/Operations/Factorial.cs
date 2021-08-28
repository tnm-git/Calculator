
namespace Calculator
{
    /// <summary>
    /// Факториал числа
    /// </summary>
    public class Factorial : ArgumentOperation
    {
        public override int GetValue(int a)
        {
            if (a == 0)
                return 1;
            else
                return a * GetValue(a - 1);
        }
        public override decimal GetValue(decimal a)
        {
            if (a == 0)
                return 1;
            else
                return a * GetValue(a - 1);
        }
    }
}
