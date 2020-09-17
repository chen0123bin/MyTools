using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollViewContent : MonoBehaviour {
    private GridLayoutGroup gridGroup;
    private RectTransform rectTransform;
    private int childCount = 0;
    private float height;
    private bool isInit= false;
    public int columnCount = 1;
    private List<GameObject> childList;
	// Use this for initialization
	void Start () {
        Init();
    }
    void Init() {
        if (!isInit) {
            isInit = true;
            gridGroup = GetComponent<GridLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
            height = gridGroup.cellSize.y + gridGroup.spacing.y;
            childList = new List<GameObject>();
        }
    }
    public void AddChild(GameObject child) {
        Init();
        child.transform.SetParent(transform);
        child.transform.localScale = new Vector3(1, 1, 1);
        childCount++;
        childList.Add(child);
        ResetGroup();
    }

    private void ResetGroup() {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height * ((childCount/ columnCount)+1));
    }
    public void ClearChild() {

        for (int i = 0; i < childList.Count; i++)
        {
            Destroy(childList[i]);
        }
        ResetGroup();
        childCount = 0;
        childList.Clear();
    }
}
