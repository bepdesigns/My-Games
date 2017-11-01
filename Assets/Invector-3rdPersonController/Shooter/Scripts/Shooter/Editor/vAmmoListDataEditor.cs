using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(vAmmoListData), true)]
public class vAmmoListDataEditor : Editor
{
    GUISkin skin;
    private Texture2D m_Logo = null;

    void OnEnable()
    {
        m_Logo = (Texture2D)Resources.Load("icon_v2", typeof(Texture2D));
    }

    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;
        GUILayout.BeginVertical("Ammo Manager List Data", "window");
        GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));
        GUILayout.Space(10);

        //EditorGUILayout.HelpBox("You can add and manage ammo here if you're NOT using the Inventory. Otherwise the weapon will search for ammo inside the Inventory.", MessageType.Info);

        base.OnInspectorGUI();
        //DrawPropertiesExcluding(serializedObject, "m_Script");

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    [MenuItem("Invector/Shooter/Create new AmmoListData")]
    public static void CreateAmmoList()
    {
        vScriptableObjectUtility.CreateAsset<vAmmoListData>();
    }
}
