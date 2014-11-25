namespace Cdc7600
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Machine
    {
        private List<Instruction> _instructions = new List<Instruction>();
        private readonly CentralProcessor _cpu = new CentralProcessor();
        private int _timeCounter;
        private int _instructionCounter;
        public int LastRunTime { get; private set; }

        public Machine()
        {
            LastRunTime = 0;
        }

        public void AddInstructions(List<Instruction> instructions)
        {
            foreach (var i in _instructions)
                i.IsFinished = false;
            _instructions = instructions;
        }

        public int Run()
        {
            // Return no time if no instructions exist
            if (!_instructions.Any()) return 0;
            _instructionCounter = 0;

            // Simulate clock cycles processing the instructions until their finished
            while (_instructions.Any(i => !i.IsFinished))
            {
                // Increment timer
                _timeCounter++;
                // Print time
                Console.WriteLine("Cycle: {0}", _timeCounter);

                
                if (_cpu.U3Register != null)
                {
                    // Process Instruction in U3 Register
                    _cpu.U3Register.IsFinished = true;
                    _cpu.U3Register = null;
                }

                if (_cpu.U3Register == null)
                {
                    ShiftRegisters();
                }

                // Print U Register contents
                PrintURegisters();
            }

            return _timeCounter;
        }

        private void ShiftRegisters()
        {
            _cpu.U3Register = _cpu.U2Register;
            _cpu.U2Register = _cpu.U1Register;
            _cpu.U1Register = _instructionCounter < _instructions.Count 
                ? _instructions[_instructionCounter] 
                : null;
            _instructionCounter++;
        }

        private void PrintURegisters()
        {
            var u1 = "\t";
            var u2 = "\t";
            var u3 = "\t";

            if (_cpu.U1Register != null) u1 = _cpu.U1Register.ToString();
            if (_cpu.U2Register != null) u2 = _cpu.U2Register.ToString();
            if (_cpu.U3Register != null) u3 = _cpu.U3Register.ToString();

            Console.WriteLine("U-Registers: {0}  \t->  {1}  \t->  {2}", u1, u2, u3);
        }
    }
}
