using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlanePrefab : IComponentData
{
    public Entity prefab;
    public Entity material;
}