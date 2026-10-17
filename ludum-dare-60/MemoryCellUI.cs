using Godot;

[Tool]
public partial class MemoryCellUI : Control
{
    private byte _Value;
    [Export]
    public byte Value
    {
        get => _Value;
        set
        {
            _Value = value;
            var label = GetNodeOrNull<Label>("%ValueLabel");
            if (label == null)
            {
                return;
            }

            label.Text = $"0x{value:X2}";
        }
    }
}