namespace Cdc.Simulator7600
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a readonly list of hardcoded instructions for use
    /// by the system.
    /// </summary>
    public class InstructionSet
    {
        // AX^2 + B
        public readonly List<Instruction> InstructionSet1 = new List<Instruction>
        {
            #region Instructions
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.X1,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Fetch X
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A2,
                Operand2 = Register.K,
                OutputRegister = Register.X2,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Fetch A
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X1,
                Operand2 = Register.X1,
                OutputRegister = Register.X0,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Form X^2
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X0,
                Operand2 = Register.X2,
                OutputRegister = Register.X6,
                IsStartOfWord = false,
                IsEndOfWord = false
            }, // Form AX^2
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A3,
                Operand2 = Register.K,
                OutputRegister = Register.X3,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Fetch B
            new Instruction
            {
                OpCode = OpCode.FloatingSum,   
                Length = InstructionLength.Short,
                Operand1 = Register.X6,
                Operand2 = Register.X3,
                OutputRegister = Register.X7,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Form Y
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A7,
                Operand2 = Register.K,
                OutputRegister = Register.X7,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Store Y
            #endregion
        };
        // AX^2 + BX + C
        public readonly List<Instruction> InstructionSet2 = new List<Instruction>
        {
            #region Instructions
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.X1,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Fetch X
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A2,
                Operand2 = Register.K,
                OutputRegister = Register.X2,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Fetch A
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X1,
                Operand2 = Register.X1,
                OutputRegister = Register.X0,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Form X^2
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X0,
                Operand2 = Register.X2,
                OutputRegister = Register.X6,
                IsStartOfWord = false,
                IsEndOfWord = false
            }, // Form AX^2
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A3,
                Operand2 = Register.K,
                OutputRegister = Register.X3,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Fetch B
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A4,
                Operand2 = Register.K,
                OutputRegister = Register.X4,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Fetch C
            new Instruction
            {
                OpCode = OpCode.FloatingProduct, 
                Length = InstructionLength.Short,
                Operand1 = Register.X3,
                Operand2 = Register.X1,
                OutputRegister = Register.X3,
                IsStartOfWord = false,
                IsEndOfWord = false
            }, // Form BX
            new Instruction
            {
                OpCode = OpCode.FloatingSum,   
                Length = InstructionLength.Short,
                Operand1 = Register.X6,
                Operand2 = Register.X3,
                OutputRegister = Register.X5,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Form AX^2+BX
            new Instruction
            {
                OpCode = OpCode.FloatingSum,   
                Length = InstructionLength.Short,
                Operand1 = Register.X6,
                Operand2 = Register.X3,
                OutputRegister = Register.X7,
                IsStartOfWord = true,
                IsEndOfWord = false
            }, // Form Y
            new Instruction
            {
                OpCode = OpCode.SumAjandKToXi, 
                Length = InstructionLength.Long,
                Operand1 = Register.A7,
                Operand2 = Register.K,
                OutputRegister = Register.X7,
                IsStartOfWord = false,
                IsEndOfWord = true
            }, // Store Y
            #endregion
        };
        // AX^2 + BX + C (X and Y are vectors of n elements)
        public readonly List<Instruction> InstructionSet3 = new List<Instruction>();

        public InstructionSet()
        {
            #region Set 3 Instructions
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.B1,
                IsStartOfWord = true,
                IsEndOfWord = false,
                Value = 5
            }); // Fetch N
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A1,
                Operand2 = Register.K,
                OutputRegister = Register.X1,
                IsStartOfWord = false,
                IsEndOfWord = true,
                Value = 1
            }); // Fetch A
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A2,
                Operand2 = Register.K,
                OutputRegister = Register.X2,
                IsStartOfWord = true,
                IsEndOfWord = false,
                Value = 1
            }); // Fetch B
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A3,
                Operand2 = Register.K,
                OutputRegister = Register.X3,
                IsStartOfWord = false,
                IsEndOfWord = true,
                Value = 1
            }); // Fetch C
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A4,
                Operand2 = Register.K,
                OutputRegister = Register.B2,
                IsStartOfWord = true,
                IsEndOfWord = false,
                Value = 1
            }); // Fetch Decrement Value = 1
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A5,
                Operand2 = Register.K,
                OutputRegister = Register.X5,
                IsStartOfWord = false,
                IsEndOfWord = true,
                Value = 1
            }); // Fetch X[i]
            // X0 and X4 are available at this point.
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.FloatingProduct,
                Length = InstructionLength.Short,
                Operand1 = Register.X5,
                Operand2 = Register.X5,
                OutputRegister = Register.X0,
                IsStartOfWord = true,
                IsEndOfWord = false
            }); // Form X^2
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.FloatingProduct,
                Length = InstructionLength.Short,
                Operand1 = Register.X2,
                Operand2 = Register.X5,
                OutputRegister = Register.X0,
                IsStartOfWord = false,
                IsEndOfWord = false
            }); // Form BX
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.FloatingProduct,
                Length = InstructionLength.Short,
                Operand1 = Register.X0,
                Operand2 = Register.X1,
                OutputRegister = Register.X4,
                IsStartOfWord = false,
                IsEndOfWord = false
            }); // Form AX^2
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.FloatingSum,
                Length = InstructionLength.Short,
                Operand1 = Register.X0,
                Operand2 = Register.X4,
                OutputRegister = Register.X6,
                IsStartOfWord = false,
                IsEndOfWord = true
            }); // Form AX^2+BX
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.FloatingSum,
                Length = InstructionLength.Short,
                Operand1 = Register.X6,
                Operand2 = Register.X3,
                OutputRegister = Register.X7,
                IsStartOfWord = true,
                IsEndOfWord = false
            }); // Form Y
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumBjandBktoBi,
                Length = InstructionLength.Short,
                Operand1 = Register.B2,
                Operand2 = Register.B0,
                OutputRegister = Register.B0,
                IsStartOfWord = false,
                IsEndOfWord = false
            }); // Increment B0 by B2
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.SumAjandKToXi,
                Length = InstructionLength.Long,
                Operand1 = Register.A7,
                Operand2 = Register.K,
                OutputRegister = Register.X7,
                IsStartOfWord = false,
                IsEndOfWord = true
            }); // Store Y[i]
            InstructionSet3.Add(new Instruction
            {
                OpCode = OpCode.GoToKifBiNotEqualsBj,
                Length = InstructionLength.Long,
                Operand1 = Register.B1,
                Operand2 = Register.B0,
                OutputRegister = Register.K,
                IsStartOfWord = true,
                IsEndOfWord = true,
                BranchTo = InstructionSet3[5]
            }); // Branch to instruction 6 if B1 != BO

            #endregion
        }
    }
}
