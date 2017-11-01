using UnityEngine;
using System.Collections;
using UnityEditor;
using Invector;
using Invector.CharacterController;

[CanEditMultipleObjects]
[CustomEditor(typeof(vShooterManager),true)]
public class vShooterManagerEditor : vEditorBase
{
    protected override void OnEnable()
    {
        m_Logo = Resources.Load("icon_v2") as Texture2D;
        base.OnEnable();        
    }

    public override void OnInspectorGUI()
    {       
        GUI.skin = skin;        

        vShooterManager manager = (vShooterManager)this.target;
       

        GUILayout.BeginVertical("Shooter Manager", "window");        
        GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));        

        openCloseWindow = GUILayout.Toggle(openCloseWindow, openCloseWindow ? "Close" : "Open", EditorStyles.toolbarButton);
       
        if (openCloseWindow)
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.HelpBox("Playmode Debug - Equip a weapon first", MessageType.Info);
                EditorGUILayout.Space();

                if (GUILayout.Button(manager.tpCamera.lockCamera ? "Unlock Camera" : "Lock Camera", EditorStyles.toolbarButton))
                {
                    manager.tpCamera.lockCamera = !manager.tpCamera.lockCamera;
                }
                EditorGUILayout.Space();
                if (GUILayout.Button(manager.alwaysAiming ? "Unlock Aiming" : "Lock Aiming", EditorStyles.toolbarButton))
                {
                    manager.alwaysAiming = !manager.alwaysAiming;
                }
                EditorGUILayout.Space();
                if (GUILayout.Button(manager.showCheckAimGizmos ? "Hide Aim Gizmos" : "Show Aim Gizmos", EditorStyles.toolbarButton))
                {
                    manager.showCheckAimGizmos = !manager.showCheckAimGizmos;
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
       
    }
}
