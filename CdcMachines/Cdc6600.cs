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
        private const int FETCH_TIME = 5;
        private const int STORE_TIME = 5;

        public void AddInstructions(List<Instruction> instructions)
        {
            foreach (var i in _instructions)
            {
                i.IsFinished = false;
                i.Issue = 0;
                i.Start = 0;
                i.Result = 0;
                i.UnitReady = 0;
                i.Fetch = 0;
                i.Start = 0;
            }
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

            // Check for first order conflict
            if (_cpu.Scoreboard.Any(u => u.Type == unitType && !u.IsReady()))
            {
                return;
            }

            // Check for second order conflicts
            foreach(var fu in _cpu.Scoreboard)
            {
                if (fu.InProgress == null && fu.Reserve == null) continue;
                
                var dependencies = new List<object>();
                if (fu.InProgress != null)
                {
                    if (fu.InProgress.OutputRegister == _cpu.U3.Operand1)
                    {
                        dependencies.Add(new { Instruction = fu.InProgress, Register = fu.InProgress.OutputRegister });
                    }
                    if (fu.InProgress.OutputRegister == _cpu.U3.Operand2)
                    {
                        dependencies.Add(new { Instruction = fu.InProgress, Register = fu.InProgress.OutputRegister });
                    }
                }
                if (fu.Reserve != null)
                {
                    if (fu.Reserve.OutputRegister == _cpu.U3.Operand1)
                    {
                        dependencies.Add(new { Instruction = fu.InProgress, Register = fu.Reserve.OutputRegister });
                    }
                    if (fu.Reserve.OutputRegister == _cpu.U3.Operand2)
                    {
                        dependencies.Add(new { Instruction = fu.InProgress, Register = fu.Reserve.OutputRegister });
                    }
                }

                // If there weren't any dependencies found, there's no need to continue
                if (!dependencies.Any()) continue;
                if (!_cpu.Scoreboard.Any(u => u.Type == unitType && u.CanReserve())) continue;
                // Handle dependencies
                // Issue instruction but delay execution
                var unit = _cpu.Scoreboard.First(u => u.Type == unitType && u.CanReserve());
                unit.Reserve = _cpu.U3;

                // Calculate timing for issued instruction
                _cpu.U3.Issue = _timeCounter;


                if (fu.InProgress.OpCode >= OpCode.SumAjandKToAi && fu.InProgress.OpCode <= OpCode.DifferenceBjandBktoXi) // Increment
                {
                    if (fu.InProgress.Operand1 >= Register.A1 && fu.InProgress.Operand1 <= Register.A5) // Read from Memory
                    {
                        _cpu.U3.Start = fu.InProgress.Fetch ?? 0;
                    }
                    else if (fu.InProgress.Operand1 >= Register.A6 && fu.InProgress.Operand1 <= Register.A7) // Write to Memory
                    {
                        _cpu.U3.Start = fu.InProgress.Store ?? 0;
                    }
                }
                else
                {
                    _cpu.U3.Start = fu.InProgress.Result;
                }
                _cpu.U3.Result = _cpu.U3.Start + _cpu.TimingMap[_cpu.U3.OpCode];
                CalculateU3StoreFetchTiming();
                _cpu.U3.UnitReady = fu.InProgress.Result + 1;
                _cpu.U3.IsFinished = true;

                if (_cpu.U3.Length == InstructionLength.Long)
                    _timeCounter++;
                _cpu.U3 = null;
                return;
            }
            // Check for third order conflict
            foreach (var fu in _cpu.Scoreboard)
            {
                if (fu.InProgress == null) continue;

                if (fu.InProgress.Operand1 == _cpu.U3.OutputRegister ||
                    fu.InProgress.Operand2 == _cpu.U3.OutputRegister)
                {
                    // Issue/Start execution but hold result until conflict resolved
                    _cpu.U3.Issue = _timeCounter;
                    _cpu.U3.Start = _timeCounter;
                    _cpu.U3.Result = fu.InProgress.Result;
                    CalculateU3StoreFetchTiming();
                    _cpu.U3.UnitReady = fu.InProgress.Result + 1;
                    _cpu.U3.IsFinished = true;

                    if (_cpu.U3.Length == InstructionLength.Long)
                        _timeCounter++;
                    _cpu.U3 = null;
                    return;
                }
            }

            // No conflict; issue/start instruction immediately
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
                // No instructions in progress or reserved
                if (unit.InProgress == null && unit.Reserve == null)
                {
                    continue;
                }

                if (unit.Reserve != null)
                {
                    // See if 2nd order conflict is resolved
                    if (unit.Reserve.Start >= _timeCounter)
                    {
                        // See if in progress instruction is completed
                        if (unit.InProgress != null)
                        {
                            if (unit.InProgress.UnitReady >= _timeCounter)
                            {
                                unit.InProgress = null;
                            }
                        }
                        
                        if (unit.InProgress == null)
                        {
                            // Conflict resolved, instruction is now processing
                            unit.InProgress = unit.Reserve;
                        }
                    }
                }

                if (unit.InProgress == null) continue;
                // See if in progress instruction is completed
                if (unit.InProgress.UnitReady >= _timeCounter)
                {
                    unit.InProgress = null;
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
