using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;

public class StepControl_ChangeTexture : StepControl_Base
{
    private Renderer meshRenderer;
    private string oldTexture;
    private string newTexture;
    // Use this for initialization

    public override void StepStart() {
        base.StepStart();
        meshRenderer = GetComponent<Renderer>();
        //Texture2D texture = Resources.Load<Texture2D>(oldTexture);
        //Debug.Log(texture);
       
        //meshRenderer.materials[1].SetTexture("_MainTex", texture);

        SetTexture(oldTexture);
    }
    public override void StepEnd() {
        //Texture texture = Resources.Load<Texture>(newTexture);
        //meshRenderer.materials[1].SetTexture("_MainTex", texture);
        SetTexture(newTexture);
        base.StepEnd();
    }


    void SetTexture(string xmlStr)
    {
        if (xmlStr == "") return;
        string[] xmlValues = xmlStr.Split(';');
        for (int i = 0; i < xmlValues.Length; i++)
        {
            Debug.Log(xmlValues[i]);
            string[] filed = xmlValues[i].Split('=');

            Texture texture = Resources.Load<Texture>(filed[1]);
            meshRenderer.materials[int.Parse(filed[0])].SetTexture("_MainTex", texture);
        }
    }


    public override void InitEnd(object xmlData) {
        
        base.InitEnd(xmlData);
        XElement value = (XElement)xmlData;
        newTexture = value.Element("TextureResPath").Attribute("Path").Value;
    }
    public void InitStart(object xmlData)
    {
        XElement value = (XElement)xmlData;
        oldTexture = value.Element("TextureResPath").Attribute("Path").Value;
       
    }
}
