namespace Cdc7600
{
    public class Instruction
    {
        public InstructionLength Length { get; set; }
        public OpCode OpCode { get; set; }

        public int Issue { get; set; }
        public int Start { get; set; }
        public int Result { get; set; }
        public int UnitReady { get; set; }
        public int? Fetch { get; set; }
        public int? Store { get; set; }

        public bool IsFinished { get; set; }

        public enum InstructionLength
        {
            Short,
            Long
        }

        public override string ToString()
        {
            return (int)OpCode + " (" + Length.ToString()[0] + ")";
        }
    }
}
