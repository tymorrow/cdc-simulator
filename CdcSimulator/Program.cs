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
                new Instruction{},
                new Instruction{},
                new Instruction{}
            };

            // Create the machine object and pass it the instructions to run
            var cdc7600 = new CDC7600(instructions);
            var runTime = cdc7600.Run();

            // Display results to console.
            Console.WriteLine("Simulation Completed in: {0} clock cycles", runTime);
            Console.ReadKey();
        }
    }
}
