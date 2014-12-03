namespace Cdc.Simulator6600
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// CDC6600 Simulator Class that executes instructions and outputs timing results.
    /// </summary>
    public class Cdc6600
    {
        private List<Instruction> _instructions = new List<Instruction>();
        private List<string> _output = new List<string>();
        private readonly Cpu _cpu = new Cpu();
        private int _timeCounter = -3;
        private int _instructionCounter;
        private int _lastWordStart;
        private const string NAME = "CDC6600";
        private const int NEW_WORD_TIME = 8;
        private const int FETCH_TIME = 5;
        private const int STORE_TIME = 5;
        private const int OOS_INSTRUCTION_BRANCH_COST = 6;

        /// <summary>
        /// Takes a list of instructions and resets all of the timing information 
        /// before storing the instructions in an internal field for later use.
        /// </summary>
        /// <param name="instructions">Used when Run is executed to determine timing information.</param>
        public void AddInstructions(List<Instruction> instructions)
        {
            _instructions.Clear();
            _output.Clear();
            _cpu.Reset();
            _timeCounter = -3;
            _instructionCounter = 0;
            _lastWordStart = 0;
            _instructions = instructions;
        }
        /// <summary>
        /// Takes the internal list of Instructions, if any, and fills out their 
        /// timing information by simulating CDC behavior and rules.
        /// </summary>
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

                // No instruction available in U3 register, skip this cycle.
                if (_cpu.U3 != null)
                {
                    var newWordComing = _cpu.U3.IsEndOfWord;
                    // Clear the functional units of completed instructions
                    UpdateScoreboard();
                    // See if the next instruction can be added to its functional unit
                    AttemptToProcessNextInstruction();
                    // Detect end of word and increment time accordingly
                    if (newWordComing)
                        _timeCounter = _lastWordStart + NEW_WORD_TIME - 1;
                }

                // Instruction wasn't issued (first order conflict), so don't shift registers.
                if (_cpu.U3 != null) continue;

                ShiftRegisters();
            }

            PrintSchedule();
        }

        /// <summary>
        /// Tries to fill out the timing information for the next instruction in U3
        /// by assessing first, second, and third conflicts and looking up timing information
        /// based on instruction contents.
        /// </summary>
        private void AttemptToProcessNextInstruction()
        {
            var instructionIndex = _instructionCounter;
            if (_instructionCounter > _instructions.Count - 1)
            {
                instructionIndex = _instructions.Count - 1;
            }
            // Find functional unit for this instruction
            var unitType = _cpu.UnitMap[_cpu.U3.OpCode];

            // Assign initial timing values
            var issue = _timeCounter;
            var start = _timeCounter;
            var result = start + _cpu.TimingMap[_cpu.U3.OpCode];

            // Check for first order conflict
            if (!_cpu.Scoreboard.Any(u => u.Type == unitType && !u.IsInUse()))
            {
                return;
            }

            var unit = _cpu.Scoreboard.First(u => u.Type == unitType && !u.IsInUse());

            // Check for second order conflicts
            for (var i = 0; i < instructionIndex; i++)
            {
                // Detect proper register being waited on
                var outputRegister = _cpu.U3.Operand1;
                if (_cpu.U3.Operand1 == Register.A6)
                    outputRegister = Register.X6;
                if (_cpu.U3.Operand1 == Register.A7)
                    outputRegister = Register.X7;

                if (_instructions[i].OutputRegister != outputRegister &&
                    _instructions[i].OutputRegister != _cpu.U3.Operand2) continue;

                var tempStart = 0;
                // Detect if this instruction is waiting on a value from memory
                if (_instructions[i].OpCode >= OpCode.SumAjandKToAi && 
                    _instructions[i].OpCode <= OpCode.DifferenceBjandBktoXi) // Increment
                {
                    if (_instructions[i].Operand1 >= Register.A1 && 
                        _instructions[i].Operand1 <= Register.A5) // Read from Memory
                    {
                        tempStart = _instructions[i].Fetch ?? 0;
                    }
                    else if (_instructions[i].Operand1 >= Register.A6 && 
                        _instructions[i].Operand1 <= Register.A7) // Write to Memory
                    {
                        tempStart = _instructions[i].Store ?? 0;
                    }
                }
                else
                {
                    tempStart = _instructions[i].Result;
                }

                // The result with the largest delay dictates when this instruction can start
                if (start >= tempStart) continue;

                start = tempStart;
                result = start + _cpu.TimingMap[_cpu.U3.OpCode];
            }

            // Check for third order conflicts
            for (var i = 0; i < instructionIndex; i++)
            {
                if (_instructions[i].Operand1 != _cpu.U3.OutputRegister &&
                    _instructions[i].Operand2 != _cpu.U3.OutputRegister) continue;

                if (result < _instructions[i].Result)
                {
                    result = _instructions[i].Result;
                }
            }

            _cpu.U3.Issue = issue;
            _cpu.U3.Start = start;
            _cpu.U3.Result = result;
            CalculateU3StoreFetchTiming();
            _cpu.U3.UnitReady = _cpu.U3.Result + 1;
            _cpu.U3.IsFinished = true;
            unit.InUse = _cpu.U3;
            _output.Add(_cpu.U3.GetScheduleOutput());

            if (!DetectBranch())
            {
                // Skip a cycle if instruction is long
                if (_cpu.U3.Length == InstructionLength.Long)
                    _timeCounter++;
            }
            _cpu.U3 = null;
        }
        /// <summary>
        /// Detects whether or not the issued instruction is a branch 
        /// and responds accordingly to it.
        /// </summary>
        /// <returns>Returns true if branch was detected, otherwise false.</returns>
        private bool DetectBranch()
        {
            var canBranch = false;
            var branchDestination = _cpu.U3.BranchTo;
            switch (_cpu.U3.OpCode)
            {
                case OpCode.ReturnJumpToK:
                    canBranch = true;
                    break;
                case OpCode.GoToKplusBi:
                    var index = _instructions.IndexOf(branchDestination);
                    branchDestination = _instructions[index];
                    canBranch = true;
                    break;
                case OpCode.GoToKifXEqualsZero:
                    canBranch = _cpu.Registers[_cpu.U3.Operand2] == 0;
                    break;
                case OpCode.GoToKifXNotEqualsZero:
                    canBranch = _cpu.Registers[_cpu.U3.Operand2] != 0;
                    break;
                case OpCode.GoToKifXPositive:
                    canBranch = _cpu.Registers[_cpu.U3.Operand2] >= 0;
                    break;
                case OpCode.GoToKifXNegative:
                    canBranch = _cpu.Registers[_cpu.U3.Operand2] < 0;
                    break;
                case OpCode.GoToKifBiEqualsBj:
                    canBranch = _cpu.Registers[_cpu.U3.Operand1] == 
                                _cpu.Registers[_cpu.U3.Operand2];
                    break;
                case OpCode.GoToKifBiNotEqualsBj:
                    canBranch = _cpu.Registers[_cpu.U3.Operand1] != 
                                _cpu.Registers[_cpu.U3.Operand2];
                    break;
                case OpCode.GoToKifBiGreaterThanEqualToBj:
                    canBranch = _cpu.Registers[_cpu.U3.Operand1] >= 
                                _cpu.Registers[_cpu.U3.Operand2];
                    break;
                case OpCode.GoToKifBiLessThanBj:
                    canBranch = _cpu.Registers[_cpu.U3.Operand1] < 
                                _cpu.Registers[_cpu.U3.Operand2];
                    break;
            }
            if (!canBranch) return false;

            var indexOfDestination = _instructions.IndexOf(branchDestination);
            for (var i = indexOfDestination; i < _instructions.Count; i++)
            {
                _instructions[i].Reset();
            }
            _instructionCounter = indexOfDestination;
            _timeCounter += _cpu.U3.Result + OOS_INSTRUCTION_BRANCH_COST;
            _cpu.U1 = _cpu.U2 = _cpu.U3 = null;
            return true;
        }
        /// <summary>
        /// Sets the Store or Fetch timing information if necessary based on OpCode.
        /// </summary>
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
        /// <summary>
        /// Attempts to mark functional units as no longer in use whose
        /// instructions would have finished executing by now.
        /// </summary>
        private void UpdateScoreboard()
        {
            // Clear units which are finished and store results from fetches.
            foreach(var unit in _cpu.Scoreboard)
            {
                if (unit.InUse == null) continue;
                if (unit.InUse.UnitReady > _timeCounter) continue;

                unit.InUse = null;
            }

            // If unit instructions have fetched a value, store it in the appropriate register.
            foreach (var instruction in _instructions)
            {
                if (!instruction.IsFinished) continue;

                if (InstructionIsFetch(instruction) && instruction.Fetch <= _timeCounter)
                {
                    _cpu.Registers[instruction.OutputRegister] = instruction.Value;
                }
            }
        }

        /// <summary>
        /// Determines if the given instruction involves a fetch 
        /// based on its OpCode
        /// </summary>
        /// <param name="instruction">Takes an Instruction object.</param>
        /// <returns>Returns true or false.</returns>
        private bool InstructionIsFetch(Instruction instruction)
        {
            if (instruction.OpCode < OpCode.SumAjandKToAi || 
                instruction.OpCode > OpCode.DifferenceBjandBktoXi)
                return false;

            return instruction.Operand1 >= Register.A1 &&
                   instruction.Operand1 <= Register.A5;
        }
        /// <summary>
        /// Shifts the CPU U registers and adjusts the time counter appropriately
        /// based on instruction length.
        /// </summary>
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

        /// <summary>
        /// Prints the current value of the time counter to the Console.
        /// </summary>
        private void PrintTime()
        {
            Console.WriteLine("Cycle: {0}", _timeCounter);
        }
        /// <summary>
        /// Prints the current value of the CPU U registers to the Console.
        /// </summary>
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
        /// <summary>
        /// Prints the timing information for a given instruction to the Console.
        /// </summary>
        private void PrintInstructionTiming(Instruction i)
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
        /// <summary>
        /// Prints the current contents of the CPU registers.
        /// </summary>
        private void PrintRegisters()
        {
            var registerTypes = Enum.GetValues(typeof(Register)).Cast<Register>();
            foreach (var e in registerTypes)
            {
                Console.WriteLine("Register: {0}[{1}]", 
                    e, _cpu.Registers[e]);
            }
        }
        /// <summary>
        /// Prints the timing schedule for the internal list of instructions.
        /// </summary>
        private void PrintSchedule()
        {
            Console.WriteLine();
            Console.WriteLine(NAME);
            Console.WriteLine("====================== Timing Schedule ======================");
            Console.WriteLine("Code\tLength\tIssue\tStart\tResult\tUnit\tFetch\tStore");
            foreach (var i in _output)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
        }
    }
}
