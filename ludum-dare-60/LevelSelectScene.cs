using Godot;

public partial class LevelSelectScene : Control
{
    private GridContainer _LevelGrid;
    private Button _BackButton;

    [Export]
    private PackedScene LevelButtonScene { get; set; }

    [Export]
    private Godot.Collections.Array<Resource> LevelResources { get; set; } = new();

    public override void _EnterTree()
    {
        _LevelGrid = GetNode<GridContainer>("%LevelGrid");

        _BackButton = GetNode<Button>("%BackButton");
        _BackButton.Pressed += OnBackButtonPressed;

        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        if (LevelButtonScene == null)
        {
            GD.PrintErr("LevelButtonScene is not assigned in the Inspector!");
            return;
        }

        int totalLevels = LevelResources.Count;

        for (int i = 0; i < totalLevels; i++)
        {
            var levelButton = LevelButtonScene.Instantiate<Button>();

            levelButton.Text = $"Level {i + 1}";

            var levelData = LevelResources[i];
            levelButton.Pressed += () => OnLevelButtonPressed(levelData);

            _LevelGrid.AddChild(levelButton);
        }
    }

    private void OnLevelButtonPressed(Resource levelData)
    {
        GD.Print($"Player clicked a level. Resource data: {levelData}");
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://MainMenu.tscn");
    }
}