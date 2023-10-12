using System.Collections.Generic;

[System.Serializable]
public class EnumList
{
    public string name;
    public bool foldout { get; set; }
    public List<string> enumVariable = new List<string>();
}
