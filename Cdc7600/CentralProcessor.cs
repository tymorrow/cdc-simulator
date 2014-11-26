namespace Cdc7600
{
    using System.Collections.Generic;

    public class CentralProcessor
    {
        public static Scoreboard Scoreboard = new Scoreboard();
        public Dictionary<OpCode, int> TimingMap = new Dictionary<OpCode, int>
        {
            #region Values
            // Branch
            {OpCode.Stop, 0},
            {OpCode.ReturnJumpToK, 14},
            {OpCode.GoToKplusBi, 14},
            {OpCode.GoToKifXEqualsZero, 9},
            {OpCode.GoToKifXNotEqualsZero, 9},
            {OpCode.GoToKifXPositive, 9},
            {OpCode.GoToKifXNegative, 9},
            {OpCode.GoToKifInRange, 9},
            {OpCode.GoToKifXOutOfRange, 9},
            {OpCode.GoToKifXDefinite, 9},
            {OpCode.GoToKifXIndefinite, 9},
            {OpCode.GoToKifBiEqualsBj, 8},
            {OpCode.GoToKifBiNotEqualsBj, 8},
            {OpCode.GoToKifBiGreaterThanEqualToBj, 8},
            {OpCode.GoToKifBiLessThanBj, 8},
            // Boolean
            {OpCode.TransmitXjToXi, 3},
            {OpCode.LogicalProductXjandXkToXi, 3},
            {OpCode.LogicalSumXjandXkToXi, 3},
            {OpCode.LogicalDifferenceXjandXkToXi, 3},
            {OpCode.TransmitXjandXkComplementToXi, 3},
            {OpCode.LogicalProductXjandXkComplementToXi, 3},
            {OpCode.LogicalSumXjandXkComplementToXi, 3},
            {OpCode.LogicalDifferenceXjandXkComplementToXi, 3},
            // Shift
            {OpCode.ShiftXiLeftjkPlaces, 3},
            {OpCode.ShiftXiRightjkPlaces, 3},
            {OpCode.ShiftXiNominallyLeftBjPlaces, 3},
            {OpCode.ShiftXiNominallyRightBjPlaces, 3},
            {OpCode.NormalizeXkinXiandBj, 4},
            {OpCode.RoundNormalizeXkinXiandBj, 4},
            {OpCode.UnpackXktoXiandBj, 3},
            {OpCode.PackXifromXkandBj, 3},
            {OpCode.FormjkMaskinXi, 3},
            // Add
            {OpCode.FloatingSum, 4},
            {OpCode.FloatingDifference, 4},
            {OpCode.FloatingDpSum, 4},
            {OpCode.FloatingDpDifference, 4},
            {OpCode.RoundFloatingSum, 4},
            {OpCode.RoundFloatingDifference, 4},
            // Long Add
            {OpCode.IntegerSum, 3},
            {OpCode.IntegerDifference, 3},
            // Divide
            {OpCode.FloatingDivide, 29},
            {OpCode.RoundFloatingDivide, 29},
            {OpCode.Pass, 0},
            {OpCode.SumOfOnes, 8},
            // Multiply
            {OpCode.FloatingProduct, 10},
            {OpCode.RoundFloatingProduct, 10},
            {OpCode.FloatingDpProduct, 10},
            // Increment
            {OpCode.SumAjandKToAi,3},
            {OpCode.SumBjandKToAi, 3},
            {OpCode.SumXjandKToAi, 3},
            {OpCode.SumXjandBkToAi, 3},
            {OpCode.SumAjandBkToAi, 3},
            {OpCode.DifferenceAjandBktoAi, 3},
            {OpCode.SumBjandBktoZi, 3},
            {OpCode.DifferenceBjandBktoZi, 3},
            {OpCode.SumAjandKToBi, 3},
            {OpCode.SumBjandKToBi, 3},
            {OpCode.SumXjandKToBi, 3},
            {OpCode.SumXjandBkToBi, 3},
            {OpCode.SumAjandBkToBi, 3},
            {OpCode.DifferenceAjandBktoBi, 3},
            {OpCode.SumBjandBktoBi, 3},
            {OpCode.DifferenceBjandBktoBi, 3},
            {OpCode.SumAjandKToXi, 3},
            {OpCode.SumBjandKToXi, 3},
            {OpCode.SumXjandKToXi, 3},
            {OpCode.SumXjandBkToXi, 3},
            {OpCode.SumAjandBkToXi, 3},
            {OpCode.DifferenceAjandBktoXi, 3},
            {OpCode.SumBjandBktoXi, 3},
            {OpCode.DifferenceBjandBktoXi, 3},
            #endregion
        };

        public Dictionary<OpCode, PipelinedUnit> UnitMap = new Dictionary<OpCode, PipelinedUnit>
        {
            #region Values
            // Branch
            {OpCode.Stop, Scoreboard.BranchUnit},
            {OpCode.ReturnJumpToK, Scoreboard.BranchUnit},
            {OpCode.GoToKplusBi, Scoreboard.BranchUnit},
            {OpCode.GoToKifXEqualsZero, Scoreboard.BranchUnit},
            {OpCode.GoToKifXNotEqualsZero, Scoreboard.BranchUnit},
            {OpCode.GoToKifXPositive, Scoreboard.BranchUnit},
            {OpCode.GoToKifXNegative, Scoreboard.BranchUnit},
            {OpCode.GoToKifInRange, Scoreboard.BranchUnit},
            {OpCode.GoToKifXOutOfRange, Scoreboard.BranchUnit},
            {OpCode.GoToKifXDefinite, Scoreboard.BranchUnit},
            {OpCode.GoToKifXIndefinite, Scoreboard.BranchUnit},
            {OpCode.GoToKifBiEqualsBj, Scoreboard.BranchUnit},
            {OpCode.GoToKifBiNotEqualsBj, Scoreboard.BranchUnit},
            {OpCode.GoToKifBiGreaterThanEqualToBj, Scoreboard.BranchUnit},
            {OpCode.GoToKifBiLessThanBj, Scoreboard.BranchUnit},
            // Boolean
            {OpCode.TransmitXjToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalProductXjandXkToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalSumXjandXkToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalDifferenceXjandXkToXi, Scoreboard.BooleanUnit},
            {OpCode.TransmitXjandXkComplementToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalProductXjandXkComplementToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalSumXjandXkComplementToXi, Scoreboard.BooleanUnit},
            {OpCode.LogicalDifferenceXjandXkComplementToXi, Scoreboard.BooleanUnit},
            // Shift
            {OpCode.ShiftXiLeftjkPlaces, Scoreboard.ShiftUnit},
            {OpCode.ShiftXiRightjkPlaces, Scoreboard.ShiftUnit},
            {OpCode.ShiftXiNominallyLeftBjPlaces, Scoreboard.ShiftUnit},
            {OpCode.ShiftXiNominallyRightBjPlaces, Scoreboard.ShiftUnit},
            {OpCode.NormalizeXkinXiandBj, Scoreboard.ShiftUnit},
            {OpCode.RoundNormalizeXkinXiandBj, Scoreboard.ShiftUnit},
            {OpCode.UnpackXktoXiandBj, Scoreboard.ShiftUnit},
            {OpCode.PackXifromXkandBj, Scoreboard.ShiftUnit},
            {OpCode.FormjkMaskinXi, Scoreboard.ShiftUnit},
            // Add
            {OpCode.FloatingSum, Scoreboard.AddUnit},
            {OpCode.FloatingDifference, Scoreboard.AddUnit},
            {OpCode.FloatingDpSum, Scoreboard.AddUnit},
            {OpCode.FloatingDpDifference, Scoreboard.AddUnit},
            {OpCode.RoundFloatingSum, Scoreboard.AddUnit},
            {OpCode.RoundFloatingDifference, Scoreboard.AddUnit},
            // Long Add
            {OpCode.IntegerSum, Scoreboard.LongAddUnit},
            {OpCode.IntegerDifference, Scoreboard.LongAddUnit},
            // Divide
            {OpCode.FloatingDivide, Scoreboard.DivideUnit},
            {OpCode.RoundFloatingDivide, Scoreboard.DivideUnit},
            {OpCode.Pass, Scoreboard.DivideUnit},
            {OpCode.SumOfOnes, Scoreboard.DivideUnit},
            // Multiply
            {OpCode.FloatingProduct, Scoreboard.MultiplyUnit},
            {OpCode.RoundFloatingProduct, Scoreboard.MultiplyUnit},
            {OpCode.FloatingDpProduct, Scoreboard.MultiplyUnit},
            // Increment
            {OpCode.SumAjandKToAi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandKToAi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandKToAi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandBkToAi, Scoreboard.IncrementUnit},
            {OpCode.SumAjandBkToAi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceAjandBktoAi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandBktoZi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceBjandBktoZi, Scoreboard.IncrementUnit},
            {OpCode.SumAjandKToBi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandKToBi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandKToBi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandBkToBi, Scoreboard.IncrementUnit},
            {OpCode.SumAjandBkToBi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceAjandBktoBi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandBktoBi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceBjandBktoBi, Scoreboard.IncrementUnit},
            {OpCode.SumAjandKToXi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandKToXi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandKToXi, Scoreboard.IncrementUnit},
            {OpCode.SumXjandBkToXi, Scoreboard.IncrementUnit},
            {OpCode.SumAjandBkToXi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceAjandBktoXi, Scoreboard.IncrementUnit},
            {OpCode.SumBjandBktoXi, Scoreboard.IncrementUnit},
            {OpCode.DifferenceBjandBktoXi, Scoreboard.IncrementUnit},
            #endregion
        };

        public Instruction U1Register;
        public Instruction U2Register;
        public Instruction U3Register;

        public CentralProcessor()
        {

        }
    }
}
