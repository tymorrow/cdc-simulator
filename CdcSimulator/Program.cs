namespace CdcSimulator
{
    using Cdc7600;
    using System;
    using System.Collections.Generic;

    class Program
    {
        #region Instruction Sets
        // AX^2 + B
        private static readonly List<Instruction> InstructionSet1 = new List<Instruction>
        {
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.X1
            }, // Fetch X
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A2,
                Operand2 = Register.K,
                OutputRegister = Register.X2
            }, // Fetch A
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X1,
                Operand2 = Register.X1,
                OutputRegister = Register.X0
            }, // Form X^2
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X0,
                Operand2 = Register.X2,
                OutputRegister = Register.X6
            }, // Form AX^2
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A3,
                Operand2 = Register.K,
                OutputRegister = Register.X3
            }, // Fetch B
            new Instruction
            {
                OpCode = OpCode.FloatingSum,   
                Length = InstructionLength.Short,
                Operand1 = Register.X6,
                Operand2 = Register.X3,
                OutputRegister = Register.X7
            }, // Form Y
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.X7
            }, // Store Y
        }; 
        // AX^2 + BX + C
        private static readonly List<Instruction> InstructionSet2 = new List<Instruction>
        {

        }; 
        // AX^2 + BX + C (X and Y are vectors of n elements, where n = 5)
        private static readonly List<Instruction> InstructionSet3 = new List<Instruction>
        {

        }; 
        #endregion

        static void Main(string[] args)
        {
            // Create the machine object and pass it the instructions to run
            var cdc7600 = new Machine();
            cdc7600.AddInstructions(InstructionSet1);
            var runTime = cdc7600.Run();

            // Display results to console.
            Console.WriteLine("Simulation Completed in: {0} clock cycles", runTime);
            Console.ReadKey();
        }
    }
}
