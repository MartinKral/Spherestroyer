using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(ButtonAuthoring))]
public class ButtonAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        string warningMessage = "Remember to always Regenerate after changing button position / scale.";

        EditorGUILayout.HelpBox(warningMessage, MessageType.Info);

        DrawDefaultInspector();

        ButtonAuthoring buttonAuthoring = (ButtonAuthoring)target;
        if (GUILayout.Button("Regenerate MinMaxRect"))
        {
            buttonAuthoring.Regenerate();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(buttonAuthoring);
            EditorSceneManager.MarkSceneDirty(buttonAuthoring.gameObject.scene);
        }
    }
}