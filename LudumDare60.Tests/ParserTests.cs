namespace LudumDare60.Tests;

public class ParserTests
{
    [Test]
    public void ParseInstructions_SingleMoveInstruction_Success()
    {
        var program = @"MOV R1 R2";

        var instructions = Parser.ParseInstructions(program);

        Assert.That(instructions.Count, Is.EqualTo(1));

        var instruction = instructions[0];

        Assert.That(instruction.OpCode, Is.EqualTo(OpCode.Move));

        Assert.That(instruction.Operands.Count, Is.EqualTo(2));

        var operand1 = instruction.Operands[0];

        Assert.That(operand1.Type, Is.EqualTo(InstructionOperandType.Register));

        Assert.That(operand1.GetRegisterName(), Is.EqualTo(RegisterName.R1));

        var operand2 = instruction.Operands[1];

        Assert.That(operand2.Type, Is.EqualTo(InstructionOperandType.Register));

        Assert.That(operand2.GetRegisterName(), Is.EqualTo(RegisterName.R2));
    }
}