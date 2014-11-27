namespace Cdc7600
{
    using System.Collections.Generic;

    public class PipelinedUnit
    {
        public Queue<Instruction> Pipeline { get; set; } 
        public int SegmentTime { get; set; }
        public UnitType Type { get; set; }
        public int LastStart { get; set; }
        public bool IsReserved { get; set; }

        public PipelinedUnit(int segmentTime, UnitType type)
        {
            Pipeline = new Queue<Instruction>();
            SegmentTime = segmentTime;
            Type = type;
        }

        public bool IsReady(int currentTime)
        {
            return (LastStart + SegmentTime <= currentTime) || IsReserved;
        }
    }

    public enum UnitType
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
