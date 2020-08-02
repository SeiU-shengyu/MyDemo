using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class GenerateUIManager :  Editor{
    [MenuItem("GameObject/UI/GenerateUIManager")]
    public static void Generate()
    {
        if (Selection.activeGameObject.GetComponent<Canvas>() == null)
        {
            Debug.Log("请选择UI的父级Canvas");
            return;
        }
        else
        {
            string path = Application.dataPath + "/Scripts/UIManager.cs";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"using UnityEngine;
using UnityEngine.UI;

/*此文件为自动生成,请勿手动修改*/

public class UIManager : MonoBehaviour
{");
            RectTransform[] rectTransforms = Selection.activeGameObject.GetComponentsInChildren<RectTransform>(true);
            foreach (RectTransform rect in rectTransforms)
            {
                if (rect.name.Contains("Ctrl"))
                {
                    Image image = rect.GetComponent<Image>();
                    Text text = rect.GetComponent<Text>();
                    Animator animator = rect.GetComponent<Animator>();
                    Button button = rect.GetComponent<Button>();
                    if (image)
                        sb.Append("\n\t" + "public Image " + rect.name + ";");
                    else if (text)
                        sb.Append("\n\t" + "public Text " + rect.name + ";");
                    else if(animator)
                        sb.Append("\n\t" + "public Animator " + rect.name + ";");
                    else if (button)
                        sb.Append("\n\t" + "public Button " + rect.name + ";");
                    else
                        sb.Append("\n\t" + "public RectTransform " + rect.name + ";");
                }
            }
            sb.Append("\n}");
            File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("GameObject/UI/SetUIManagerParam")]
    public static void SetParam()
    {
        UIManager uIManager = Selection.activeGameObject.GetComponent<UIManager>();
        RectTransform[] rectTransforms = Selection.activeGameObject.GetComponentsInChildren<RectTransform>(true);
        SerializedObject serializedObject = new SerializedObject(uIManager);
        foreach (RectTransform rect in rectTransforms)
        {
            if (rect.name.Contains("Ctrl"))
            {
                Image image = rect.GetComponent<Image>();
                SerializedProperty serializedProperty = serializedObject.FindProperty(rect.name);
                if (image)
                {
                    serializedProperty.objectReferenceValue = image;
                }
                else
                {
                    serializedProperty.objectReferenceValue = rect;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
