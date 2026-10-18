using Godot;

[GlobalClass]
public partial class LevelDefinition : Resource
{
    [Export]
    private string _Name;
    public string Name => _Name;

    [Export(PropertyHint.MultilineText)]
    private string _Program;
    public string Program => _Program;
}