using Godot;

[Tool]
public partial class PunchCardTableLabel : Control
{
    [Export]
    public string Text
    {
        get => GetNodeOrNull<Label>("%Label")?.Text ?? "";
        set
        {
            var node = GetNodeOrNull<Label>("%Label");
            if (node == null)
            {
                return;
            }

            node.Text = value;
        }
    }
}
