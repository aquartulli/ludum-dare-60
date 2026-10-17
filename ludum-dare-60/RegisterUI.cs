using Godot;

[Tool]
public partial class RegisterUI : Node
{
    private RegisterName _RegisterName;
    [Export]
    public RegisterName RegisterName
    {
        get => _RegisterName;
        set
        {
            _RegisterName = value;
            var registerNameLabel = GetNodeOrNull<Label>("%RegisterName");
            if (registerNameLabel == null)
            {
                return;
            }

            registerNameLabel.Text = value.ToString();
        }
    }

    private VirtualMachine _Vm;
    private SpinBox _Data;

    public override void _EnterTree()
    {
        _Data = GetNode<SpinBox>("%Data");
        _Data.ValueChanged += OnDataValueChanged;
    }

    public void Bind(VirtualMachine vm)
    {
        _Vm = vm;
    }

    public void Unbind()
    {
        _Vm = null;
    }

    private void OnDataValueChanged(double value) => _Vm.SetRegisterValue(_RegisterName, (byte)value);
}