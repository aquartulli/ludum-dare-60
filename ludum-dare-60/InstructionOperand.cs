using System;

public readonly struct InstructionOperand
{
    public InstructionOperandType Type { get; }

    private readonly byte _Value;

    public InstructionOperand(byte immediate)
    {
        Type = InstructionOperandType.Immediate;
        _Value = immediate;
    }

    public InstructionOperand(RegisterName registerName)
    {
        Type = InstructionOperandType.Register;
        _Value = (byte)registerName;
    }

    public byte GetImmediate()
    {
        if (Type != InstructionOperandType.Immediate)
        {
            throw new InvalidOperationException();
        }

        return _Value;
    }

    public RegisterName GetRegisterName()
    {
        if (Type != InstructionOperandType.Register)
        {
            throw new InvalidOperationException();
        }

        return (RegisterName)_Value;
    }

    public override string ToString() => Type switch
    {
        InstructionOperandType.Register => ((RegisterName)_Value).ToString(),
        InstructionOperandType.Immediate => _Value.ToString(),
        _ => Type.ToString()
    };
}