using System;
using System.Collections.Generic;

public class VirtualMachine
{
    public const int MemorySize = 256;

    private readonly Dictionary<RegisterName, byte> _Registers = [];
    private readonly byte[] _Memory = new byte[MemorySize];

    public VirtualMachine()
    {
        foreach (var rn in Enum.GetValues<RegisterName>())
        {
            _Registers.Add(rn, 0);
        }
    }

    public byte GetRegisterValue(RegisterName registerName) => _Registers[registerName];
    public void SetRegisterValue(RegisterName registerName, byte value) => _Registers[registerName] = value;

    public byte GetMemoryValue(int address) => _Memory[address];
    public void SetMemoryValue(int address, byte value) => _Memory[address] = value;
}
