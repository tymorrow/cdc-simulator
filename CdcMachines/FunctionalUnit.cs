namespace CdcMachines
{
    public class FunctionalUnit
    {
        public Instruction InUse { get; set; }
        public UnitType Type { get; set; }

        public FunctionalUnit(UnitType type)
        {
            Type = type;
        }

        public bool IsInUse()
        {
            return InUse != null;
        }
    }
}
