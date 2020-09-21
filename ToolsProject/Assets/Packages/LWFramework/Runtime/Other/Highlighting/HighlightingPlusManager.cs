using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
public class HighlightingPlusManager : IHighlightingManager,IManager
{
    HighlightProfile m_HighlightProfile;
    public void Init()
    {
        m_HighlightProfile = Resources.Load<HighlightProfile>("HighlightPlusFastProfile");
    }
    public void Update()
    {
    }
    public void AddFlashingHighlighting(GameObject p_Go, Color[] p_ColorArray)
    {
       
    }

    public void AddHighlighting(GameObject p_GO, Color p_Color)
    {
        HighlightEffect highlightEffect = p_GO.AddComponent<HighlightEffect>();
        highlightEffect.ProfileLoad(m_HighlightProfile);
        highlightEffect.outlineColor = p_Color;
        highlightEffect.highlighted = true;
       
    } 
    public void RemoveHighlighting(GameObject p_GO)
    {
        GameObject.Destroy(p_GO.GetComponent<HighlightEffect>());
    }

   
}
