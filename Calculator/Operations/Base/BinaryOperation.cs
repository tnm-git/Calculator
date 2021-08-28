
namespace Calculator
{
    /// <summary>
    /// Операция с двумя аргументами типа a + b, a % b, a ^ b
    /// </summary>
    public abstract class BinaryOperation : IOperation
    {
        public virtual int Priority { set; get; } = 1;
        public abstract int GetValue(int a, int b);
        public abstract decimal GetValue(decimal a, decimal b);
    }
}
