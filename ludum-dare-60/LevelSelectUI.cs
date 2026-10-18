using Godot;
using System.Collections.Generic;

public partial class LevelSelectUI : Control
{
    private Node _LevelContainer;
    private Button _BackButton;

    [Export]
    private PackedScene _LevelSelectItemScene;

    private IReadOnlyList<LevelDefinition> _Levels;

    [Signal]
    public delegate void LevelSelectedEventHandler(LevelDefinition levelDefinition);

    [Signal]
    public delegate void BackRequestedEventHandler();

    public override void _EnterTree()
    {
        _LevelContainer = GetNode("%LevelContainer");

        _BackButton = GetNode<Button>("%BackButton");
        _BackButton.Pressed += OnBackButtonPressed;
    }

    public void Bind(IReadOnlyList<LevelDefinition> levels)
    {
        Unbind();

        _Levels = levels;

        foreach (LevelDefinition level in _Levels)
        {
            var levelSelectItem = _LevelSelectItemScene.Instantiate<LevelSelectItemUI>();
            _LevelContainer.AddChild(levelSelectItem);
            levelSelectItem.Bind(level);
            levelSelectItem.SelectionRequested += OnLevelSelectionRequested;
        }
    }

    public void Unbind()
    {
        foreach (var child in _LevelContainer.GetChildren())
        {
            child.QueueFree();
        }

        _Levels = null;
    }

    private void OnLevelSelectionRequested(LevelDefinition levelDefinition) => EmitSignalLevelSelected(levelDefinition);

    private void OnBackButtonPressed() => EmitSignalBackRequested();
}
