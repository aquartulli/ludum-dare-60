using Godot;

[Tool]
public partial class PunchCardTableInputCell : Control
{
    public bool Punched
    {
        get => GetNodeOrNull<CheckBox>("%CheckBox")?.ButtonPressed ?? false;
        set
        {
            var checkBox = GetNodeOrNull<CheckBox>("%CheckBox");
            if (checkBox != null)
            {
                checkBox.ButtonPressed = value;
            }
        }
    }
}