namespace CdcSimulator
{
    using System.Collections.Generic;
    using System.Linq;
    using Cdc7600;

    public class CDC7600
    {
        private readonly List<Instruction> _instructions = new List<Instruction>();
        private readonly CentralProcessor _cpu = new CentralProcessor();
        private int _timeCounter;
        public int LastRunTime { get; private set; }

        public CDC7600()
        {
            LastRunTime = 0;
        }
        public CDC7600(List<Instruction> instructions)
        {
            _instructions = instructions;
            LastRunTime = 0;
        }

        public int Run()
        {
            // Return no time if no instructions exist
            if (!_instructions.Any()) return 0;

            // Send all instructions to cpu in sequence.
            foreach (var i in _instructions)
            {
                // Increment timer
                _timeCounter++;

                // Put instruction into U registers.
                _cpu.U3Register.CurrentInstruction = _cpu.U2Register.CurrentInstruction;
                _cpu.U2Register.CurrentInstruction = _cpu.U1Register.CurrentInstruction;
                _cpu.U1Register.CurrentInstruction = i;

                // Update scoreboard

            }

            // Record the running time and return to simulator.
            LastRunTime = _timeCounter;
            return _timeCounter;
        }
    }
}
