
namespace Calculator
{

    /// <summary>
    /// Пара скобок
    /// </summary>
    public class Bracket
    {
        string open; // обозначение открытой
        string close; // обозначение закрытой
        int priority; // приоритет

        public Bracket(string open, string close, int priority)
        {
            this.open = open;
            this.close = close;
            this.priority = priority;
        }

        public Bracket(Bracket bracket)
        {
            this.open = bracket.open;
            this.close = bracket.close;
            this.priority = bracket.priority;
        }

        public string Open
        {
            get => open;
        }

        public string Close
        {
            get => close;
        }

    }
}
