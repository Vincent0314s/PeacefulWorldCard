using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FileRenameEditor : EditorWindow
{
    private string originalFileName;
    private string newFilename;

    Vector2 scrollPosition;

    List<Object> files = new List<Object>();

    List<Object> filesContainingName;

    bool updatedFiles = false;
    bool updatedFilesContainingNames = false;
    bool replacePressed = false;
    bool renameConfirmed = false;
    bool renameCompleted = false;
    int numberOfFilesRenamed;

    [MenuItem("VincentTools/RenameFiles")]
    public static void ShowWindow()
    {
        GetWindow<FileRenameEditor>("RenameFiles");
    }

    private void OnGUI()
    {
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.HelpBox("Select the files that you want to rename in the Project Window on the \"Select\" Button ", MessageType.Info, true);
        GUILayout.Label("Files to rename", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Select", GUILayout.Width(100), GUILayout.Height(25)))
        {
            renameConfirmed = false;
            updatedFiles = false;
            updatedFilesContainingNames = false;
            replacePressed = false;
            renameCompleted = false;
            ObtainFiles();
        }
        EditorGUILayout.EndVertical();

        if (updatedFiles)
        {
            //EditorGUILayout.BeginVertical(GUILayout.Width(400),GUILayout.Height(400));
            GUILayout.Label("\n Selected " + files.Count + " Files", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < files.Count; i++)
            {
                GUILayout.Label(files[i].name);
            }
            GUILayout.EndScrollView();
            //EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            GUILayout.Label("Enter the string to be Replaced and the String to be replaced With");
            GUILayout.Label("Both these strings are Case Sensitive");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Replace : ");
            originalFileName = EditorGUILayout.TextField(originalFileName);
            GUILayout.Label("With : ");
            newFilename = EditorGUILayout.TextField(newFilename);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();
            if (!string.IsNullOrEmpty(originalFileName) && !string.IsNullOrEmpty(newFilename) && newFilename != originalFileName)
            {

                if (updatedFiles && !updatedFilesContainingNames)
                {
                    filesContainingName = new List<Object>();
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i].name.Contains(originalFileName))
                        {
                            filesContainingName.Add(files[i]);
                        }
                    }
                    updatedFilesContainingNames = true;
                }

                if (GUILayout.Button("REPLACE", GUILayout.Width(100), GUILayout.Height(25)))
                {
                    replacePressed = true;
                    renameConfirmed = false;

                    if (filesContainingName.Count > 0)
                    {
                        if (EditorUtility.DisplayDialog("Confirmation", "Are you sure you want to replace " + filesContainingName.Count + " file names?", "YES", "NO"))
                        {
                            renameConfirmed = true;
                            renameCompleted = false;
                        }
                        else
                        {
                            renameConfirmed = false;
                        }
                    }
                    if (replacePressed && !renameCompleted)
                    {
                        numberOfFilesRenamed = 0;

                        if (renameConfirmed)
                        {
                            for (int i = 0; i < filesContainingName.Count; i++)
                            {
                                if (EditorUtility.DisplayCancelableProgressBar("Renaming", "Renaming File : " + filesContainingName[i].name
                                    , (((float)(filesContainingName.IndexOf(filesContainingName[i])) / (filesContainingName.Count)))))
                                {
                                    break;
                                }
                                if (filesContainingName[i].name.Contains(originalFileName))
                                {
                                    numberOfFilesRenamed++;
                                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetOrScenePath(filesContainingName[i]), filesContainingName[i].name.Replace(originalFileName, newFilename));
                                }
                            }
                            EditorUtility.ClearProgressBar();

                            renameCompleted = true;
                        }

                        if (numberOfFilesRenamed == 0)
                        {
                            if (EditorUtility.DisplayDialog("Recheck files", "No file name replaced", "Exit"))
                            {
                                replacePressed = false;
                            }
                        }
                    }
                }
            }
        }
    }


    private void ObtainFiles()
    {
        if (Selection.activeObject != null)
        {
            files = new List<Object>();
            files.AddRange(Selection.objects);
        }
        updatedFiles = true;
        replacePressed = false;
    }
}
