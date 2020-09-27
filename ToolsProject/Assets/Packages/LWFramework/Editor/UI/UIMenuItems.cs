using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    //重写Create->UI->Text事件  
    [MenuItem("GameObject/UI/Text")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            //如果选中的是列表里的Canvas  
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                //新建Text对象  
                GameObject go = new GameObject("Text", typeof(Text));
                //将raycastTarget置为false  
                go.GetComponent<Text>().raycastTarget = false;
                //设置其父物体  
                go.transform.SetParent(Selection.activeTransform,false);
                Selection.activeObject = go;
            }
        }
    }

    //重写Create->UI->Text事件  
    [MenuItem("GameObject/UI/Raw Image")]
    static void CreatRawImage()
    {
        if (Selection.activeTransform)
        {
            //如果选中的是列表里的Canvas  
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                //新建Text对象  
                GameObject go = new GameObject("RawImage", typeof(RawImage));
                //将raycastTarget置为false  
                go.GetComponent<RawImage>().raycastTarget = false;
                //设置其父物体  
                go.transform.SetParent(Selection.activeTransform,false);
                Selection.activeObject = go;
            }
        }
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
