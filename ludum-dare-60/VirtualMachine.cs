using System;
using System.Collections.Generic;

public sealed class VirtualMachine
{
    public const int MemorySize = 256;

    private readonly Dictionary<RegisterName, byte> _Registers = [];
    private readonly byte[] _Memory = new byte[MemorySize];

    public event Action<byte> Outputed;

    public VirtualMachine()
    {
        foreach (var rn in Enum.GetValues<RegisterName>())
        {
            _Registers.Add(rn, 0);
        }
    }

    public void Reset()
    {
        foreach (var rn in Enum.GetValues<RegisterName>())
        {
            _Registers[rn] = 0;
        }

        Array.Clear(_Memory);
    }

    public byte GetRegisterValue(RegisterName registerName) => _Registers[registerName];
    public void SetRegisterValue(RegisterName registerName, byte value) => _Registers[registerName] = value;

    public byte GetMemoryValue(int address) => _Memory[address];
    public void SetMemoryValue(int address, byte value) => _Memory[address] = value;

    public void Output(byte value)
    {
        Outputed?.Invoke(value);
    }

    public bool CompareStates(VirtualMachine vm)
    {
        if (_Registers.Count != vm._Registers.Count)
        {
            return false;
        }

        foreach (var registerName in _Registers.Keys)
        {
            if (!vm._Registers.TryGetValue(registerName, out var otherValue) || _Registers[registerName] != otherValue)
            {
                return false;
            }
        }

        return _Memory.AsSpan().SequenceEqual(vm._Memory);
    }
}
