using Godot;

public partial class HowToPlayScreen : Control
{
    private Button _BackButton;

    [Signal]
    public delegate void BackRequestedEventHandler();

    public override void _EnterTree()
    {
        _BackButton = GetNode<Button>("%BackButton");
        _BackButton.Pressed += OnBackButtonPressed;
    }

    private void OnBackButtonPressed()
    {
        EmitSignalBackRequested();
    }
}