using Godot;
using System;

public partial class Main : Node
{
    [Export]
    private PackedScene _MainMenuScene;

    [Export]
    private PackedScene _LevelScene;

    [Export]
    private LevelDefinition _TestLevel;

    public override void _Ready()
    {
        base._Ready();
        SwitchToMainMenu();
    }

    private void SwitchToMainMenu()
    {
        ClearChildren();

        var mainMenu = _MainMenuScene.Instantiate<MainMenu>();
        AddChild(mainMenu);

        mainMenu.StartRequested += OnMainMenuStartRequested;
    }

    private void ClearChildren()
    {
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
    }

    private void OnMainMenuStartRequested()
    {
        SwitchToLevel();
    }

    private void SwitchToLevel()
    {
        ClearChildren();

        var level = _LevelScene.Instantiate<Level>();
        AddChild(level);

        level.Initialize(_TestLevel);
        level.QuitRequested += OnLevelQuitRequested;
    }

    private void OnLevelQuitRequested() => SwitchToMainMenu();
}
