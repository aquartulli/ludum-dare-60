using Godot;
using System.Collections.Generic;

public partial class ComputerUI : Node
{
    private MemoryUI _MemoryUI;
    private RegisterModuleUI _RegisterModuleUI;
    private InstructionModuleUI _InstructionModuleUI;
    private Button _HaltButton;
    private Manual _Manual;
    private AnimationPlayer _ManualAnimationPlayer;

    [Signal]
    public delegate void HaltRequestedEventHandler();

    public override void _EnterTree()
    {
        base._EnterTree();

        _MemoryUI = GetNode<MemoryUI>("%MemoryModuleUI");
        _RegisterModuleUI = GetNode<RegisterModuleUI>("%RegisterModuleUI");

        _InstructionModuleUI = GetNode<InstructionModuleUI>("%InstructionModuleUI");
        _InstructionModuleUI.OpenManualRequested += OnOpenManualRequested;

        _HaltButton = GetNode<Button>("%HaltButton");
        _HaltButton.Pressed += OnHaltButtonPressed;

        _Manual = GetNode<Manual>("%Manual");
        _Manual.CloseRequested += OnManualCloseRequested;

        _ManualAnimationPlayer = GetNode<AnimationPlayer>("%ManualAnimationPlayer");
    }

    private void OnManualCloseRequested() => _ManualAnimationPlayer.Play("Close");
    private void OnOpenManualRequested() => _ManualAnimationPlayer.Play("Open");

    public void Bind(VirtualMachine vm, IReadOnlyList<Instruction> instructions)
    {
        Unbind();

        _MemoryUI.Bind(vm);
        _RegisterModuleUI.Bind(vm);
        _InstructionModuleUI.Bind(instructions);
    }

    public void Unbind()
    {
        _MemoryUI.Unbind();
        _RegisterModuleUI.Unbind();
        _InstructionModuleUI.Unbind();
    }

    private void OnHaltButtonPressed()
    {
        EmitSignalHaltRequested();
    }
}