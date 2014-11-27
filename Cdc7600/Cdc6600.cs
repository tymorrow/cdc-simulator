namespace CdcMachines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Cdc6600
    {
        private List<Instruction> _instructions = new List<Instruction>();
        private readonly Cdc6600Cpu _cpu = new Cdc6600Cpu();
        private int _timeCounter = -3;
        private int _instructionCounter;
        private int _lastWordStart;
        private const int NEW_WORD_TIME = 8;
        private const int FETCH_TIME = 4;
        private const int STORE_TIME = 5;

        public void AddInstructions(List<Instruction> instructions)
        {
            foreach (var i in _instructions)
                i.IsFinished = false;
            _instructions = instructions;
        }
        public void Run()
        {
            // Return no time if no instructions exist
            if (!_instructions.Any()) return;
            _instructionCounter = 0;

            // Simulate clock cycles processing the instructions until all are finished
            while (_instructions.Any(i => !i.IsFinished))
            {
                // Increment clock cycle timer
                _timeCounter++;

                // PrintTime();

                // Process U Registers
                if (_cpu.U3 != null)
                {
                    var newWordComing = _cpu.U3.IsEndOfWord;
                    // Clear the pipelines of completed instructions
                    UpdateScoreboard();

                    // See if the next instruction can be added to its functional unit
                    AttemptToProcessNextInstruction();

                    // Detect end of word and increment time accordingly
                    if (newWordComing)
                        _timeCounter = _lastWordStart + NEW_WORD_TIME - 1;
                }

                if (_cpu.U3 == null)
                {
                    ShiftRegisters();
                }
            }

            PrintSchedule();
        }

        private void AttemptToProcessNextInstruction()
        {
            // Find functional unit for this instruction
            var unitType = _cpu.UnitMap[_cpu.U3.OpCode];
            var unit = _cpu.Scoreboard.Single(u => u.Type == unitType);

            // Check for first order conflict
            if (!unit.IsReady(_timeCounter)) return;

            // Check for second order conflict
            foreach(var fu in _cpu.Scoreboard)
            {
                
            }
            // Check for third order conflict
            foreach (var fu in _cpu.Scoreboard)
            {
                
            }

            // No conflict; issue/start instruction immediately
            // TODO

            // Fill out schedule for instruction
            _cpu.U3.Issue = _timeCounter;
            _cpu.U3.Start = _timeCounter;
            _cpu.U3.Result = _timeCounter + _cpu.TimingMap[_cpu.U3.OpCode];
            CalculateU3StoreFetchTiming();
            _cpu.U3.UnitReady = _cpu.U3.Result + 1;
            _cpu.U3.IsFinished = true;

            // Skip a cycle if instruction is long
            if (_cpu.U3.Length == InstructionLength.Long)
                _timeCounter++;
            _cpu.U3 = null;
        }
        private void CalculateU3StoreFetchTiming()
        {
            // Calculate fetch/store timing if necessary
            if (_cpu.U3.OpCode < OpCode.SumAjandKToAi && _cpu.U3.OpCode > OpCode.DifferenceBjandBktoXi) return;

            if (_cpu.U3.Operand1 >= Register.A1 && _cpu.U3.Operand1 <= Register.A5) // Read from Memory
            {
                _cpu.U3.Fetch = _cpu.U3.Result + FETCH_TIME;
            }
            else if (_cpu.U3.Operand1 >= Register.A6 && _cpu.U3.Operand1 <= Register.A7) // Write to Memory
            {
                _cpu.U3.Store = _cpu.U3.Result + STORE_TIME;
            }
        }
        private void UpdateScoreboard()
        {
            foreach(var unit in _cpu.Scoreboard)
            {
                
            }
        }
        private void ShiftRegisters()
        {
            if (_cpu.U2 != null && _cpu.U2.IsStartOfWord)
                _lastWordStart = _timeCounter + 1;
                
            _cpu.U3 = _cpu.U2;
            _cpu.U2 = _cpu.U1;
            _cpu.U1 = _instructionCounter < _instructions.Count 
                ? _instructions[_instructionCounter] 
                : null;
            _instructionCounter++;
        }

        private void PrintTime()
        {
            Console.WriteLine("Cycle: {0}", _timeCounter);
        }
        private void PrintURegisters()
        {
            var u1 = "\t";
            var u2 = "\t";
            var u3 = "\t";

            if (_cpu.U1 != null) u1 = _cpu.U1.ToString();
            if (_cpu.U2 != null) u2 = _cpu.U2.ToString();
            if (_cpu.U3 != null) u3 = _cpu.U3.ToString();

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
}
