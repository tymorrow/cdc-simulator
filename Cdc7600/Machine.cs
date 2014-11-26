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
        private int _timeCounter = -3;
        private int _instructionCounter;
        private const int FETCH_TIME = 4;
        private const int STORE_TIME = 5;

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

            // Simulate clock cycles processing the instructions until all are finished
            while (_instructions.Any(i => !i.IsFinished))
            {
                // Increment timer
                _timeCounter++;
                // Print time
                Console.WriteLine("Cycle: {0}", _timeCounter);

                // Process U* Registers
                if (_cpu.U3Register != null)
                {
                    // Find functional unit for this instruction


                    // Detect unit readiness


                    // Fill out schedule for instruction
                    _cpu.U3Register.Issue = _timeCounter;
                    _cpu.U3Register.Start = _timeCounter;
                    _cpu.U3Register.Result = _timeCounter + _cpu.TimingMap[_cpu.U3Register.OpCode];
                    _cpu.U3Register.UnitReady = _timeCounter + _cpu.UnitMap[_cpu.U3Register.OpCode].SegmentTime;

                    _cpu.U3Register.IsFinished = true;
                    // Skip a cycle if instruction is long
                    if(_cpu.U3Register.Length == InstructionLength.Long)
                    _timeCounter++;
                    _cpu.U3Register = null;
                    
                }

                if (_cpu.U3Register == null)
                {
                    ShiftRegisters();
                }

                PrintURegisters();
            }

            PrintSchedule();
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

        private void PrintSchedule()
        {
            Console.WriteLine();
            Console.WriteLine("====================== Timing Schedule ======================");
            Console.WriteLine("Code\tLength\tIssue\tStart\tResult\tUnit\tFetch\tStore");
            foreach (var i in _instructions)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                    (int)i.OpCode,
                    i.Length.ToString()[0],
                    i.Issue,
                    i.Start,
                    i.Result,
                    i.UnitReady,
                    i.Fetch,
                    i.Store);
            }
            Console.WriteLine();
        }
    }

    public enum Register
    {
        K,
        X0, 
        X1, // Read
        X2, // Read
        X3, // Read
        X4, // Read
        X5, // Read
        X6, // Write
        X7, // Write
        A0,
        A1, // Read
        A2, // Read
        A3, // Read
        A4, // Read
        A5, // Read
        A6, // Write
        A7, // Write
        B0,
        B1, // Read
        B2, // Read
        B3, // Read
        B4, // Read
        B5, // Read
        B6, // Write
        B7, // Write
    }
}
