using Godot;

[Tool]
public partial class ManualEntryUI : Control
{
    private ManualEntry _ManualEntry;
    private Label _OpCode;
    private Label _Description;

    public override void _EnterTree()
    {
        _OpCode = GetNode<Label>("%OpCode");
        _Description = GetNode<Label>("%Description");
    }

    public override void _ExitTree()
    {
        Unbind();
    }

    public void Bind(ManualEntry manualEntry)
    {
        Unbind();

        _ManualEntry = manualEntry;
        _ManualEntry.Changed += OnManualEntryChanged;

        OnManualEntryChanged();
    }

    private void OnManualEntryChanged()
    {
        _OpCode.Text = _ManualEntry.OpCode;
        _Description.Text = _ManualEntry.Description;
    }

    public void Unbind()
    {
        if (_ManualEntry == null)
        {
            return;
        }

        _ManualEntry.Changed -= OnManualEntryChanged;
        _ManualEntry = null;

        _OpCode.Text = "OPCODE";
        _Description.Text = "Description";
    }
}