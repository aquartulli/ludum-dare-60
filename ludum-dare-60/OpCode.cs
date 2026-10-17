using System.Collections.Generic;

public sealed class OpCode
{
    public string Name { get; }
    public string AssemblyText { get; }
    public IReadOnlyList<InstructionOperandType> Operands { get; }

    private OpCode(string name, string assemblyText, IReadOnlyList<InstructionOperandType> operands)
    {
        Name = name;
        AssemblyText = assemblyText;
        Operands = operands;
    }

    public static OpCode Set { get; } = new("Set", "SET", [InstructionOperandType.Register, InstructionOperandType.Immediate]);
    public static OpCode Move { get; } = new("Move", "MOV", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Add { get; } = new("Add", "ADD", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Subtract { get; } = new("Subtract", "SUB", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Load { get; } = new("Load", "LOAD", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Store { get; } = new("Store", "STORE", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Jump { get; } = new("Jump", "JMP", [InstructionOperandType.Register]);
    public static OpCode JumpZero { get; } = new("JumpZero", "JZ", [InstructionOperandType.Register, InstructionOperandType.Register]);
    public static OpCode Out { get; } = new("Out", "OUT", [InstructionOperandType.Register]);
    public static OpCode Halt { get; } = new("Halt", "HALT", []);
}
