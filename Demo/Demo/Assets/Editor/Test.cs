using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Test : EditorWindow
{
    [MenuItem("Tools/Test")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<Test>(false);
    }

    private string m_path1;
    private string m_path2;

    public void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        m_path1 = EditorGUILayout.TextField(m_path1, GUILayout.Width(500), GUILayout.Height(20));
        m_path2 = EditorGUILayout.TextField(m_path2, GUILayout.Width(500), GUILayout.Height(20));
        if (GUILayout.Button("转换"))
        {
            //DirectoryInfo directoryInfo = new DirectoryInfo(m_path1);
            //FileInfo[] fileInfos = directoryInfo.GetFiles();
            //int count = 0;
            //foreach (FileInfo fileInfo in fileInfos)
            //{
            //    if (fileInfo.Name.EndsWith(".meta"))
            //        continue;
            //    count++;
            //    FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            //    BinaryReader br = new BinaryReader(fs);
            //    byte[] bytes = new byte[br.BaseStream.Length];
            //    br.Read(bytes, 0, (int)br.BaseStream.Length);
            //    br.Close();
            //    fs.Close();
            //    System.IO.File.WriteAllBytes(m_path2 + "/" + count + ".png", bytes);
            //    //fs.Read(bytes, 0, (int)fs.Length);
            //    //fs.Close();
            //    //fs = new FileStream(m_path2 + "/" + count + ".png", FileMode.OpenOrCreate, FileAccess.Write);
            //    //fs.Write(bytes, 0, bytes.Length);
            //    //fs.Close();
            //}
            //AssetDatabase.Refresh();

            //DirectoryInfo directoryInfo = new DirectoryInfo(m_path2);
            //FileInfo[] fileInfos = directoryInfo.GetFiles();
            //foreach (FileInfo fileInfo in fileInfos)
            //{
            //    if (fileInfo.Name.EndsWith(".meta"))
            //        continue;
            //    string str = fileInfo.Name;
            //    str = str.Replace(fileInfo.Extension, "");
            //    Object obj = AssetDatabase.LoadAssetAtPath(m_path2 + "/" + str, typeof(Object));
            //    Debug.Log(m_path2 + "/" + str);
            //}

            Object[] objects = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);

            foreach (Object obj in objects)
            {
                Texture2D tex = obj as Texture2D;
                if (!tex)
                    continue;
                Texture2D newTex = new Texture2D(tex.height, tex.width, TextureFormat.ARGB32, false);
                for (int i = 0; i < tex.height; i++)
                {
                    for (int j = 0; j < tex.width; j++)
                    {
                        Color c = tex.GetPixel(i, j);
                        if (c == Color.black)
                            c.a = 0;
                        newTex.SetPixel(i, j, c);
                    }
                }
                byte[] bytes = newTex.EncodeToPNG();
                FileStream fs = new FileStream(m_path2 + tex.name + ".png", FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndVertical();
    }

    private void GetAssetsPath()
    {
        m_path1 = AssetDatabase.GetAssetPath(Selection.activeObject);
        Repaint();
    }

    public void OnEnable()
    {
        Selection.selectionChanged += GetAssetsPath;
        Debug.Log("open");
    }

    public void OnDisable()
    {
        Selection.selectionChanged -= GetAssetsPath;
        Debug.Log("close");
    }
}
