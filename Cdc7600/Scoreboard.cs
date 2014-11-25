namespace Cdc7600
{
    public class Scoreboard
    {
        private PipelinedUnit _branchUnit = new PipelinedUnit(1);
        private PipelinedUnit _booleanUnit = new PipelinedUnit(1);
        private PipelinedUnit _shiftUnit = new PipelinedUnit(1);
        private PipelinedUnit _addUnit = new PipelinedUnit(1);
        private PipelinedUnit _longAddUnit = new PipelinedUnit(1);
        private PipelinedUnit _divideUnit = new PipelinedUnit(18);
        private PipelinedUnit _multiplyUnit = new PipelinedUnit(2);
        private PipelinedUnit _incrementUnit = new PipelinedUnit(1);

        public PipelinedUnit BranchUnit
        {
            get { return _branchUnit; }
            set { _branchUnit = value; }
        }

        public PipelinedUnit BooleanUnit
        {
            get { return _booleanUnit; }
            set { _booleanUnit = value; }
        }

        public PipelinedUnit ShiftUnit
        {
            get { return _shiftUnit; }
            set { _shiftUnit = value; }
        }

        public PipelinedUnit AddUnit
        {
            get { return _addUnit; }
            set { _addUnit = value; }
        }

        public PipelinedUnit LongAddUnit
        {
            get { return _longAddUnit; }
            set { _longAddUnit = value; }
        }

        public PipelinedUnit DivideUnit
        {
            get { return _divideUnit; }
            set { _divideUnit = value; }
        }

        public PipelinedUnit MultiplyUnit
        {
            get { return _multiplyUnit; }
            set { _multiplyUnit = value; }
        }

        public PipelinedUnit IncrementUnit
        {
            get { return _incrementUnit; }
            set { _incrementUnit = value; }
        }

        public Scoreboard()
        {
            
        }
    }
}
