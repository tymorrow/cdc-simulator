namespace Cdc7600
{
    using System.Collections.Generic;
    using System.Linq;

    public class Machine
    {
        private List<Instruction> _instructions = new List<Instruction>();
        private readonly CentralProcessor _cpu = new CentralProcessor();
        private int _timeCounter;
        public int LastRunTime { get; private set; }

        public Machine()
        {
            LastRunTime = 0;
        }

        public void AddInstructions(List<Instruction> instructions)
        {
            _instructions = instructions;
        }

        public int Run()
        {
            // Return no time if no instructions exist
            if (!_instructions.Any()) return 0;

            // Create blank schedule


            // Send all instructions to cpu in sequence.
            foreach (var i in _instructions)
            {
                // Increment timer
                _timeCounter++;

                // Put instruction into U* registers.
                _cpu.U3Register = _cpu.U2Register;
                _cpu.U2Register = _cpu.U1Register;
                _cpu.U1Register = i;

                // Update scoreboard

            }

            // Record the running time and return to simulator.
            LastRunTime = _timeCounter;
            return _timeCounter;
        }
    }
}
