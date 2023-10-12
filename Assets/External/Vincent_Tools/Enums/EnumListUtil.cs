#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class EnumListUtil
{
    private const string extension = ".cs";
    public static string filePath = Application.dataPath + "/External/Vincent_Tools/Lists/";
    public static string assetPath = "Assets/External/Vincent_Tools/Lists/EnumListAsset.asset";

    public static string fileName = "EnumListStorage";
    public static string sceneFileName = "SceneListStorage";

    public static void ReImportFolder()
    {
        AssetDatabase.Refresh();
    }


    //Used By Editor
    public static void AddNewEnum(List<EnumList> data)
    {
        using (StreamWriter sw = File.CreateText(filePath + fileName + extension))
        {
            foreach (var list in data)
            {
                sw.WriteLine("public enum " + list.name + " \n{");
                foreach (var item in list.enumVariable)
                {
                    string lineRep = item.ToString().Replace(" ", string.Empty); //Remove Extra Space
                    if (!string.IsNullOrEmpty(lineRep)) // if line is not null
                    {
                        sw.WriteLine(string.Format("\t{0},", lineRep));
                    }
                }
                sw.WriteLine("\n}");
            }
        }

        AssetDatabase.Refresh();
    }

    public static void LoadCurrentEnum()
    {
        StreamReader sr = new StreamReader(filePath + fileName + extension);
        while (!sr.EndOfStream)
        {
            Debug.Log(sr.ReadLine());
        }
        sr.Close();
    }

    public static string GetCurrentEnum()
    {
        StreamReader sr = new StreamReader(filePath + fileName + extension);
        return sr.ReadToEnd();
    }

    public static void AddSceneList()
    {
        using (StreamWriter sw = File.CreateText(filePath + sceneFileName + extension))
        {
            sw.WriteLine("public enum " + "SceneInTheBuild" + " \n{");
            EditorBuildSettingsScene[] allScenes = EditorBuildSettings.scenes;

            foreach (var item in allScenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(item.path);
                string lineRep = sceneName.ToString().Replace(" ", string.Empty); //Remove Extra Space
                if (!string.IsNullOrEmpty(lineRep)) // if line is not null
                {
                    sw.WriteLine(string.Format("\t{0},", lineRep));
                }
            }
            sw.WriteLine("\n}");
        }
        AssetDatabase.Refresh();
    }
}
#endif
