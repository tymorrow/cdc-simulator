namespace CdcMachines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Cdc6600
    {
        private List<Instruction> _instructions = new List<Instruction>();
        private readonly CentralProcessor _cpu = new CentralProcessor();
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

        public int Run()
        {
            // Return no time if no instructions exist
            if (!_instructions.Any()) return 0;
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
            return _timeCounter;
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
                foreach(var i in fu.Pipeline)
                {
                    if(i.OutputRegister == _cpu.U3.Operand1 || 
                       i.OutputRegister == _cpu.U3.Operand2)
                    {
                        // Reserve unit and issue instruction, but delay execution
                        unit.IsReserved = true;
                        unit.Pipeline.Enqueue(_cpu.U3);
                        unit.LastStart = i.Result;

                        _cpu.U3.Issue = _timeCounter;
                        // Calculate start time
                        if(i.OpCode >= OpCode.SumAjandKToAi || i.OpCode <= OpCode.DifferenceBjandBktoXi) // Increment
                        {
                                
                            if(i.Operand1 >= Register.A1 && i.Operand1 <= Register.A5) // Read from Memory
                            {
                                _cpu.U3.Start = i.Fetch ?? 0;
                            }
                            else if(i.Operand1 >= Register.A1 && i.Operand1 <= Register.A5) // Write to Memory
                            {
                                _cpu.U3.Start = i.Store ?? 0;
                            }
                        }
                        else
                        {
                            _cpu.U3.Start = i.Result;
                        }
                        // Calculate when result will be generated
                        _cpu.U3.Result = _cpu.U3.Start + _cpu.TimingMap[_cpu.U3.OpCode];
                        CalculateU3StoreFetchTiming();
                        _cpu.U3.UnitReady = i.Result + unit.SegmentTime;
                        _cpu.U3.IsFinished = true;

                        if (_cpu.U3.Length == InstructionLength.Long)
                            _timeCounter++;
                        _cpu.U3 = null;
                        return;
                    }
                }
            }
            // Check for third order conflict
            foreach (var fu in _cpu.Scoreboard)
            {
                foreach (var i in fu.Pipeline)
                {
                    if (i.Operand1 == _cpu.U3.OutputRegister ||
                        i.Operand2 == _cpu.U3.OutputRegister)
                    {
                        // Issue/Start execution but hold result until conflict resolved
                        unit.Pipeline.Enqueue(_cpu.U3);
                        unit.LastStart = _timeCounter;

                        _cpu.U3.Issue = _timeCounter;
                        _cpu.U3.Start = _timeCounter;
                        _cpu.U3.Result = i.Result;
                        CalculateU3StoreFetchTiming();
                        _cpu.U3.UnitReady = i.Result + unit.SegmentTime;
                        _cpu.U3.IsFinished = true;

                        if (_cpu.U3.Length == InstructionLength.Long)
                            _timeCounter++;
                        _cpu.U3 = null;
                        return;
                    }
                }
            }

            // No conflict; issue/start instruction immediately
            unit.Pipeline.Enqueue(_cpu.U3);
            unit.LastStart = _timeCounter;

            // Fill out schedule for instruction
            _cpu.U3.Issue = _timeCounter;
            _cpu.U3.Start = _timeCounter;
            _cpu.U3.Result = _timeCounter + _cpu.TimingMap[_cpu.U3.OpCode];
            CalculateU3StoreFetchTiming();
            _cpu.U3.UnitReady = _timeCounter + unit.SegmentTime;
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
                // Return immediately if no instructions are in the pipeline
                if (!unit.Pipeline.Any()) continue;

                // Initiate any pending reservations from 2nd order conflicts
                if(unit.Pipeline.Any(i => i.IsBeingHeld))
                {
                    var instructionBeingHeld = unit.Pipeline.Single(i => i.IsBeingHeld);
                    if(instructionBeingHeld.Start >= _timeCounter)
                    {
                        unit.IsReserved = false;
                    }
                }

                // Clear the front of the pipeline of completed instructions
                var frontInstructionCompletionTime = unit.Pipeline.Peek().Result;
                if (frontInstructionCompletionTime >= _timeCounter)
                {
                    unit.Pipeline.Dequeue();
                }
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
