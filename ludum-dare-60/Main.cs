using Godot;

public partial class Main : Node
{
    [Export]
    private PackedScene _MainMenuScene;

    public override void _Ready()
    {
        base._Ready();

        var mainMenu = _MainMenuScene.Instantiate();
        AddChild(mainMenu);
    }
}

// TODO: Next, help zac push test project and test ProgramRunner.