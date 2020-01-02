using Unity.Entities;

[GenerateAuthoringComponent]
public struct IcospherePrefab : IComponentData
{
    public Entity prefab;
    public Entity material;
}