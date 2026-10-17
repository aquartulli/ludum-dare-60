using Godot;
using System;
using System.Linq;

[Tool]
public partial class PunchCard : Node2D
{
    public const string PunchValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789";
    private const int _MaxStringLength = 12;

    private string _Text;
    [Export]
    public string Text
    {
        get => _Text;
        set
        {
            if (!value.All(c => PunchValues.Contains(c)))
            {
                throw new ArgumentException("String contains invalid characters.");
            }

            if (value.Length > _MaxStringLength)
            {
                throw new ArgumentException("String too long.");
            }

            _Text = value;

            var inputRowContainer = GetNodeOrNull("%InputRowContainer");
            if (inputRowContainer == null)
            {
                return;
            }

            for (var i = 0; i < _MaxStringLength; i++)
            {
                var inputRow = inputRowContainer.GetChild<PunchCardInputRow>(i);
                int? checkIndex = i >= value.Length 
                    ? null 
                    : PunchValues.IndexOf(value[i]);
                inputRow.CheckIndex = checkIndex;
            }
        }
    }
}
