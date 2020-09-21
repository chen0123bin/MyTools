using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIMenuItems  
{
    [MenuItem("GameObject/UIFramework/创建View", false, -101)]
    static void CreateViewObject()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        GameObject viewObj = new GameObject("View", typeof(RectTransform), typeof(CanvasGroup));      
        viewObj.transform.SetParent((Selection.activeObject as GameObject).transform, false);       
        Selection.activeObject = viewObj;
    }
    [MenuItem("GameObject/UIFramework/复制路径(Shift+C) #c", false, -101)]
    static void CopyParents()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        string path = (Selection.activeObject as GameObject).GetHierarchyPath();//GetParentPath(Selection.activeObject as GameObject, "");
        GUIUtility.systemCopyBuffer = path;
        Debug.Log(string.Format("systemCopyBuffer: {0}", path));
    }
    [MenuItem("GameObject/UIFramework/改变界面状态(Shift+T)  #t", false, -101)]
    static void ChangeViewState()
    {
        GameObject view = Selection.activeObject as GameObject;
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        canvasGroup.SetActive(canvasGroup.alpha == 0);
    }
}
