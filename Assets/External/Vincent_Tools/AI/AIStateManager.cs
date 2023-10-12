#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AIStateManager : MonoBehaviour
{
    const string extension = ".cs";
    public static string filePath = Application.dataPath + "/";

    //Used By Editor
    public static void AddNewAIState<T>(string fileName,T type)
    {
        using (StreamWriter sw = File.CreateText(filePath + fileName + extension))
        {
            sw.WriteLine("using AIState;\n");
            sw.WriteLine("\npublic class " + fileName + ": State<" + type.ToString() + ">" + " \n{");
            sw.WriteLine("\tprivate static " + fileName + " _i;\n");
            sw.WriteLine("\tprivate " + fileName + "()\n{");
            sw.WriteLine("\t\tif(_i == null)\n{");
            sw.WriteLine("\t\t\t_i = this;\n}");
            sw.WriteLine("\t\telse\n{");
            sw.WriteLine("\t\t\treturn;\n}\n");
            sw.WriteLine("}");
            sw.WriteLine("\npublic static " + fileName + " i\n{");
            sw.WriteLine("\tget\n{");
            sw.WriteLine("\tif(_i == null)\n{");
            sw.WriteLine("\t\tnew " + fileName + "();\n}");
            sw.WriteLine("\treturn _i;\n}");
            sw.WriteLine("\t\n}");
            sw.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }
}
#endif