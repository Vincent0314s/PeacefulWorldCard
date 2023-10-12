using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSavedData
{

}

public class SettingSavedData
{
    public float MasterVolume;
    public float MusicVolume;
    public float SoundEffectVolume;
}

public static class SaveLoadSystem
{
    private readonly static string SAVE_FOLDER = Application.dataPath + StringDefs.Path.SaveFolder;
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static bool HasSaveData()
    {
        if (File.Exists(SAVE_FOLDER + StringDefs.SaveData.gameSave))
        {
            return true;
        }
        return false;
    }

    public static void Save(string _saveType,string _saveString)
    {
        File.WriteAllText(SAVE_FOLDER + _saveType, _saveString);
    }

    public static string Load(string _saveType)
    {
        if (File.Exists(SAVE_FOLDER + _saveType))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + _saveType);
            return saveString;
        }
        else
        {
            return null;
        }
    }

   

    //public staic void SaveData()
    //{
    //    SaveObject saveObject = new SaveObject
    //    {

    //    };
    //    string json = JsonUtility.ToJson(saveObject);
    //    SaveLoadSystem.Save(json);
    //}

    //public void LoadData()
    //{
    //    string saveString = SaveLoadSystem.Load();
    //    if (saveString != null)
    //    {
    //        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

    //    }
    //    else
    //    {
    //        Debug.LogError("NoSave");
    //    }
    //}
}
