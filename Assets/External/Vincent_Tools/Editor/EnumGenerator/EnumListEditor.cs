using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


public class EnumListEditor : EditorWindow
{
    public List<EnumList> newList = new List<EnumList>();

    private ReorderableList _reOrderableList;
    private SerializedObject _serializedObject;
    private SerializedProperty _serializedPropetry;

    private MultipleEnumList _multiepleEnumList;
    private Vector2 _scrollPosition;

    [MenuItem("VincentTools/Enum/Enum List Generator")]
    public static void ShowWindow()
    {
        GetWindow<EnumListEditor>("EnumList");
    }

    [MenuItem("VincentTools/Enum/Generate Scene List")]
    public static void GenerateSceneList()
    {
        EnumListUtil.AddSceneList();
    }

    private void OnEnable()
    {
        LoadListAsset();

        _serializedObject = new SerializedObject(this);
        _serializedPropetry = _serializedObject.FindProperty("newList");

        _reOrderableList = new ReorderableList(_serializedObject, _serializedPropetry)
        {
            displayAdd = true,
            displayRemove = true,

            drawHeaderCallback = StringsDrawHeader,

            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = _serializedPropetry.GetArrayElementAtIndex(index);

                var enumName = element.FindPropertyRelative("name");
                var innerList = element.FindPropertyRelative("enumVariable");
                var foldOut = element.FindPropertyRelative("foldout");

                foldOut.boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight), foldOut.boolValue, enumName.stringValue);
                EditorGUI.indentLevel++;
                if (foldOut.boolValue)
                {
                    rect.y += EditorGUIUtility.singleLineHeight;

                    var innerReOrderableList = new ReorderableList(element.serializedObject, innerList)
                    {
                        displayAdd = true,
                        displayRemove = true,
                        draggable = true,

                        drawHeaderCallback = innerRect =>
                        {
                            enumName.stringValue = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Enum Class Name", enumName.stringValue);
                        },

                        drawElementCallback = (innerRect, innerIndex, innerIsActive, InnerIsFocused) =>
                        {
                            var innerElement = innerList.GetArrayElementAtIndex(innerIndex);
                            EditorGUI.PropertyField(innerRect, innerElement);
                        },
                    };

                    var height = (innerList.arraySize + 3) * EditorGUIUtility.singleLineHeight;
                    innerReOrderableList.DoList(new Rect(rect.x, rect.y, rect.width, height));
                    rect.y += EditorGUIUtility.singleLineHeight;

                }
                EditorGUI.indentLevel--;

            },
            elementHeightCallback = index =>
            {
                var element = _serializedPropetry.GetArrayElementAtIndex(index);
                var foldout = element.FindPropertyRelative("foldout");
                var innerList = element.FindPropertyRelative("enumVariable");

                var height = EditorGUIUtility.singleLineHeight;

                if (foldout.boolValue)
                {
                    height = (innerList.arraySize + 5) * EditorGUIUtility.singleLineHeight;
                }

                return height;
            }
        };
    }
    private void OnGUI()
    {
        if (_serializedObject == null)
        {
            return;
        }

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

        _serializedObject.Update();
        _reOrderableList.DoLayoutList();
        _serializedObject.ApplyModifiedProperties();

        GUILayout.EndScrollView();

        if (GUILayout.Button("Update Enum List", GUILayout.Width(200), GUILayout.Height(30)))
        {
            EnumListUtil.AddNewEnum(newList);
            CreateListAsset();
        }
    }

    private void CreateListAsset()
    {
        MultipleEnumList asset = ScriptableObject.CreateInstance<MultipleEnumList>();

        _multiepleEnumList = asset;
        _multiepleEnumList.enumLists = new List<EnumList>(newList);
        AssetDatabase.CreateAsset(asset, EnumListUtil.assetPath);
        AssetDatabase.SaveAssets();
    }

    private void LoadListAsset()
    {
        _multiepleEnumList = AssetDatabase.LoadAssetAtPath(EnumListUtil.assetPath, typeof(MultipleEnumList)) as MultipleEnumList;
        if (_multiepleEnumList)
        {
            newList = new List<EnumList>(_multiepleEnumList.enumLists);
        }
    }
    private void StringsDrawHeader(Rect rect)
    {
        // your GUI code here for list header
        EditorGUI.LabelField(rect, "Custom Enum List");
    }

    #region Legacy
    void StringsDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        // your GUI code here for list content
        //EditorGUI.LabelField(rect, "New List");
        var element = _serializedPropetry.GetArrayElementAtIndex(index);

        var enumName = element.FindPropertyRelative("name");
        var enumChild = element.FindPropertyRelative("enumVariable");
        var foldout = element.FindPropertyRelative("foldout");
        EditorGUI.indentLevel++;
        {
            //foldout.boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? "" : enumName.stringValue);
            foldout.boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight), foldout.boolValue, enumName.stringValue);

            if (foldout.boolValue)
            {
                enumName.stringValue = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), enumName.stringValue);
                rect.y += EditorGUIUtility.singleLineHeight;

                var enumList = new ReorderableList(element.serializedObject, enumChild)
                {
                    displayAdd = true,
                    displayRemove = true,

                    drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "EnumList");
                    },
                    drawElementCallback = (convRect, convIndex, convActive, convFocused) =>
                    {
                        enumChild.stringValue = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), enumChild.stringValue);
                    },
                };

            }
            EditorGUI.indentLevel--;
            //EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), enumChild, true);
        }
    }


    private float GetEleHeight(SerializedProperty ele)
    {
        var foldout = ele.FindPropertyRelative("Foldout");
        var height = EditorGUIUtility.singleLineHeight;

        if (foldout.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight * 5;

            var singleEnum = ele.FindPropertyRelative("enumVariable");

            for (int i = 0; i < singleEnum.arraySize; i++)
            {
                var e = singleEnum.GetArrayElementAtIndex(i);
                height += EditorGUIUtility.singleLineHeight;
            }
        }

        return height;
    }
    #endregion
}
