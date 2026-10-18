using Godot;

[GlobalClass]
[Tool]
public partial class ManualEntry : Resource
{
    private string _OpCode;
    [Export]
    public string OpCode
    { 
        get => _OpCode;
        private set
        {
            _OpCode = value;
            EmitChanged();
        }
    }

    private string _Description;
    [Export(PropertyHint.MultilineText)]
    public string Description
    {
        get => _Description;
        set
        {
            _Description = value;
            EmitChanged();
        }
    }
}