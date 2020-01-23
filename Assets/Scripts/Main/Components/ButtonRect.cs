using Unity.Entities;

public struct Button : IComponentData
{
    public MinMaxRect MinMaxRect;
    public ButtonType Type;
}