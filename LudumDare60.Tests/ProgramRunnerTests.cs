namespace LudumDare60.Tests;

public class ProgramRunnerTests
{
    [Test]
    public void RunProgram_SingleMove_DestRegisterContainsValue()
    {
        const byte value = 123;
        const RegisterName source = RegisterName.R1;
        const RegisterName dest = RegisterName.R2;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(source, value);

        var program = new Instruction[]
        {
            new(OpCode.Move, [new(source), new(dest)])
        };

        ProgramRunner.RunProgram(vm, program);

        var destValue = vm.GetRegisterValue(dest);

        Assert.That(destValue, Is.EqualTo(value));
    }

    [Test]
    public void RunProgram_Empty_Returns()
    {
        var vm = new VirtualMachine();
        var program = Array.Empty<Instruction>();

        ProgramRunner.RunProgram(vm, program);
    }

    [Test]
    public void RunProgram_Halt_NextInstructionNotExecuted()
    {
        const byte value = 123;
        const RegisterName dest = RegisterName.R1;

        var vm = new VirtualMachine();
        var program = new Instruction[]
        {
            new(OpCode.Halt, []),
            new(OpCode.Set, [new(dest), new(value)])
        };

        ProgramRunner.RunProgram(vm, program);

        var destValue = vm.GetRegisterValue(dest);
        Assert.That(destValue, Is.EqualTo(default(byte)));
    }

    [Test]
    public void RunProgram_Set_SetsRegisterToImmediate()
    {
        const byte value = 123;
        const RegisterName dest = RegisterName.R1;

        var vm = new VirtualMachine();
        var program = new Instruction[]
        {
            new(OpCode.Set, [new(dest), new(value)])
        };

        ProgramRunner.RunProgram(vm, program);

        var destValue = vm.GetRegisterValue(dest);

        Assert.That(destValue, Is.EqualTo(value));
    }

    [Test]
    public void RunProgram_AddNoOverflow_SetsDestToResult()
    {
        const RegisterName dest = RegisterName.R1;
        const RegisterName other = RegisterName.R2;
        const byte left = 2;
        const byte right = 5;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(dest, left);
        vm.SetRegisterValue(other, right);

        var program = new Instruction[]
        {
            new(OpCode.Add, [new(dest), new(other)])
        };

        ProgramRunner.RunProgram(vm, program);

        var sum = vm.GetRegisterValue(dest);
        Assert.That(sum, Is.EqualTo((byte)(left + right)));
    }

    [Test]
    public void RunProgram_AddWithOverflow_SetsDestToWrappedResult()
    {
        const RegisterName dest = RegisterName.R1;
        const RegisterName other = RegisterName.R2;
        const byte left = 255;
        const byte right = 2;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(dest, left);
        vm.SetRegisterValue(other, right);

        var program = new Instruction[]
        {
            new(OpCode.Add, [new(dest), new(other)])
        };

        ProgramRunner.RunProgram(vm, program);

        var sum = vm.GetRegisterValue(dest);
        Assert.That(sum, Is.EqualTo(unchecked((byte)(left + right))));
    }

    [Test]
    public void RunProgram_SubtractNoUndeflow_SetsDestToResult()
    {
        const RegisterName dest = RegisterName.R1;
        const RegisterName other = RegisterName.R2;
        const byte left = 10;
        const byte right = 3;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(dest, left);
        vm.SetRegisterValue(other, right);

        var program = new Instruction[]
        {
            new(OpCode.Subtract, [new(dest), new(other)])
        };

        ProgramRunner.RunProgram(vm, program);

        var sum = vm.GetRegisterValue(dest);
        Assert.That(sum, Is.EqualTo((byte)(left - right)));
    }

    [Test]
    public void RunProgram_SubtractWithUndeflow_SetsDestToWrappedResult()
    {
        const RegisterName dest = RegisterName.R1;
        const RegisterName other = RegisterName.R2;
        const byte left = 2;
        const byte right = 3;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(dest, left);
        vm.SetRegisterValue(other, right);

        var program = new Instruction[]
        {
            new(OpCode.Subtract, [new(dest), new(other)])
        };

        ProgramRunner.RunProgram(vm, program);

        var sum = vm.GetRegisterValue(dest);
        Assert.That(sum, Is.EqualTo(unchecked((byte)(left - right))));
    }

    [Test]
    public void RunProgram_Jump_ProgramCounterSetToRegisterValue()
    {
        const RegisterName addressRegister = RegisterName.R1;
        const byte address = 123;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(addressRegister, address);

        var program = new Instruction[]
        {
            new(OpCode.Jump, [new(addressRegister)])
        };

        ProgramRunner.RunProgram(vm, program, out var programCounter);

        Assert.That(programCounter, Is.EqualTo(address));
    }

    [Test]
    public void RunProgram_JumpZero_Zero_Jumps()
    {
        const RegisterName addressRegister = RegisterName.R1;
        const byte address = 123;
        const RegisterName conditionRegister = RegisterName.R2;
        const byte condition = 0;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(addressRegister, address);
        vm.SetRegisterValue(conditionRegister, condition);

        var program = new Instruction[]
        {
            new(OpCode.JumpZero, [new(addressRegister), new(conditionRegister)])
        };

        ProgramRunner.RunProgram(vm, program, out var programCounter);

        Assert.That(programCounter, Is.EqualTo(address));
    }

    [Test]
    public void RunProgram_JumpZero_NonZero_MovesToNextInstructionWithoutJumping()
    {
        const RegisterName addressRegister = RegisterName.R1;
        const byte address = 123;
        const RegisterName conditionRegister = RegisterName.R2;
        const byte condition = 1;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(addressRegister, address);
        vm.SetRegisterValue(conditionRegister, condition);

        var program = new Instruction[]
        {
            new(OpCode.JumpZero, [new(addressRegister), new(conditionRegister)])
        };

        ProgramRunner.RunProgram(vm, program, out var programCounter);

        Assert.That(programCounter, Is.EqualTo(1));
    }

    [Test]
    public void RunProgram_Load_SetsRegisterToValueAtMemoryAddess()
    {
        const RegisterName addressRegister = RegisterName.R1;
        const byte address = 123;

        const RegisterName destRegister = RegisterName.R2;
        const byte data = 69;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(addressRegister, address);
        vm.SetMemoryValue(address, data);

        var program = new Instruction[]
        {
            new(OpCode.Load, [new(addressRegister), new(destRegister)])
        };

        ProgramRunner.RunProgram(vm, program);

        var destValue = vm.GetRegisterValue(destRegister);

        Assert.That(destValue, Is.EqualTo(data));
    }

    [Test]
    public void RunProgram_Store_SetsMemoryToRegisterValue()
    {
        const RegisterName addressRegister = RegisterName.R1;
        const byte address = 123;

        const RegisterName sourceRegister = RegisterName.R2;
        const byte data = 69;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(addressRegister, address);
        vm.SetRegisterValue(sourceRegister, data);

        var program = new Instruction[]
        {
            new(OpCode.Store, [new(addressRegister), new(sourceRegister)])
        };

        ProgramRunner.RunProgram(vm, program);

        var memoryValue = vm.GetMemoryValue(address);

        Assert.That(memoryValue, Is.EqualTo(data));
    }

    [Test]
    public void RunProgram_Out_RaisesOutputedEventWithRegisterValue()
    {
        const RegisterName sourceRegister = RegisterName.R1;
        const byte value = 123;

        var vm = new VirtualMachine();
        vm.SetRegisterValue(sourceRegister, value);

        byte? outputValue = null;
        vm.Outputed += v => outputValue = v;

        var program = new Instruction[]
        {
            new(OpCode.Out, [new(sourceRegister)])
        };

        ProgramRunner.RunProgram(vm, program);

        Assert.That(outputValue.HasValue, Is.True);
        Assert.That(outputValue.Value, Is.EqualTo(value));
    }
}
