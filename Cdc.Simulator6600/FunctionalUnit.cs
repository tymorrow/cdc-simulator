namespace Cdc.Simulator6600
{
    /// <summary>
    /// Represents a functional unit by storing state information.  
    /// Management of the object must be done externally.
    /// </summary>
    public class FunctionalUnit
    {
        public Instruction InUse { get; set; }
        public UnitType Type { get; set; }

        public FunctionalUnit(UnitType type)
        {
            Type = type;
        }

        /// <summary>
        /// Identifies whether or not the unit can be issued a new instruction.
        /// </summary>
        /// <returns>Returns true if the unit is marked as "in use."</returns>
        public bool IsInUse()
        {
            return InUse != null;
        }
    }
}
