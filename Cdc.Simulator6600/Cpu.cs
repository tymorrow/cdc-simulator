namespace Cdc.Simulator6600
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores information related to the system such as the Scoreboard 
    /// (stores Functional Unit information), a timing map (maps OpCodes to 
    /// clock cycles), functional unit map (maps Opcodes to Units), 
    /// a register-value map (maps Register enums to a value), and U registers,
    /// which store instructions.
    /// </summary>
    public class Cpu
    {
        public List<FunctionalUnit> Scoreboard { get; set; }
        public Dictionary<OpCode, int> TimingMap { get; set; }
        public Dictionary<OpCode, UnitType> UnitMap { get; set; }
        public Dictionary<Register, int> Registers { get; set; } 

        public Instruction U1 { get; set; }
        public Instruction U2 { get; set; }
        public Instruction U3 { get; set; }

        public Cpu()
        {
            UnitMap = new Dictionary<OpCode, UnitType>
            {
                #region Values
                // Branch
                {OpCode.Stop, UnitType.Branch},
                {OpCode.ReturnJumpToK, UnitType.Branch},
                {OpCode.GoToKplusBi, UnitType.Branch},
                {OpCode.GoToKifXEqualsZero, UnitType.Branch},
                {OpCode.GoToKifXNotEqualsZero, UnitType.Branch},
                {OpCode.GoToKifXPositive, UnitType.Branch},
                {OpCode.GoToKifXNegative, UnitType.Branch},
                {OpCode.GoToKifInRange, UnitType.Branch},
                {OpCode.GoToKifXOutOfRange, UnitType.Branch},
                {OpCode.GoToKifXDefinite, UnitType.Branch},
                {OpCode.GoToKifXIndefinite, UnitType.Branch},
                {OpCode.GoToKifBiEqualsBj, UnitType.Branch},
                {OpCode.GoToKifBiNotEqualsBj, UnitType.Branch},
                {OpCode.GoToKifBiGreaterThanEqualToBj, UnitType.Branch},
                {OpCode.GoToKifBiLessThanBj, UnitType.Branch},
                // Boolean
                {OpCode.TransmitXjToXi, UnitType.Boolean},
                {OpCode.LogicalProductXjandXkToXi, UnitType.Boolean},
                {OpCode.LogicalSumXjandXkToXi, UnitType.Boolean},
                {OpCode.LogicalDifferenceXjandXkToXi, UnitType.Boolean},
                {OpCode.TransmitXjandXkComplementToXi, UnitType.Boolean},
                {OpCode.LogicalProductXjandXkComplementToXi, UnitType.Boolean},
                {OpCode.LogicalSumXjandXkComplementToXi, UnitType.Boolean},
                {OpCode.LogicalDifferenceXjandXkComplementToXi, UnitType.Boolean},
                // Shift
                {OpCode.ShiftXiLeftjkPlaces, UnitType.Shift},
                {OpCode.ShiftXiRightjkPlaces, UnitType.Shift},
                {OpCode.ShiftXiNominallyLeftBjPlaces, UnitType.Shift},
                {OpCode.ShiftXiNominallyRightBjPlaces, UnitType.Shift},
                {OpCode.NormalizeXkinXiandBj, UnitType.Shift},
                {OpCode.RoundNormalizeXkinXiandBj, UnitType.Shift},
                {OpCode.UnpackXktoXiandBj, UnitType.Shift},
                {OpCode.PackXifromXkandBj, UnitType.Shift},
                {OpCode.FormjkMaskinXi, UnitType.Shift},
                // Add
                {OpCode.FloatingSum, UnitType.Add},
                {OpCode.FloatingDifference, UnitType.Add},
                {OpCode.FloatingDpSum, UnitType.Add},
                {OpCode.FloatingDpDifference, UnitType.Add},
                {OpCode.RoundFloatingSum, UnitType.Add},
                {OpCode.RoundFloatingDifference, UnitType.Add},
                // Long Add
                {OpCode.IntegerSum, UnitType.LongAdd},
                {OpCode.IntegerDifference, UnitType.LongAdd},
                // Divide
                {OpCode.FloatingDivide, UnitType.Divide},
                {OpCode.RoundFloatingDivide, UnitType.Divide},
                {OpCode.Pass, UnitType.Divide},
                {OpCode.SumOfOnes, UnitType.Divide},
                // Multiply
                {OpCode.FloatingProduct, UnitType.Multiply},
                {OpCode.RoundFloatingProduct, UnitType.Multiply},
                {OpCode.FloatingDpProduct, UnitType.Multiply},
                // Increment
                {OpCode.SumAjandKToAi, UnitType.Increment},
                {OpCode.SumBjandKToAi, UnitType.Increment},
                {OpCode.SumXjandKToAi, UnitType.Increment},
                {OpCode.SumXjandBkToAi, UnitType.Increment},
                {OpCode.SumAjandBkToAi, UnitType.Increment},
                {OpCode.DifferenceAjandBktoAi, UnitType.Increment},
                {OpCode.SumBjandBktoZi, UnitType.Increment},
                {OpCode.DifferenceBjandBktoZi, UnitType.Increment},
                {OpCode.SumAjandKToBi, UnitType.Increment},
                {OpCode.SumBjandKToBi, UnitType.Increment},
                {OpCode.SumXjandKToBi, UnitType.Increment},
                {OpCode.SumXjandBkToBi, UnitType.Increment},
                {OpCode.SumAjandBkToBi, UnitType.Increment},
                {OpCode.DifferenceAjandBktoBi, UnitType.Increment},
                {OpCode.SumBjandBktoBi, UnitType.Increment},
                {OpCode.DifferenceBjandBktoBi, UnitType.Increment},
                {OpCode.SumAjandKToXi, UnitType.Increment},
                {OpCode.SumBjandKToXi, UnitType.Increment},
                {OpCode.SumXjandKToXi, UnitType.Increment},
                {OpCode.SumXjandBkToXi, UnitType.Increment},
                {OpCode.SumAjandBkToXi, UnitType.Increment},
                {OpCode.DifferenceAjandBktoXi, UnitType.Increment},
                {OpCode.SumBjandBktoXi, UnitType.Increment},
                {OpCode.DifferenceBjandBktoXi, UnitType.Increment},
                #endregion
            };
            TimingMap = new Dictionary<OpCode, int>
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
            Scoreboard = new List<FunctionalUnit>
            {
                #region Functional Units
                new FunctionalUnit(UnitType.Branch),
                new FunctionalUnit(UnitType.Boolean),
                new FunctionalUnit(UnitType.Shift),
                new FunctionalUnit(UnitType.Add),
                new FunctionalUnit(UnitType.LongAdd),
                new FunctionalUnit(UnitType.Divide),
                new FunctionalUnit(UnitType.Multiply),
                new FunctionalUnit(UnitType.Multiply),
                new FunctionalUnit(UnitType.Increment),
                new FunctionalUnit(UnitType.Increment)
                #endregion
            };
        }
    }
}
