namespace Cdc.App
{
    using System.IO;
    using System.Text;
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
            var sb = new StringBuilder();

            // Create instruction sets.
            var set6600 = new Simulator6600.InstructionSet();
            var set7600 = new Simulator7600.InstructionSet();

            // Create the CDC6600 machine object and pass it the instructions to run.
            var cdc6600 = new Cdc6600();
            sb.AppendLine("Executing Instruction Set 1");
            cdc6600.AddInstructions(set6600.InstructionSet1);
            sb.AppendLine(cdc6600.Run());
            sb.AppendLine("Executing Instruction Set 2");
            cdc6600.AddInstructions(set6600.InstructionSet2);
            sb.AppendLine(cdc6600.Run());
            sb.AppendLine("Executing Instruction Set 3");
            cdc6600.AddInstructions(set6600.InstructionSet3);
            sb.AppendLine(cdc6600.Run());

            // Create the CDC7600 machine object and pass it the instructions to run.
            var cdc7600 = new Cdc7600();
            sb.AppendLine("Executing Instruction Set 1");
            cdc7600.AddInstructions(set7600.InstructionSet1);
            sb.AppendLine(cdc7600.Run());
            sb.AppendLine("Executing Instruction Set 2");
            cdc7600.AddInstructions(set7600.InstructionSet2);
            sb.AppendLine(cdc7600.Run());
            sb.AppendLine("Executing Instruction Set 3");
            cdc7600.AddInstructions(set7600.InstructionSet3);
            sb.AppendLine(cdc7600.Run());

            // Output the results to a file.
            const string outputFolder = @"Results\";
            var folderPath = Path.Combine(Environment.CurrentDirectory, outputFolder);
            var exists = Directory.Exists(folderPath);
            if (!exists)
                Directory.CreateDirectory(folderPath);

            using (var outfile = new StreamWriter(Path.Combine(folderPath, DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt") + ".txt")))
            {
                outfile.Write(sb.ToString());
            }

            Console.ReadKey();
        }
    }
}
