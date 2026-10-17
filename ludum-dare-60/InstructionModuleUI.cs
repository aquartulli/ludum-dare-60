using Godot;
using System.Collections.Generic;

public partial class InstructionModuleUI : Control
{
    private Node _PunchCardTarget;
    private SpinBox _ProgramCounter;
    private Button _FetchButton;
    private PunchCard _PunchCard;

    private IReadOnlyList<Instruction> _Instructions;

    [Signal]
    public delegate void FetchFailedEventHandler();

    [Signal]
    public delegate void FetchedInstructionEventHandler();

    public override void _EnterTree()
    {
        _PunchCardTarget = GetNode("%PunchCardTarget");
        _ProgramCounter = GetNode<SpinBox>("%ProgramCounter");
        _FetchButton = GetNode<Button>("%FetchButton");
        _PunchCard = GetNode<PunchCard>("%PunchCard");

        _FetchButton.Pressed += OnFetchButtonPressed;
    }

    public void Bind(IReadOnlyList<Instruction> instructions)
    {
        Unbind();

        _Instructions = instructions;
    }

    public void Unbind()
    {
        if (_Instructions == null)
        {
            return;
        }

        _Instructions = null;
        _PunchCard.Text = "";
        _PunchCard.Hide();
    }

    private void OnFetchButtonPressed()
    {
        var programCounter = (byte)_ProgramCounter.Value;
        if (programCounter < 0 || programCounter > _Instructions.Count)
        {
            EmitSignalFetchFailed();
            return;
        }

        var instruction = _Instructions[programCounter];
        _PunchCard.Text = instruction.ToString();
        _PunchCard.Show();
    }
}