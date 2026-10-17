using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class Instruction
{
    public OpCode OpCode { get; }
    public IReadOnlyList<InstructionOperand> Operands { get; }

    public Instruction(OpCode opCode, IReadOnlyList<InstructionOperand> operands)
    {
        if (opCode.Operands.Count != operands.Count)
        {
            throw new ArgumentException($"Operand counts do not match.");
        }

        if (!opCode.Operands
            .Zip(operands)
            .All(tuple =>
            {
                var (operandType, operand) = tuple;
                return operandType == operand.Type;
            }))
        {
            throw new ArgumentException($"Operand types do not match.");
        }

        OpCode = opCode;
        Operands = operands;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(OpCode.ToString());

        foreach (var operand in Operands)
        {
            sb.Append($" {operand.ToString()}");
        }

        return sb.ToString();
    }
}
