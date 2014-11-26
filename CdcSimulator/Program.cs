namespace CdcSimulator
{
    using Cdc7600;
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            // Create list of instructions
            var instructions = new List<Instruction>
            {
                new Instruction{OpCode = OpCode.FloatingDivide, Length = InstructionLength.Long},
                new Instruction{OpCode = OpCode.SumXjandBkToAi, Length = InstructionLength.Short},
                new Instruction{OpCode = OpCode.FloatingDivide, Length = InstructionLength.Long},
                new Instruction{OpCode = OpCode.ShiftXiLeftjkPlaces, Length = InstructionLength.Short},
                new Instruction{OpCode = OpCode.GoToKifXNegative, Length = InstructionLength.Long}
            };

            // Create the machine object and pass it the instructions to run
            var cdc7600 = new Machine();
            cdc7600.AddInstructions(instructions);
            var runTime = cdc7600.Run();

            // Display results to console.
            Console.WriteLine("Simulation Completed in: {0} clock cycles", runTime);
            Console.ReadKey();
        }
    }
}
