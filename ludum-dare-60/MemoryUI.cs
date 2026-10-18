using Godot;

public partial class MemoryUI : Control
{
    private SpinBox _Address;
    private SpinBox _Data;
    private Button _LoadButton;
    private Button _StoreButton;
    private Node _MemoryRowContainer;
    private VirtualMachine _Vm;

    public override void _EnterTree()
    {
        _Address = GetNode<SpinBox>("%Address");
        _Data = GetNode<SpinBox>("%Data");
        _LoadButton = GetNode<Button>("%LoadButton");
        _StoreButton = GetNode<Button>("%StoreButton");
        _MemoryRowContainer = GetNode("%MemoryRowContainer");

        _LoadButton.Pressed += OnLoadButtonPressed;
        _StoreButton.Pressed += OnStoreButtonPressed;
    }

    public void Bind(VirtualMachine vm)
    {
        Unbind();

        _Vm = vm;

        for (var i = 0; i < VirtualMachine.MemorySize; i++)
        {
            UpdateMemoryCell(i);
        }
    }

    public void Unbind()
    {
        if (_Vm == null)
        {
            return;
        }

        _Vm = null;
    }

    private void UpdateMemoryCell(int address)
    {
        var data = _Vm.GetMemoryValue(address);

        var (row, column) = GetAddressRowAndColumn(address);
        var rowUI = GetRowFromIndex(row);
        rowUI.SetMemoryValue(column, data);
    }

    private void OnStoreButtonPressed()
    {
        var address = Mathf.RoundToInt(_Address.Value);
        var data = (byte)Mathf.RoundToInt(_Data.Value);

        _Vm.SetMemoryValue(address, data);
        UpdateMemoryCell(address);
    }

    private void OnLoadButtonPressed()
    {
        var address = (byte)Mathf.RoundToInt(_Address.Value);
        _Data.Value = _Vm.GetMemoryValue(address);
    }

    private MemoryRowUI GetRowFromIndex(int index) => _MemoryRowContainer.GetChild<MemoryRowUI>(index);

    private static (int row, int column) GetAddressRowAndColumn(int address)
    {
        return (address / MemoryRowUI.CellsPerRow, address % MemoryRowUI.CellsPerRow);
    }
}
