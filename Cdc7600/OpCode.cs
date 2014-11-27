namespace CdcMachines
{
    public enum OpCode
    {
        // Branch
        Stop = 00,
        ReturnJumpToK = 01,
        GoToKplusBi = 02,
        GoToKifXEqualsZero = 30,
        GoToKifXNotEqualsZero = 31,
        GoToKifXPositive = 32,
        GoToKifXNegative = 33,
        GoToKifInRange = 34,
        GoToKifXOutOfRange = 35,
        GoToKifXDefinite = 36,
        GoToKifXIndefinite = 37,
        GoToKifBiEqualsBj = 03,
        GoToKifBiNotEqualsBj = 04,
        GoToKifBiGreaterThanEqualToBj = 05,
        GoToKifBiLessThanBj = 06,

        // Boolean
        TransmitXjToXi = 10,
        LogicalProductXjandXkToXi = 11,
        LogicalSumXjandXkToXi = 12,
        LogicalDifferenceXjandXkToXi = 13,
        TransmitXjandXkComplementToXi = 14,
        LogicalProductXjandXkComplementToXi = 15,
        LogicalSumXjandXkComplementToXi = 16,
        LogicalDifferenceXjandXkComplementToXi = 17,

        // Shift
        ShiftXiLeftjkPlaces = 20,
        ShiftXiRightjkPlaces = 21,
        ShiftXiNominallyLeftBjPlaces = 22,
        ShiftXiNominallyRightBjPlaces = 23,
        NormalizeXkinXiandBj = 24,
        RoundNormalizeXkinXiandBj = 25,
        UnpackXktoXiandBj = 26,
        PackXifromXkandBj = 27,
        FormjkMaskinXi = 43,

        // Add
        FloatingSum = 300,
        FloatingDifference = 310,
        FloatingDpSum = 320,
        FloatingDpDifference = 330,
        RoundFloatingSum = 340,
        RoundFloatingDifference = 350,
        // Long Add
        IntegerSum = 360,
        IntegerDifference = 370,

        // Divide
        FloatingDivide = 44,
        RoundFloatingDivide = 45,
        Pass = 46,
        SumOfOnes = 47,

        // Multiply
        FloatingProduct = 40,
        RoundFloatingProduct = 41,
        FloatingDpProduct = 42,

        // Increment
        SumAjandKToAi = 50,
        SumBjandKToAi = 51,
        SumXjandKToAi = 52,
        SumXjandBkToAi = 53,
        SumAjandBkToAi = 54,
        DifferenceAjandBktoAi = 55,
        SumBjandBktoZi = 56,
        DifferenceBjandBktoZi = 57,
        SumAjandKToBi = 60,
        SumBjandKToBi = 61,
        SumXjandKToBi = 62,
        SumXjandBkToBi = 63,
        SumAjandBkToBi = 64,
        DifferenceAjandBktoBi = 65,
        SumBjandBktoBi = 66,
        DifferenceBjandBktoBi = 67,
        SumAjandKToXi = 70,
        SumBjandKToXi = 71,
        SumXjandKToXi = 72,
        SumXjandBkToXi = 73,
        SumAjandBkToXi = 74,
        DifferenceAjandBktoXi = 75,
        SumBjandBktoXi = 76,
        DifferenceBjandBktoXi = 77,
    }
}
