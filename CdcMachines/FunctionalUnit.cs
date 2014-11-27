namespace CdcMachines
{
    public class FunctionalUnit
    {
        public Instruction Reserve { get; set; }
        public Instruction InProgress { get; set; }
        public UnitType Type { get; set; }

        public FunctionalUnit(UnitType type)
        {
            Type = type;
        }

        public bool IsReady()
        {
            return InProgress == null && Reserve == null;
        }

        public bool CanReserve()
        {
            return Reserve == null;
        }
    }
}
