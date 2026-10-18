using Godot;
using System.Collections.Generic;

public partial class Level : Node
{
    [Export]
    private LevelDefinition _TestLevel;

    private Label _LevelName;
    private ComputerUI _ComputerUI;
    private Control _PauseMenuModal;
    private Button _ResumeButton;
    private Button _ResetButton;
    private Button _QuitButton;

    private VirtualMachine _Vm;
    private IReadOnlyList<Instruction> _Instructions;

    [Signal]
    public delegate void QuitRequestedEventHandler();

    public override void _EnterTree()
    {
        _Vm = new VirtualMachine();

        _LevelName = GetNode<Label>("%LevelName");
        _ComputerUI = GetNode<ComputerUI>("%ComputerUI");
        _PauseMenuModal = GetNode<Control>("%PauseMenuModal");

        _ResumeButton = GetNode<Button>("%ResumeButton");
        _ResetButton = GetNode<Button>("%ResetButton");
        _QuitButton = GetNode<Button>("%QuitButton");

        _ResumeButton.Pressed += OnResumeButtonPressed;
        _ResetButton.Pressed += OnResetButtonPressed;
        _QuitButton.Pressed += OnQuitButtonPressed;

        // ------------------------------------------
        // Was testing this, but the game needs to call Initialize to test it properly. Also need to push LevelDefinition for zac.
    }

    public override void _Ready()
    {
        base._Ready();

        Initialize(_TestLevel);
    }

    public void Initialize(LevelDefinition levelDefinition)
    {
        _LevelName.Text = levelDefinition.Name;

        _Instructions = Parser.ParseInstructions(levelDefinition.Program);

        _ComputerUI.Bind(_Vm, _Instructions);
        _ComputerUI.HaltRequested += OnHaltRequested;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey iek && iek.Pressed && iek.Keycode == Key.Escape)
        {
            _PauseMenuModal.Visible = !_PauseMenuModal.Visible;
        }
    }

    private void OnQuitButtonPressed()
    {
        EmitSignalQuitRequested();
    }

    private void OnResetButtonPressed()
    {
        _Vm.Reset();
        _ComputerUI.Unbind();
        _ComputerUI.Bind(_Vm, _Instructions);
        _PauseMenuModal.Hide();
    }

    private void OnResumeButtonPressed()
    {
        _PauseMenuModal.Visible = false;
    }

    private void OnHaltRequested()
    {
        var testVm = new VirtualMachine();
        ProgramRunner.RunProgram(testVm, [.. _Instructions]);

        if (_Vm.CompareStates(testVm))
        {
            GD.Print("Beat level");
        }
        else
        {
            GD.Print("Failed level");
        }
    }
}
