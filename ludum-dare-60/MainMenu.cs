using Godot;

public partial class MainMenu : Control
{
    private Button _StartButton;

    [Signal]
    public delegate void StartRequestedEventHandler();

    public override void _EnterTree()
    {
        _StartButton = GetNode<Button>("%StartButton");
        _StartButton.Pressed += OnStartButtonPressed;
    }

    public override void _ExitTree()
    {
        _StartButton.Pressed -= OnStartButtonPressed;
    }

    private void OnStartButtonPressed() => EmitSignalStartRequested();
}
