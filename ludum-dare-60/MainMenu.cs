using Godot;

public partial class MainMenu : Control
{
    private Button _StartButton;
    private Button _OptionsButton;
    private Button _QuitButton;
    private ConfirmationDialog _QuitConfirmationDialog;

    public override void _Ready()
    {
        _StartButton = GetNode<Button>("%StartButton");
        _StartButton.Pressed += OnStartButtonPressed;

        _OptionsButton = GetNode<Button>("%OptionsButton");
        _OptionsButton.Pressed += OnOptionsButtonPressed;

        _QuitButton = GetNode<Button>("%QuitButton");
        _QuitButton.Pressed += OnQuitButtonPressed;

        _QuitConfirmationDialog = GetNode<ConfirmationDialog>("%QuitConfirmationDialog");
        _QuitConfirmationDialog.Confirmed += OnQuitConfirmed;
    }



    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://LevelSelectScene.tscn");
    }

    private void OnOptionsButtonPressed() 
    {
        GD.Print("Options Button Pressed");
    }

    private void OnQuitButtonPressed()
    {
        _QuitConfirmationDialog.PopupCentered();
    }

    public void OnQuitConfirmed()
    {
        GetTree().Quit();
    }
}
