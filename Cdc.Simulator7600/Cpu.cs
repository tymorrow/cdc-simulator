namespace Cdc.Simulator7600
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Stores information related to the system such as the Scoreboard 
    /// (stores Functional Unit information), a timing map (maps OpCodes to 
    /// clock cycles), functional unit map (maps Opcodes to Units), 
    /// a register-value map (maps Register enums to a value), and U registers,
    /// which store instructions.
    /// </summary>
    public class Cpu
    {
        public List<PipelinedUnit> Scoreboard { get; set; }
        public Dictionary<OpCode, int> TimingMap { get; set; }
        public Dictionary<OpCode, UnitType> UnitMap { get; set; }
        public Dictionary<Register, int> Registers { get; set; }

        public Instruction U1 { get; set; }
        public Instruction U2 { get; set; }
        public Instruction U3 { get; set; }

        public Cpu()
        {
            Scoreboard = new List<PipelinedUnit>
            {
                #region Functional Units
                new PipelinedUnit(1, UnitType.Normalize),
                new PipelinedUnit(1, UnitType.Boolean),
                new PipelinedUnit(1, UnitType.Shift),
                new PipelinedUnit(1, UnitType.FloatingAdd),
                new PipelinedUnit(1, UnitType.FixedAdd),
                new PipelinedUnit(18, UnitType.FloatingDivide),
                new PipelinedUnit(2, UnitType.FloatingMultiply),
                new PipelinedUnit(1, UnitType.Increment)
                #endregion
            };
            TimingMap = new Dictionary<OpCode, int>
            {
                #region Values
                // Normalize
                {OpCode.Stop, 0},
                {OpCode.ReturnJumpToK, 9},
                {OpCode.GoToKplusBi, 9},
                {OpCode.GoToKifXEqualsZero, 4},
                {OpCode.GoToKifXNotEqualsZero, 4},
                {OpCode.GoToKifXPositive, 4},
                {OpCode.GoToKifXNegative, 4},
                {OpCode.GoToKifInRange, 4},
                {OpCode.GoToKifXOutOfRange, 4},
                {OpCode.GoToKifXDefinite, 4},
                {OpCode.GoToKifXIndefinite, 4},
                {OpCode.GoToKifBiEqualsBj, 3},
                {OpCode.GoToKifBiNotEqualsBj, 3},
                {OpCode.GoToKifBiGreaterThanEqualToBj, 3},
                {OpCode.GoToKifBiLessThanBj, 3},
                // Boolean
                {OpCode.TransmitXjToXi, 2},
                {OpCode.LogicalProductXjandXkToXi, 2},
                {OpCode.LogicalSumXjandXkToXi, 2},
                {OpCode.LogicalDifferenceXjandXkToXi, 2},
                {OpCode.TransmitXjandXkComplementToXi, 2},
                {OpCode.LogicalProductXjandXkComplementToXi, 2},
                {OpCode.LogicalSumXjandXkComplementToXi, 2},
                {OpCode.LogicalDifferenceXjandXkComplementToXi, 2},
                // Shift
                {OpCode.ShiftXiLeftjkPlaces, 2},
                {OpCode.ShiftXiRightjkPlaces, 2},
                {OpCode.ShiftXiNominallyLeftBjPlaces, 2},
                {OpCode.ShiftXiNominallyRightBjPlaces, 2},
                {OpCode.NormalizeXkinXiandBj, 3},
                {OpCode.RoundNormalizeXkinXiandBj, 3},
                {OpCode.UnpackXktoXiandBj, 2},
                {OpCode.PackXifromXkandBj, 2},
                {OpCode.FormjkMaskinXi, 2},
                // FloatingAdd
                {OpCode.FloatingSum, 4},
                {OpCode.FloatingDifference, 4},
                {OpCode.FloatingDpSum, 4},
                {OpCode.FloatingDpDifference, 4},
                {OpCode.RoundFloatingSum, 4},
                {OpCode.RoundFloatingDifference, 4},
                // FixedAdd
                {OpCode.IntegerSum, 2},
                {OpCode.IntegerDifference, 2},
                // FloatingDivide
                {OpCode.FloatingDivide, 20},
                {OpCode.RoundFloatingDivide, 20},
                {OpCode.Pass, 0},
                {OpCode.SumOfOnes, 8},
                // FloatingMultiply
                {OpCode.FloatingProduct, 5},
                {OpCode.RoundFloatingProduct, 5},
                {OpCode.FloatingDpProduct, 5},
                // Increment
                {OpCode.SumAjandKToAi, 2},
                {OpCode.SumBjandKToAi, 2},
                {OpCode.SumXjandKToAi, 2},
                {OpCode.SumXjandBkToAi, 2},
                {OpCode.SumAjandBkToAi, 2},
                {OpCode.DifferenceAjandBktoAi, 2},
                {OpCode.SumBjandBktoZi, 2},
                {OpCode.DifferenceBjandBktoZi, 2},
                {OpCode.SumAjandKToBi, 2},
                {OpCode.SumBjandKToBi, 2},
                {OpCode.SumXjandKToBi, 2},
                {OpCode.SumXjandBkToBi, 2},
                {OpCode.SumAjandBkToBi, 2},
                {OpCode.DifferenceAjandBktoBi, 2},
                {OpCode.SumBjandBktoBi, 2},
                {OpCode.DifferenceBjandBktoBi, 2},
                {OpCode.SumAjandKToXi, 2},
                {OpCode.SumBjandKToXi, 2},
                {OpCode.SumXjandKToXi, 2},
                {OpCode.SumXjandBkToXi, 2},
                {OpCode.SumAjandBkToXi, 2},
                {OpCode.DifferenceAjandBktoXi, 2},
                {OpCode.SumBjandBktoXi, 2},
                {OpCode.DifferenceBjandBktoXi, 2},
                #endregion
            };
            UnitMap = new Dictionary<OpCode, UnitType>
            {
                #region Values
                // Normalize
                {OpCode.Stop, UnitType.Normalize},
                {OpCode.ReturnJumpToK, UnitType.Normalize},
                {OpCode.GoToKplusBi, UnitType.Normalize},
                {OpCode.GoToKifXEqualsZero, UnitType.Normalize},
                {OpCode.GoToKifXNotEqualsZero, UnitType.Normalize},
                {OpCode.GoToKifXPositive, UnitType.Normalize},
                {OpCode.GoToKifXNegative, UnitType.Normalize},
                {OpCode.GoToKifInRange, UnitType.Normalize},
                {OpCode.GoToKifXOutOfRange, UnitType.Normalize},
                {OpCode.GoToKifXDefinite, UnitType.Normalize},
                {OpCode.GoToKifXIndefinite, UnitType.Normalize},
                {OpCode.GoToKifBiEqualsBj, UnitType.Normalize},
                {OpCode.GoToKifBiNotEqualsBj, UnitType.Normalize},
                {OpCode.GoToKifBiGreaterThanEqualToBj, UnitType.Normalize},
                {OpCode.GoToKifBiLessThanBj, UnitType.Normalize},
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
                // FloatingAdd
                {OpCode.FloatingSum, UnitType.FloatingAdd},
                {OpCode.FloatingDifference, UnitType.FloatingAdd},
                {OpCode.FloatingDpSum, UnitType.FloatingAdd},
                {OpCode.FloatingDpDifference, UnitType.FloatingAdd},
                {OpCode.RoundFloatingSum, UnitType.FloatingAdd},
                {OpCode.RoundFloatingDifference, UnitType.FloatingAdd},
                // FixedAdd
                {OpCode.IntegerSum, UnitType.FixedAdd},
                {OpCode.IntegerDifference, UnitType.FixedAdd},
                // FloatingDivide
                {OpCode.FloatingDivide, UnitType.FloatingDivide},
                {OpCode.RoundFloatingDivide, UnitType.FloatingDivide},
                {OpCode.Pass, UnitType.FloatingDivide},
                {OpCode.SumOfOnes, UnitType.FloatingDivide},
                // FloatingMultiply
                {OpCode.FloatingProduct, UnitType.FloatingMultiply},
                {OpCode.RoundFloatingProduct, UnitType.FloatingMultiply},
                {OpCode.FloatingDpProduct, UnitType.FloatingMultiply},
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
            // Initialize all register values to zero.
            Registers = new Dictionary<Register, int>();
            var registerTypes = Enum.GetValues(typeof(Register)).Cast<Register>();
            foreach (var e in registerTypes)
            {
                Registers.Add(e, 0);
            }
        }

        /// <summary>
        /// Resets the state of the _cpu for a completely new set
        /// of instructions.
        /// </summary>
        public void Reset()
        {
            U1 = U2 = U3 = null;
            var registerTypes = Enum.GetValues(typeof(Register)).Cast<Register>();
            foreach (var e in registerTypes)
            {
                Registers[e] = 0;
            }
        }
    }
}
