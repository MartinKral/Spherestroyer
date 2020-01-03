using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct MaterialReferences : ISharedComponentData, IEquatable<MaterialReferences>
{
    public Material[] Materials;

    // It does not really matter here as this is Editor only, but is EqualityComparer fast enough comparison?
    // How often is it triggered?
    // https://forum.unity.com/threads/how-to-implement-iequatable-on-isharedcomponentdata.681505/#post-4561582

    public bool Equals(MaterialReferences other)
    {
        return EqualityComparer<Material[]>.Default.Equals(Materials, other.Materials);
    }

    public override int GetHashCode()
    {
        return 573613553 + EqualityComparer<Material[]>.Default.GetHashCode(Materials);
    }
}