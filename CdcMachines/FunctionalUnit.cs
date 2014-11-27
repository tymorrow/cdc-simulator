namespace CdcMachines
{
    public class FunctionalUnit
    {
        public Instruction Instruction { get; set; }
        public UnitType Type { get; set; }
        public int TimeReady { get; set; }
        public bool IsReserved { get; set; }

        public FunctionalUnit(UnitType type)
        {
            Type = type;
        }

        public virtual bool IsReady(int currentTime)
        {
            return (TimeReady <= currentTime) || IsReserved;
        }
    }
}
