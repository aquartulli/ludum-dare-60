using Godot;
using System;

[Tool]
public partial class MemoryRowUI : Control
{
    public const int CellsPerRow = 16;

    private byte _RowAddress;
    [Export]
    public byte RowAddress
    {
        get => _RowAddress;
        set
        {
            _RowAddress = value;
            var addressLabel = GetNodeOrNull<Label>("%AddressLabel");
            if (addressLabel == null)
            {
                return;
            }

            addressLabel.Text = $"0x{value:X2}";
        }
    }

    public void SetMemoryValue(int index, byte value)
    {
        var container = GetNode("%MemoryCellContainer");
        if (index < 0 || index >= container.GetChildCount())
        {
            throw new ArgumentOutOfRangeException();
        }

        var cellUI = container.GetChild<MemoryCellUI>(index);
        cellUI.Value = value;
    }
}