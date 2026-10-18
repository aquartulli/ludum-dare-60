using Godot;
using System.Collections.Generic;

public partial class Level : Node
{
    private Label _LevelName;
    private ComputerUI _ComputerUI;
    private Control _PauseMenuModal;
    private Button _ResumeButton;
    private Button _ResetButton;
    private Button _QuitButton;
    private Control _EndScreenModal;
    private Button _EndScreenResetButton;
    private Button _EndScreenLevelSelectButton;
    private RichTextLabel _ExitCodeLabel;

    private bool _Won;
    private VirtualMachine _Vm;
    private IReadOnlyList<Instruction> _Instructions;

    [Export]
    private Color _ExitCode0Color = Colors.Green;

    [Export]
    private Color _ExitCode1Color = Colors.Red;

    [Signal]
    public delegate void QuitRequestedEventHandler();

    [Signal]
    public delegate void LevelEndedEventHandler(bool victory);

    public override void _EnterTree()
    {
        _Vm = new VirtualMachine();

        _LevelName = GetNode<Label>("%LevelName");
        _ComputerUI = GetNode<ComputerUI>("%ComputerUI");

        _PauseMenuModal = GetNode<Control>("%PauseMenuModal");
        _ResumeButton = GetNode<Button>("%ResumeButton");
        _ResetButton = GetNode<Button>("%ResetButton");
        _QuitButton = GetNode<Button>("%QuitButton");

        _EndScreenModal = GetNode<Control>("%EndScreenModal");
        _ExitCodeLabel = GetNode<RichTextLabel>("%ExitCodeLabel");
        _EndScreenResetButton = GetNode<Button>("%EndScreenResetButton");
        _EndScreenLevelSelectButton = GetNode<Button>("%EndScreenLevelSelectButton");

        _ResumeButton.Pressed += OnResumeButtonPressed;
        _ResetButton.Pressed += OnResetButtonPressed;
        _QuitButton.Pressed += OnQuitButtonPressed;

        _EndScreenResetButton.Pressed += OnEndScreenResetButtonPressed;
        _EndScreenLevelSelectButton.Pressed += OnEndScreenLevelSelectButtonPressed;
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

    private void OnEndScreenLevelSelectButtonPressed()
    {
        EmitSignalLevelEnded(_Won);
    }

    private void OnEndScreenResetButtonPressed()
    {
        _Vm.Reset();
        _ComputerUI.Unbind();
        _ComputerUI.Bind(_Vm, _Instructions);
        _EndScreenModal.Hide();
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

        _Won = _Vm.CompareStates(testVm);
        _EndScreenModal.Show();
        var colorString = _Won ? _ExitCode0Color.ToHtml() : _ExitCode1Color.ToHtml();
        var statusCodeString = _Won ? "0" : "1";
        _ExitCodeLabel.Text = $"EXIT CODE: [color=#{colorString}]{statusCodeString}[/color]";
    }
}
