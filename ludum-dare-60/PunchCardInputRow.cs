using Godot;
using System;

[Tool]
public partial class PunchCardInputRow : Control
{
    private int _InstructionNumber;
    [Export]
    public int InstructionNumber
    {
        get => _InstructionNumber;
        set
        {
            _InstructionNumber = value;
            var label = GetNode<PunchCardTableLabel>("%InstructionNumberLabel");
            if (label == null)
            {
                return;
            }

            label.Text = _InstructionNumber.ToString();
        }
    }

    private int? _CheckIndex;
    public int? CheckIndex
    {
        get => _CheckIndex;
        set
        {
            if (value.HasValue)
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value.Value, PunchCard.PunchValues.Length);
            }
            _CheckIndex = value;

            var inputContainer = GetNodeOrNull("%InputContainer");
            if (inputContainer == null)
            {
                return;
            }

            for (var i = 0; i < inputContainer.GetChildCount(); i++)
            {
                var input = inputContainer.GetChild<PunchCardTableInputCell>(i);
                var punched = value.HasValue && i == value.Value;
                input.Punched = punched;
            }
        }
    }
}
