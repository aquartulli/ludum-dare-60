using Godot;

public partial class MainMenu : Control
{
    private Button _StartButton;

    public override void _EnterTree()
    {
        _StartButton = GetNode<Button>("%StartButton");
        _StartButton.Pressed += OnStartButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        GD.Print("Start Button");
    }
}