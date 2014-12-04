namespace Cdc.Simulator7600
{
    /// <summary>
    /// Stores the content and timing information associated for
    /// a processable instruction.
    /// </summary>
    public class Instruction
    {
        public OpCode OpCode { get; set; }
        public InstructionLength Length { get; set; }
        public Register Operand1 { get; set; }
        public Register Operand2 { get; set; }
        public Register OutputRegister { get; set; }
        public Instruction BranchTo { get; set; }
        public int Value { get; set; }

        public int Issue { get; set; }
        public int Start { get; set; }
        public int Result { get; set; }
        public int UnitReady { get; set; }
        public int? Fetch { get; set; }
        public int? Store { get; set; }

        public bool IsFinished { get; set; }
        public bool IsBeingHeld { get; set; }
        public bool IsStartOfWord { get; set; }
        public bool IsEndOfWord { get; set; }

        /// <summary>
        /// Converts the instruction to a friendlier representation.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            return (int)OpCode + " (" + Length.ToString()[0] + ")";
        }
        /// <summary>
        /// Converts the instruction's schedule information to a string.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public string GetScheduleOutput()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                    OutputRegister + "=" + Operand1 + "," + Operand2,
                    (int)OpCode,
                    Length.ToString()[0],
                    Issue,
                    Start,
                    Result,
                    UnitReady,
                    Fetch,
                    Store);
        }
        /// <summary>
        /// Resets all of the instructions timing information to 
        /// their default values.
        /// </summary>
        internal void Reset()
        {
            Issue = 0;
            Start = 0;
            Result = 0;
            UnitReady = 0;
            Fetch = null;
            Store = null;
            IsFinished = false;
            IsBeingHeld = false;
        }
    }
}
