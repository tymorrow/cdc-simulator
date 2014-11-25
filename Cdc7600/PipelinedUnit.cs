namespace Cdc7600
{
    public class PipelinedUnit : FunctionalUnit
    {
        public int SegmentTime { get; set; }

        public PipelinedUnit(int segmentTime)
        {
            SegmentTime = segmentTime;
        }
    }
}
