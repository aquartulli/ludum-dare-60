using Godot;

public partial class Manual : Control
{
    [Export]
    private ManualEntry[] _Entries = [];

    private ManualEntryUI _LeftPage;
    private ManualEntryUI _RightPage;
    private Button _PreviousPageButton;
    private Button _NextPageButton;

    private Button _CloseButton;

    private int _PageNumber;

    [Signal]
    public delegate void CloseRequestedEventHandler();

    public override void _EnterTree()
    {
        _LeftPage = GetNode<ManualEntryUI>("%LeftPage");
        _RightPage = GetNode<ManualEntryUI>("%RightPage");
        _PreviousPageButton = GetNode<Button>("%PreviousPageButton");
        _NextPageButton = GetNode<Button>("%NextPageButton");

        _PreviousPageButton.Pressed += OnPreviousPageButtonPressed;
        _NextPageButton.Pressed += OnNextPageButtonPressed;

        _CloseButton = GetNode<Button>("%CloseButton");
        _CloseButton.Pressed += OnCloseButtonPressed;
    }

    public override void _Ready()
    {
        _PageNumber = 0;

        _PreviousPageButton.Hide();
        if (_Entries.Length > 0)
        {
            _LeftPage.Bind(_Entries[0]);
            _LeftPage.Show();
        }
        else
        {
            _LeftPage.Hide();
        }

        if (_Entries.Length > 1)
        {
            _RightPage.Bind(_Entries[1]);
            _RightPage.Show();
        }
        else
        {
            _RightPage.Hide();
        }

        _NextPageButton.Visible = _Entries.Length > 2;
    }

    private void OnCloseButtonPressed() => EmitSignalCloseRequested();

    private void OnNextPageButtonPressed()
    {
        _PageNumber++;
        var leftEntryIndex = _PageNumber * 2;
        var rightEntryIndex = leftEntryIndex + 1;

        _LeftPage.Bind(_Entries[leftEntryIndex]);
        _PreviousPageButton.Show();

        if (_Entries.Length >= rightEntryIndex)
        {
            _RightPage.Bind(_Entries[rightEntryIndex]);
        }
        else
        {
            _RightPage.Unbind();
            _RightPage.Hide();
        }

        if (_Entries.Length <= rightEntryIndex + 1)
        {
            _NextPageButton.Hide();
        }
    }

    private void OnPreviousPageButtonPressed()
    {
        _PageNumber--;
        var leftEntryIndex = _PageNumber * 2;
        var rightEntryIndex = leftEntryIndex + 1;

        _LeftPage.Bind(_Entries[leftEntryIndex]);
        _RightPage.Bind(_Entries[rightEntryIndex]);

        _NextPageButton.Show();

        if (_PageNumber == 0)
        {
            _PreviousPageButton.Hide();
        }
    }
}
