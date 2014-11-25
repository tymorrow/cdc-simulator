namespace Cdc7600
{
    using System.Collections.Generic;

    public class ArithmeticLogicUnit
    {
        public class FunctionalUnit
        {
            public Dictionary<OpCode, int> TimingMap;  
            public int LastStart { get; set; }
            public bool IsRunning { get; set; }
            public bool IsAvailable { get; set; }

            public FunctionalUnit(Dictionary<OpCode, int > timingMap)
            {
                TimingMap = timingMap;
            }
        }

        public class PipeLinedFunctionalUnit : FunctionalUnit
        {
            public int SegmentTime { get; set; }

            public PipeLinedFunctionalUnit(int segmentTime, Dictionary<OpCode, int> timingMap) : base(timingMap)
            {
                SegmentTime = segmentTime;
            }
        }

        public class PipeLinedBranchUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedBranchUnit() : base(0, new Dictionary<OpCode, int>
            {

            }) {}
        }
        public class PipeLinedBooleanUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedBooleanUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedShiftUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedShiftUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedAddUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedAddUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedLongAddUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedLongAddUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedDivideUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedDivideUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedMultiplyUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedMultiplyUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }
        public class PipeLinedIncrementUnit : PipeLinedFunctionalUnit
        {
            public PipeLinedIncrementUnit() : base(0, new Dictionary<OpCode, int>
            {
                
            }) {}
        }

    }
}
