using UnityEngine;
using UnityEditor;
using System.Collections;


public class MyHierarchyMenu
{

    [MenuItem("Assets/复制Resources路径(Shift+R) #r")]
    static void CopyAssetPath()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        int startIndex = path.IndexOf("Resources") + "Resources".Length + 1;
        int length = path.LastIndexOf(".") - startIndex;
        path = path.Substring(startIndex, length);
        GUIUtility.systemCopyBuffer = path;
        Debug.Log(string.Format("systemCopyBuffer: {0}", path));
    }
    
    static string GetParentPath(GameObject child, string str)
    {
        if (child.transform.parent == null)
        {
            str = child.name + str;
            return str;
        }
        else
        {
            str = "/" + child.name + str;
            return GetParentPath(child.transform.parent.gameObject, str);
        }

    }
}