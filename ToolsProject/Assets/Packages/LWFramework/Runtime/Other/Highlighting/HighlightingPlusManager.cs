using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
public class HighlightingPlusManager : IHighlightingManager,IManager
{
    HighlightProfile _highlightProfile;
    public void Init()
    {
        _highlightProfile = Resources.Load<HighlightProfile>("HighlightPlusFastProfile");
    }
    public void Update()
    {
    }
    public void AddFlashingHighlighting(GameObject go, Color[] colorArray)
    {
       
    }

    public void AddHighlighting(GameObject go, Color color)
    {
        HighlightEffect highlightEffect = go.AddComponent<HighlightEffect>();
        highlightEffect.ProfileLoad(_highlightProfile);
        highlightEffect.outlineColor = color;
        highlightEffect.highlighted = true;
       
    } 
    public void RemoveHighlighting(GameObject go)
    {
        GameObject.Destroy(go.GetComponent<HighlightEffect>());
    }

   
}
