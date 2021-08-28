using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    /// <summary>
    /// Операция с аргументом типа sin(a), log10(a), sqrt(a)
    /// !Для decimal нужно определять свой метод расчета
    /// </summary>
    public abstract class ArgumentOperation : IOperation
    {
        public virtual int Priority { set; get; } = int.MaxValue;
        public abstract int GetValue(int a);
        public abstract decimal GetValue(decimal a);
    }
}
