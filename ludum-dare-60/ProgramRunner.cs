using System;

public static class ProgramRunner
{
    public static void RunProgram(VirtualMachine vm, ReadOnlySpan<Instruction> instructions) => RunProgram(vm, instructions, out _);

    public static void RunProgram(VirtualMachine vm, ReadOnlySpan<Instruction> instructions, out byte programCounter)
    {
        programCounter = 0;
        while (true)
        {
            if (programCounter >= instructions.Length)
            {
                break;
            }

            var instruction = instructions[programCounter];
            if (!ExecuteInstruction(vm, instruction, ref programCounter))
            {
                break;
            }
        }
    }

    private static bool ExecuteInstruction(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        if (instruction.OpCode == OpCode.Add)
        {
            ExecuteAdd(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Subtract)
        {
            ExecuteSubtract(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Jump)
        {
            ExecuteJump(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.JumpZero)
        {
            ExecuteJumpZero(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Load)
        {
            ExecuteLoad(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Move)
        {
            ExecuteMove(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Out)
        {
            ExecuteOut(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Set)
        {
            ExecuteSet(vm, instruction, ref programCounter);
            return true;
        }
        else if (instruction.OpCode == OpCode.Store)
        {
            ExecuteStore(vm, instruction, ref programCounter);
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void ExecuteStore(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var addressRegister = instruction.Operands[0].GetRegisterName();
        var sourceRegister = instruction.Operands[1].GetRegisterName();

        var address = vm.GetRegisterValue(addressRegister);
        var data = vm.GetRegisterValue(sourceRegister);

        vm.SetMemoryValue(address, data);
        programCounter++;
    }

    private static void ExecuteSet(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var destRegister = instruction.Operands[0].GetRegisterName();
        var immediate = instruction.Operands[1].GetImmediate();

        vm.SetRegisterValue(destRegister, immediate);
        programCounter++;
    }

    private static void ExecuteOut(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var sourceRegister = instruction.Operands[0].GetRegisterName();
        var sourceValue = vm.GetRegisterValue(sourceRegister);

        vm.Output(sourceValue);
        programCounter++;
    }

    private static void ExecuteMove(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var sourceRegister = instruction.Operands[0].GetRegisterName();
        var destRegister = instruction.Operands[1].GetRegisterName();

        var sourceValue = vm.GetRegisterValue(sourceRegister);
        vm.SetRegisterValue(destRegister, sourceValue);

        programCounter++;
    }

    private static void ExecuteLoad(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var addressRegister = instruction.Operands[0].GetRegisterName();
        var destRegister = instruction.Operands[1].GetRegisterName();

        var address = vm.GetRegisterValue(addressRegister);
        var data = vm.GetMemoryValue(address);

        vm.SetRegisterValue(destRegister, data);

        programCounter++;
    }

    private static void ExecuteJumpZero(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var addressRegister = instruction.Operands[0].GetRegisterName();
        var zeroRegister = instruction.Operands[1].GetRegisterName();

        var zeroValue = vm.GetRegisterValue(zeroRegister);
        if (zeroValue == 0)
        {
            programCounter = vm.GetRegisterValue(addressRegister);
        }
        else
        {
            programCounter++;
        }
    }

    private static void ExecuteJump(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var addressRegister = instruction.Operands[0].GetRegisterName();
        programCounter = vm.GetRegisterValue(addressRegister);
    }

    private static void ExecuteSubtract(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var operand1 = instruction.Operands[0].GetRegisterName();
        var operand1Value = vm.GetRegisterValue(operand1);
        var operand2Value = vm.GetRegisterValue(instruction.Operands[1].GetRegisterName());

        var result = unchecked((byte)(operand1Value - operand2Value));
        vm.SetRegisterValue(operand1, result);

        programCounter++;
    }

    private static void ExecuteAdd(VirtualMachine vm, Instruction instruction, ref byte programCounter)
    {
        var operand1 = instruction.Operands[0].GetRegisterName();
        var operand1Value = vm.GetRegisterValue(operand1);
        var operand2Value = vm.GetRegisterValue(instruction.Operands[1].GetRegisterName());

        var result = unchecked((byte)(operand1Value + operand2Value));
        vm.SetRegisterValue(operand1, result);

        programCounter++;
    }
}