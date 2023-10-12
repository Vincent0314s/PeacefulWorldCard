using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIListEditor : EditorWindow
{

    string AIState;
    AIOwner aio;

    [MenuItem("VincentTools/AIState Generator")]
    public static void ShowWindow()
    {
        GetWindow<AIListEditor>("AIState");
    }

    private void OnGUI()
    {
        AIState = EditorGUILayout.TextField("State's Name",AIState);
        aio = (AIOwner)EditorGUILayout.EnumPopup("AI's type", aio);

        if (GUILayout.Button("Add new AI state") && AIState!=null) {
            AIStateManager.AddNewAIState(AIState, aio.ToString());
        }

    }

}
