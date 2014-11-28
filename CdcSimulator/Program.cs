namespace CdcSimulator
{
    using CdcMachines;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var set1 = new InstructionSet();
            var set2 = new InstructionSet();

            // Create the CDC6600 machine object and pass it the instructions to run
            var cdc6600 = new Cdc6600();
            cdc6600.AddInstructions(set2.InstructionSet1);
            cdc6600.Run();

            // Create the CDC7600 machine object and pass it the instructions to run
            var cdc7600 = new Cdc7600();
            cdc7600.AddInstructions(set1.InstructionSet1);
            //cdc7600.Run();

            Console.ReadKey();
        }
    }
}
