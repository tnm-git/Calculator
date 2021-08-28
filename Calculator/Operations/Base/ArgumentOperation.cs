using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    public abstract class ArgumentOperation : IOperation
    {
        public virtual int Priority { set; get; } = int.MaxValue;
        public abstract int GetValue(int a);
        public abstract decimal GetValue(decimal a);
    }
}
