using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct MaterialReferences : ISharedComponentData, IEquatable<MaterialReferences>
{
    public Material[] Materials;

    public bool Equals(MaterialReferences other)
    {
        return EqualityComparer<Material[]>.Default.Equals(Materials, other.Materials);
    }

    public override int GetHashCode()
    {
        return 573613553 + EqualityComparer<Material[]>.Default.GetHashCode(Materials);
    }
}