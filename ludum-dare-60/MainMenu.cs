using Godot;

public partial class MainMenu : Control
{
    private Button _StartButton;
    private Button _HowToPlayButton;
    private Button _QuitButton;

    [Signal]
    public delegate void StartRequestedEventHandler();

    [Signal]
    public delegate void HowToPlayRequestedEventHandler();

    [Signal]
    public delegate void QuitRequestedEventHandler();

    public override void _EnterTree()
    {
        _StartButton = GetNode<Button>("%StartButton");
        _StartButton.Pressed += OnStartButtonPressed;

        _HowToPlayButton = GetNode<Button>("%HowToPlayButton");
        _HowToPlayButton.Pressed += OnHowToPlayButtonPressed;

        _QuitButton = GetNode<Button>("%QuitButton");
        _QuitButton.Pressed += OnQuitButtonPressed;
    }

    public override void _ExitTree()
    {
        _StartButton.Pressed -= OnStartButtonPressed;
        _HowToPlayButton.Pressed -= OnHowToPlayButtonPressed;
        _QuitButton.Pressed -= OnQuitButtonPressed;
    }

    private void OnQuitButtonPressed() => EmitSignalQuitRequested();

    private void OnStartButtonPressed() => EmitSignalStartRequested();

    private void OnHowToPlayButtonPressed() => EmitSignalHowToPlayRequested();
}
