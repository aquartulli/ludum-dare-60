using Godot;

public partial class RegisterModuleUI : Control
{
    private RegisterUI _Register1;
    private RegisterUI _Register2;
    private RegisterUI _Register3;
    private RegisterUI _Register4;

    public override void _EnterTree()
    {
        _Register1 = GetNode<RegisterUI>("%Register1");
        _Register2 = GetNode<RegisterUI>("%Register2");
        _Register3 = GetNode<RegisterUI>("%Register3");
        _Register4 = GetNode<RegisterUI>("%Register4");
    }

    public void Bind(VirtualMachine vm)
    {
        Unbind();

        _Register1.Bind(vm);
        _Register2.Bind(vm);
        _Register3.Bind(vm);
        _Register4.Bind(vm);
    }

    public void Unbind()
    {
        _Register1.Unbind();
        _Register2.Unbind();
        _Register3.Unbind();
        _Register4.Unbind();
    }
}
