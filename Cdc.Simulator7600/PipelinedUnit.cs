namespace Cdc.Simulator7600
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a pipelined functional unit by implementing a Queue
    /// and storing state information.  Management of the object must 
    /// be done externally.
    /// </summary>
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

        /// <summary>
        /// Identifies whether or not the unit can be issued a new instruction.
        /// </summary>
        /// <param name="currentTime">Represents the current </param>
        /// <returns>Returns true if the clock cycle provided is greater than
        /// or equal to the segment time plus the time at which the most recent
        /// instruction was issued, or if the unit is reserved.  
        /// Otherwise, it returns false.</returns>
        public bool IsReady(int currentTime)
        {
            if (Pipeline.Any())
            {
                return Pipeline.Last().UnitReady <= currentTime;
            }
            return true;
        }
    }
}
