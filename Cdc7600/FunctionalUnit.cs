namespace Cdc7600
{
    public class FunctionalUnit
    {
        public int LastStart { get; set; }
        public bool IsRunning { get; set; }
        public bool IsAvailable { get; set; }
        public FunctionalType Type { get; set; }

        public enum FunctionalType
        {
            Branch,
            Boolean,
            Shift,
            Add,
            LongAdd,
            Divide,
            Multiply,
            Increment
        }
    }
}
