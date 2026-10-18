using Godot;
using System;

public partial class Main : Node
{
    [Export]
    private PackedScene _MainMenuScene;

    [Export]
    private PackedScene _LevelSelectUIScene;

    [Export]
    private PackedScene _LevelScene;

    [Export]
    private LevelDefinition[] _Levels = [];

    private int _CurrentLevelIndex;
    private int _LevelsUnlocked = 1;

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
        mainMenu.QuitRequested += OnMainMenuQuitRequested;
    }

    private void OnMainMenuQuitRequested() => GetTree().Quit();

    private void SwitchToLevelSelect()
    {
        ClearChildren();

        var levelSelectUI = _LevelSelectUIScene.Instantiate<LevelSelectUI>();
        AddChild(levelSelectUI);
        levelSelectUI.Bind(new ArraySegment<LevelDefinition>(_Levels, 0, _LevelsUnlocked));

        levelSelectUI.LevelSelected += OnLevelSelected;
        levelSelectUI.BackRequested += OnLevelSelectBackRequested;
    }

    private void OnLevelSelectBackRequested()
    {
        SwitchToMainMenu();
    }

    private void OnLevelSelected(LevelDefinition levelDefinition)
    {
        _CurrentLevelIndex = Array.IndexOf(_Levels, levelDefinition);
        SwitchToLevel(levelDefinition);
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
        SwitchToLevelSelect();
    }

    private void SwitchToLevel(LevelDefinition levelDefinition)
    {
        ClearChildren();

        var level = _LevelScene.Instantiate<Level>();
        AddChild(level);

        level.Initialize(levelDefinition);
        level.QuitRequested += OnLevelQuitRequested;
        level.LevelEnded += OnLevelEnded;
    }

    private void OnLevelEnded(bool victory)
    {
        if (victory && _CurrentLevelIndex == _LevelsUnlocked - 1)
        {
            _LevelsUnlocked++;
            SwitchToLevelSelect();
        }
    }

    private void OnLevelQuitRequested() => SwitchToMainMenu();
}
