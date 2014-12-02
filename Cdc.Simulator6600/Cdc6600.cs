﻿namespace Cdc.Simulator6600
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
        private readonly Cpu _cpu = new Cpu();
        private int _timeCounter = -3;
        private int _instructionCounter;
        private int _lastWordStart;
        private const int NEW_WORD_TIME = 8;
        private const int FETCH_TIME = 5;
        private const int STORE_TIME = 5;

        /// <summary>
        /// Takes a list of instructions and resets all of the timing information 
        /// before storing the instructions in an internal field for later use.
        /// </summary>
        /// <param name="instructions">Used when Run is executed to determine timing information.</param>
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

                //PrintTime();

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

            // Skip a cycle if instruction is long
            if (_cpu.U3.Length == InstructionLength.Long)
                _timeCounter++;
            _cpu.U3 = null;
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
            // Clear units which are finished
            foreach(var unit in _cpu.Scoreboard)
            {
                if (unit.InUse == null) continue;

                if(unit.InUse.UnitReady <= _timeCounter)
                {
                    unit.InUse = null;
                }
            }
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
        /// Prints the current value of the CPU U registers to the Console.
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
        /// Prints the timing schedule for the internal list of instructions.
        /// </summary>
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