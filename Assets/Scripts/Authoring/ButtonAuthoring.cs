using System;
using System.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ButtonAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public ButtonType Type;

    [Header("Debug")]
    public float minX;

    public float minY;
    public float maxX;
    public float maxY;

    private Vector3 minPoint;
    private Vector3 maxPoint;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Button
        {
            MinMaxRect = new MinMaxRect(minX, minY, maxX, maxY),
            Type = Type
        });
    }

    public void Regenerate()
    {
        StartCoroutine(RegenerateAsync());
    }

    private IEnumerator RegenerateAsync()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Game");
        yield return null;
        yield return null;

        CalculateMinMaxPoints();
    }

    private void CalculateMinMaxPoints()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (!Mathf.Approximately(screenWidth / screenHeight, 1920.0f / 1080.0f))
        {
            Debug.LogError("Editor view is not set to match the internal Tiny aspect ratio (1920:1080)! " +
                "Buttons will not work properly");
        }

        // TransformPoint takes the scale into account, so works even for 2:1 textures or similar
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        minPoint = transform.TransformPoint(-offset);
        maxPoint = transform.TransformPoint(offset);

        var minScreenPoint = Camera.main.WorldToScreenPoint(minPoint);
        var maxScreenPoint = Camera.main.WorldToScreenPoint(maxPoint);

        minX = minScreenPoint.x / screenWidth;
        minY = minScreenPoint.y / screenHeight;
        maxX = maxScreenPoint.x / screenWidth;
        maxY = maxScreenPoint.y / screenHeight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minPoint.x, minPoint.y, minPoint.z), new Vector3(maxPoint.x, minPoint.y, minPoint.z));
        Gizmos.DrawLine(new Vector3(maxPoint.x, minPoint.y, minPoint.z), new Vector3(maxPoint.x, maxPoint.y, maxPoint.z));
        Gizmos.DrawLine(new Vector3(maxPoint.x, maxPoint.y, maxPoint.z), new Vector3(minPoint.x, maxPoint.y, maxPoint.z));
        Gizmos.DrawLine(new Vector3(minPoint.x, maxPoint.y, maxPoint.z), new Vector3(minPoint.x, minPoint.y, minPoint.z));

        Gizmos.DrawLine(new Vector3(minPoint.x, minPoint.y, minPoint.z), new Vector3(maxPoint.x, maxPoint.y, maxPoint.z));
        Gizmos.DrawLine(new Vector3(maxPoint.x, minPoint.y, minPoint.z), new Vector3(minPoint.x, maxPoint.y, maxPoint.z));
    }
}