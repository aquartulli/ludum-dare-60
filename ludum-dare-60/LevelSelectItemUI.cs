using Godot;

public partial class LevelSelectItemUI : Control
{
    private Button _Button;
    private LevelDefinition _LevelDefinition;

    [Signal]
    public delegate void SelectionRequestedEventHandler(LevelDefinition levelDefinition);

    public override void _EnterTree()
    {
        _Button = GetNode<Button>("%Button");
        _Button.Pressed += OnButtonPressed;
    }

    public override void _ExitTree()
    {
        _Button.Pressed -= OnButtonPressed;
        _Button = null;
    }

    public void Bind(LevelDefinition levelDefinition)
    {
        Unbind();

        _LevelDefinition = levelDefinition;
        _Button.Text = _LevelDefinition.Name;
    }

    public void Unbind()
    {
        _LevelDefinition = null;
        _Button.Text = "";
    }

    private void OnButtonPressed() => EmitSignalSelectionRequested(_LevelDefinition);
}