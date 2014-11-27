namespace CdcMachines
{
    using System.Collections.Generic;

    public class FunctionalUnitPipelined : FunctionalUnit
    {
        public Queue<Instruction> Pipeline { get; set; } 
        public int SegmentTime { get; set; }
        public int LastStart { get; set; }

        public FunctionalUnitPipelined(int segmentTime, UnitType type) : base(type)
        {
            Pipeline = new Queue<Instruction>();
            SegmentTime = segmentTime;
        }

        public override bool IsReady(int currentTime)
        {
            return (LastStart + SegmentTime <= currentTime) || IsReserved;
        }
    }
}
