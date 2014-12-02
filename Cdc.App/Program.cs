namespace Cdc.App
{
    using Simulator6600;
    using Simulator7600;
    using System;

    class Program
    {
        /// <summary>
        /// Serves as the console application entry point.
        /// It creates instruction sets for both simulators and runs them.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Create instruction sets.
            var set6600 = new Simulator6600.InstructionSet();
            var set7600 = new Simulator7600.InstructionSet();

            // Create the CDC6600 machine object and pass it the instructions to run.
            var cdc6600 = new Cdc6600();
            cdc6600.AddInstructions(set6600.InstructionSet1);
            cdc6600.Run();

            // Create the CDC7600 machine object and pass it the instructions to run.
            var cdc7600 = new Cdc7600();
            cdc7600.AddInstructions(set7600.InstructionSet1);
            //cdc7600.Run();

            Console.ReadKey();
        }
    }
}
